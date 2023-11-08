using ToolkitEngine.Health;
using UnityEngine;
using UnityEngine.Events;

namespace ToolkitEngine.Shooter
{
	[System.Serializable]
	public class ProjectileEventArgs : System.EventArgs
	{
		public ProjectileShooter projectileShooter;
		public Projectile projectile;
		public Vector3 terminal;
		public Vector3 impactNormal;
		public Vector3 surfaceNormal;
		public Collider collider;
		public DamageHit[] hits;
	}

	[AddComponentMenu("Weapon/Projectile")]
    [RequireComponent(typeof(Rigidbody))]
	public class Projectile : MonoBehaviour
	{
		#region Fields

		[SerializeField]
		private UnityEvent<ProjectileEventArgs> m_onFired;

		[SerializeField]
		private UnityEvent<ProjectileEventArgs> m_onMaxDistanceReached;

		[SerializeField]
		private UnityEvent<ProjectileEventArgs> m_onCollision;

		[SerializeField]
		private UnityEvent<ProjectileEventArgs> m_onDetonated;

		private Rigidbody m_rigidbody;
		private Collider[] m_colliders = null;

		private ProjectileShooter m_projectileShooter;
		private float m_lifetime;
		private float m_distance;
		private ImpactDamage m_impactDamage = null;
		private SplashDamage m_splashDamage = null;

		#endregion

		#region Properties

		public ProjectileShooter projectileShooter => m_projectileShooter;
		public new Rigidbody rigidbody => this.GetComponent(ref m_rigidbody);
		public Collider[] colliders => this.GetComponentsInChildren(ref m_colliders, true);
		public bool isPending { get; private set; }
		public float lifetime
		{
			get => m_lifetime;
			internal set
			{
				value = Mathf.Clamp(value, 0f, m_projectileShooter?.lifetime ?? 0f);

				// No chage, skip
				if (m_lifetime == value)
					return;

				m_lifetime = value;
			}
		}
		public float normalizedLifetime => m_projectileShooter != null ? 1f - (m_lifetime / m_projectileShooter.lifetime) : 0f;
		public float distance { get; internal set; }
		public float normalizedDistance
		{
			get
			{
				if (m_impactDamage == null || float.IsInfinity(m_impactDamage.range))
					return 0f;

				return m_impactDamage.range / distance;
			}
		}

		public UnityEvent<ProjectileEventArgs> onFired => m_onFired;
		public UnityEvent<ProjectileEventArgs> onMaxDistanceReached => m_onMaxDistanceReached;
		public UnityEvent<ProjectileEventArgs> onCollision => m_onCollision;
		public UnityEvent<ProjectileEventArgs> onDetonated => m_onDetonated;

		#endregion

		#region Methods

		private void Awake()
		{
			m_rigidbody = GetComponent<Rigidbody>();
		}

		private void OnEnable()
		{
			isPending = true;
		}

		private void OnDisable()
		{
			isPending = false;
		}

		internal void Assign(ProjectileShooter projectileShooter)
		{
			m_projectileShooter = projectileShooter;
		}

		public void Fire(ProjectileShooter projectileShooter)
		{
			Assign(projectileShooter);
			isPending = false;

			// Set initial values
			distance = 0f;
			lifetime = projectileShooter.lifetime;

			// Copy damage from shooter so damage is associated with time shot (not time detonated)
			m_impactDamage = new ImpactDamage(projectileShooter.impactDamage);
			m_splashDamage = new SplashDamage(projectileShooter.splashDamage);

			m_onFired?.Invoke(new ProjectileEventArgs()
			{
				projectileShooter = m_projectileShooter,
				projectile = this,
			});
		}

		public void Stop()
		{
			// Stop physics simulation
			rigidbody.isKinematic = true;
			rigidbody.velocity = rigidbody.angularVelocity = Vector3.zero;
		}

		public void Attach(Collider collider)
		{
			Stop();
			if (collider != null)
			{
				// Attach projectile hit collider
				transform.SetParent(collider.transform);
			}
		}

		public void Detonate()
		{
			Detonate(null);
		}

		public void Detonate(ProjectileEventArgs e)
		{
			var hits = m_splashDamage?.Apply(transform.position, m_projectileShooter?.gameObject ?? gameObject);

			if (m_onDetonated != null)
			{
				ProjectileEventArgs args;
				if (e != null)
				{
					args = new ProjectileEventArgs()
					{
						projectileShooter = e.projectileShooter,
						projectile = e.projectile,
						terminal = e.terminal,
						impactNormal = e.impactNormal,
						surfaceNormal = e.surfaceNormal,
						collider = e.collider,
					};
				}
				else
				{
					args = new ProjectileEventArgs()
					{
						projectileShooter = m_projectileShooter,
						projectile = this,
						terminal = transform.position,
					};
				}
				args.hits = hits;

				m_onDetonated.Invoke(args);
			}
		}

		private void OnCollisionEnter(Collision collision)
		{
			var hit = new DamageHit(m_impactDamage)
			{
				source = gameObject,
				victim = collision.collider.GetComponentInParent<IHealth>(),
				collider = collision.collider,
				origin = collision.contacts[0].point + collision.contacts[0].normal,
				contact = collision.contacts[0].point,
				distance = m_distance
			};
			m_impactDamage?.Apply(hit);

			if (m_onCollision != null)
			{
				var args = GetProjectileEventsArgs(collision);
				args.hits = new[] { hit };

				m_onCollision.Invoke(args);
			}
		}

		private ProjectileEventArgs GetProjectileEventsArgs(Collision collision)
		{
			return new()
			{
				projectileShooter = m_projectileShooter,
				projectile = this,
				terminal = collision?.GetContact(0).point ?? transform.position,
				impactNormal = -transform.forward,
				surfaceNormal = collision?.GetContact(0).normal ?? Vector3.zero,
				collider = collision?.collider,
			};
		}

		#endregion
	}
}