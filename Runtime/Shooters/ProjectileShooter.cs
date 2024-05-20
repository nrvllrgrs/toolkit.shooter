using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ToolkitEngine.Health;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

namespace ToolkitEngine.Shooter
{
	[AddComponentMenu("Weapon/Shooter/Projectile Shooter")]
	public class ProjectileShooter : BaseMuzzleShooter, IDamageShooter, IProjectileEvents
	{
		#region Enumerators

		public enum TerminalAction
		{
			Custom,
			Detonate,
			Attach,
		}

		#endregion

		#region Fields

		[SerializeField]
		private Spawner m_projectileSpawner;

		[SerializeField]
		private List<Collider> m_ignoredColliders;

		[SerializeField]
		private BaseMuzzleShooterPattern m_pattern;

		[SerializeField, MaxInfinity, Min(-1f), Tooltip("Seconds projectile is alive.")]
		private float m_lifetime = float.PositiveInfinity;

		[SerializeField, Min(0f)]
		private float m_speed = 10f;

		[SerializeField]
		private float m_acceleration;

		[SerializeField, Min(0f)]
		private Vector2 m_speedLimits = Vector2.zero;

		[SerializeField]
		private bool m_alignToTrajectory;

		[SerializeField]
		private TerminalAction m_maxDistanceAction = TerminalAction.Detonate;

		[SerializeField]
		private TerminalAction m_collisionAction = TerminalAction.Detonate;

		[SerializeField]
		private ImpactDamage m_impactDamage;

		[SerializeField]
		private SplashDamage m_splashDamage;

		private Projectile m_pendingProjectile;

		private Coroutine m_projectileThread = null;
		private HashSet<Projectile> m_activeProjectiles = new();

		#endregion

		#region Events

		[SerializeField]
		private UnityEvent<ProjectileEventArgs> m_onProjectileFired;

		[SerializeField]
		private UnityEvent<ProjectileEventArgs> m_onMaxDistanceReached;

		[SerializeField]
		private UnityEvent<ProjectileEventArgs> m_onCollision;

		[SerializeField]
		private UnityEvent<ProjectileEventArgs> m_onDetonated;

		#endregion

		#region Properties

		public Projectile pendingProjectile
		{
			get => m_pendingProjectile;
			set
			{
				if (m_pattern != null)
					throw new System.NotSupportedException("Cannot have pending projectile with shooter pattern.");

				// No change, skip
				if (m_pendingProjectile == value)
					return;

				// Pending projectile already defined, skip
				if (value != null && m_pendingProjectile != null)
					return;

				if (m_pendingProjectile != null && !m_activeProjectiles.Contains(m_pendingProjectile))
				{
					Untrack(m_pendingProjectile, false);
				}

				m_pendingProjectile = value;

				if (m_pendingProjectile != null)
				{
					Track(m_pendingProjectile);
				}
			}
		}
		public Set[] sets => m_projectileSpawner.sets;
		public float lifetime => m_lifetime;
		public float speed { get => m_speed; set => m_speed = value; }
		public float acceleration => m_acceleration;
		public float minSpeed => m_acceleration < 0 ? m_speedLimits.x : m_speed;
		public float maxSpeed => m_acceleration > 0 ? m_speedLimits.y : m_speed;
		public ImpactDamage impactDamage => m_impactDamage;
		public SplashDamage splashDamage => m_splashDamage;

		public UnityEvent<ProjectileEventArgs> onProjectileFired => m_onProjectileFired;
		public UnityEvent<ProjectileEventArgs> onMaxDistanceReached => m_onMaxDistanceReached;
		public UnityEvent<ProjectileEventArgs> onCollision => m_onCollision;
		public UnityEvent<ProjectileEventArgs> onDetonated => m_onDetonated;

		#endregion

		#region Methods

		private void Awake()
		{
			m_projectileSpawner.actionOnSpawned = _OnSpawned;
			m_projectileSpawner.actionOnDespawned = _OnDespawned;
		}

		public void IgnoreCollider(Collider collider, bool ignore = true)
		{
			if (ignore)
			{
				if (!m_ignoredColliders.Contains(collider))
				{
					m_ignoredColliders.Add(collider);
				}
			}
			else
			{
				if (m_ignoredColliders.Contains(collider))
				{
					m_ignoredColliders.Remove(collider);
				}
			}
		}

		public void IgnoreColliders(IEnumerable<Collider> colliders, bool ignore = true)
		{
			if (ignore)
			{
				foreach (var collider in colliders)
				{
					if (!m_ignoredColliders.Contains(collider))
					{
						m_ignoredColliders.Add(collider);
					}
				}
			}
			else
			{
				foreach (var collider in colliders)
				{
					if (m_ignoredColliders.Contains(collider))
					{
						m_ignoredColliders.Remove(collider);
					}
				}
			}
		}

		#endregion

		#region Spawner Methods

		private void _OnSpawned(GameObject obj)
		{
			var projectile = obj.GetComponent<Projectile>();
			if (projectile == null)
				return;

			Track(projectile);
		}

		private void _OnDespawned(GameObject obj)
		{
			var projectile = obj.GetComponent<Projectile>();
			if (projectile == null)
				return;

			Untrack(projectile, true);

			if (m_activeProjectiles.Count == 0)
			{
				this.CancelCoroutine(ref m_projectileThread);
			}
		}

		#endregion

		#region Shooter Methods

		[ContextMenu("Fire")]
		public override void Fire(ShooterControl shooterControl)
        {
			if (m_pattern == null)
			{
				if (m_pendingProjectile == null)
				{
					Spawn(muzzle.position, GetShotDirection(this), true);
				}
				else
				{
					m_pendingProjectile.transform.rotation = Quaternion.LookRotation(GetShotDirection(this));

					// Add pending projectile to sets before firing
					foreach (var set in m_projectileSpawner.sets)
					{
						set.Add(m_pendingProjectile.gameObject);
					}

					Fire(m_pendingProjectile);
					pendingProjectile = null;
				}
			}
			else
			{
				foreach (var ray in m_pattern.GetShotRays(this))
				{
					Fire(ray.origin, ray.direction);
				}
			}
		}

		protected void Fire(Vector3 origin, Vector3 direction)
		{
			Spawn(origin, direction, true);
		}

		#endregion

		#region Projectile Methods

		private void Track(Projectile projectile)
		{
			projectile.transform.SetPositionAndRotation(muzzle.position, muzzle.rotation);
			projectile.transform.SetParent(muzzle);

			// Projectile should not hit colliders on shooter
			foreach (var collider in m_ignoredColliders)
			{
				foreach (var projectileCollider in projectile.colliders)
				{
					Physics.IgnoreCollision(collider, projectileCollider, true);
				}
			}

			projectile.onFired.AddListener(Projectile_Fired);
			projectile.onMaxDistanceReached.AddListener(Projectile_MaxDistanceReached);
			projectile.onCollision.AddListener(Projectile_Collision);
			projectile.onDetonated.AddListener(Projectile_Detonated);

			projectile.rigidbody.velocity = projectile.rigidbody.angularVelocity = Vector3.zero;
			projectile.rigidbody.isKinematic = true;
		}

		private void Untrack(Projectile projectile, bool removeFromActiveProjectiles)
		{
			// Projectile should not hit colliders on shooter
			foreach (var collider in m_ignoredColliders)
			{
				foreach (var projectileCollider in projectile.colliders)
				{
					Physics.IgnoreCollision(collider, projectileCollider, false);
				}
			}

			projectile.onFired.RemoveListener(Projectile_Fired);
			projectile.onMaxDistanceReached.RemoveListener(Projectile_MaxDistanceReached);
			projectile.onCollision.RemoveListener(Projectile_Collision);
			projectile.onDetonated.RemoveListener(Projectile_Detonated);

			if (removeFromActiveProjectiles)
			{
				m_activeProjectiles.Remove(projectile);
			}
		}

		public void Untrack(Projectile projectile)
		{
			Untrack(projectile, true);
		}

		private void Fire(Projectile projectile)
		{
			if (!enabled)
				return;

			ShooterEventArgs args = new()
			{
				shooter = this,
				origin = projectile.transform.position
			};
			m_onFiring?.Invoke(args);

			projectile.transform.SetParent(null);
			projectile.rigidbody.isKinematic = false;
			projectile.rigidbody.velocity = muzzle.forward * speed;

			// Add projectile to active set
			m_activeProjectiles.Add(projectile);

			projectile.Fire(this);
			m_onFired?.Invoke(args);

			if (m_projectileThread == null)
			{
				m_projectileThread = StartCoroutine(AsyncUpdateProjectile());
			}
		}

		private IEnumerator AsyncUpdateProjectile()
		{
			while (m_activeProjectiles.Count > 0)
			{
				var projectiles = m_activeProjectiles.ToArray();
				foreach (var projectile in projectiles)
				{
					// Update lifetime, if necessary
					if (projectile.lifetime >= 0f)
					{
						projectile.lifetime -= Time.fixedDeltaTime;
						if (Mathf.Approximately(projectile.lifetime, 0f))
						{
							m_activeProjectiles.Remove(projectile);
							projectile.Detonate();
							continue;
						}
					}

					if (m_impactDamage.range >= 0f || m_acceleration != 0f)
					{
						// Cache magnitude of velocity to minimize sqrt calls
						var speed = projectile.rigidbody.velocity.magnitude;

						// Update distance, if necessary
						if (m_impactDamage.range >= 0f)
						{
							projectile.distance += speed * Time.fixedDeltaTime;
							if (projectile.distance >= m_impactDamage.range)
							{
								m_activeProjectiles.Remove(projectile);
								projectile.onMaxDistanceReached?.Invoke(new ProjectileEventArgs()
								{
									projectileShooter = this,
									projectile = projectile,
									terminal = projectile.transform.position,
									impactNormal = -projectile.transform.forward,
								});
								continue;
							}
						}

						// Update speed, if necessary
						if (m_acceleration != 0f)
						{
							if ((m_acceleration > 0f && speed < maxSpeed) || (m_acceleration < 0f && speed > minSpeed))
							{
								projectile.rigidbody.velocity += projectile.transform.forward * m_acceleration * Time.fixedDeltaTime;
							}
						}
					}

					if (m_alignToTrajectory)
					{
						projectile.transform.rotation = Quaternion.LookRotation(projectile.rigidbody.velocity, projectile.transform.up);
					}
				}

				yield return new WaitForFixedUpdate();
			}

			m_projectileThread = null;
		}

		[ContextMenu("Spawn")]
		public void Spawn()
		{
			Spawn(muzzle.position, muzzle.forward, false);
		}

		private void Spawn(Vector3 origin, Vector3 direction, bool autoFire)
		{
			m_projectileSpawner.Instantiate(origin, Quaternion.LookRotation(direction), muzzle, PostSpawn, autoFire);
		}

		private void PostSpawn(GameObject obj, params object[] args)
		{
			var projectile = obj.GetComponent<Projectile>();
			if (projectile == null)
				return;

			// If autofire...
			if ((bool)args[0])
			{
				Fire(projectile);
			}
			else
			{
				pendingProjectile = projectile;
				projectile.Assign(this);
			}
		}

		[ContextMenu("Despawn")]
		public void Despawn()
		{
			if (m_pendingProjectile == null)
				return;

			PoolItem.Destroy(m_pendingProjectile.gameObject);
		}

		[ContextMenu("Detonate All")]
		public void DenotateAll()
		{
			var spawns = m_projectileSpawner.spawns;
			foreach (var obj in spawns)
			{
				obj.GetComponent<Projectile>()?.Detonate();
			}
		}

		#endregion

		#region Projectile Callbacks

		private void Projectile_Fired(ProjectileEventArgs e)
		{
			m_onProjectileFired?.Invoke(e);
		}

		private void Projectile_MaxDistanceReached(ProjectileEventArgs e)
		{
			m_onMaxDistanceReached?.Invoke(e);
			ProcessTerminalAction(e, m_maxDistanceAction);
		}

		private void Projectile_Collision(ProjectileEventArgs e)
		{
			m_onCollision?.Invoke(e);
			ProcessTerminalAction(e, m_collisionAction);
		}

		private void ProcessTerminalAction(ProjectileEventArgs e, TerminalAction action)
		{
			switch (action)
			{
				case TerminalAction.Detonate:
					e.projectile.Detonate(e);
					break;

				case TerminalAction.Attach:
					Attach(e.projectile, e.collider);
					break;
			}

			m_activeProjectiles.Remove(e.projectile);
		}

		private void Projectile_Detonated(ProjectileEventArgs e)
		{
			m_onDetonated?.Invoke(e);
			Untrack(e.projectile);
		}

		public void Attach(Projectile projectile, Collider collider)
		{
			if (projectile == null)
				return;

			projectile.Attach(collider);

			// Stop tracking projectile for update and events
			Untrack(projectile);
		}

		#endregion

		#region Editor-Only
#if UNITY_EDITOR

		protected void OnDrawGizmosSelected()
		{
			var muzzle = m_muzzle != null
				? m_muzzle
				: transform;

			var range = m_impactDamage.range;
			if (float.IsInfinity(range))
			{
				range = Camera.main.farClipPlane;
			}

			var far = muzzle.position + muzzle.forward * range;
			Gizmos.DrawLine(muzzle.position, far);

			if (m_spread > 0f)
			{
				DrawSpread(range);
			}
		}

#endif
		#endregion
	}
}