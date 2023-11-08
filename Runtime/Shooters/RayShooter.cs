using System.Collections.Generic;
using UnityEngine;
using ToolkitEngine.Health;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ToolkitEngine.Shooter
{
	[AddComponentMenu("Weapon/Shooter/Ray Shooter")]
	public class RayShooter : BaseMuzzleShooter
	{
		public delegate bool RaycastFunc(Vector3 origin, Vector3 direction, out RaycastHit hit);
		public delegate RaycastHit[] RaycastAllFunc(Vector3 origin, Vector3 direction);

		#region Fields

		[SerializeField, Min(0f)]
		private float m_radius;

		[SerializeField]
		private BaseMuzzleShooterPattern m_pattern;

		[SerializeField]
		private LayerMask m_layerMask = ~0;

		[SerializeField, Min(-1)]
        private int m_penetrateCount;

		[SerializeField]
		private LayerMask m_blockingMask = ~0;

        [SerializeField]
        private ImpactDamage m_impactDamage;

		[SerializeField]
		private SplashDamage m_splashDamage;

		[SerializeField]
		private UnityEvent<ShooterEventArgs, int> m_onPenetrated;

		#endregion

		#region Properties

		public bool infinitePenetrate => m_penetrateCount < 0;

		public UnityEvent<ShooterEventArgs, int> onPenetrated => m_onPenetrated;

		#endregion

		#region Methods

		[ContextMenu("Fire")]
		public override void Fire(ShooterControl shooterControl)
        {
			if (m_pattern == null)
			{
				Fire(shooterControl, muzzle.position, GetShotDirection(this));
			}
			else
			{
				foreach (var ray in m_pattern.GetShotRays(this))
				{
					Fire(shooterControl, ray.origin, ray.direction);
				}
			}
		}

		protected void Fire(ShooterControl shooterControl, Vector3 origin, Vector3 direction)
		{
			ShooterEventArgs args = new()
			{
				shooter = this,
				origin = origin,
				terminal = origin + muzzle.forward * m_impactDamage.range
			};
			m_onFiring?.Invoke(args);

			if (Mathf.Approximately(m_radius, 0f))
			{
				// Shoot ray
				Fire(shooterControl, origin, direction, GetRaycast, GetRaycastAll);
			}
			else
			{
				// Shoot beam
				Fire(shooterControl, origin, direction, GetSphereCast, GetSphereCastAll);
			}
		}

		protected void Fire(ShooterControl shooterControl, Vector3 origin, Vector3 direction, RaycastFunc raycastFunc, RaycastAllFunc raycastAllFunc)
		{
			ShooterEventArgs args = new()
			{
				shooter = this,
				origin = origin,
			};

			List<DamageHit> hits = new();
			HashSet<IDamageReceiver> victims = new();

			// No penetration
			if (m_penetrateCount == 0)
			{
				if (ProcessHit(shooterControl, raycastFunc(origin, direction, out RaycastHit hit), hit, hits, victims))
				{
					args.terminal = hit.point;
				}
				else
				{
					ProcessTerminalPoint(origin, direction, hits, args);
				}
			}
			// Penetration
			else
			{
				int remainingPenetrateCount = m_penetrateCount;
				foreach (var hit in raycastAllFunc(origin, direction))
				{
					if (!ProcessHit(shooterControl, true, hit, hits, victims))
						continue;

					m_onPenetrated?.Invoke(args, hits.Count);

					// Hit blocking layer, exit
					if ((hit.collider.gameObject.layer | m_blockingMask) != 0)
						break;

					// Hit maximum number of penetrations, exit
					if (infinitePenetrate && --remainingPenetrateCount == 0)
						break;
				}

				if (remainingPenetrateCount > 0)
				{
					ProcessTerminalPoint(origin, direction, hits, args);
				}
			}

			args.hits = hits.ToArray();
			m_onFired?.Invoke(args);
		}

		protected bool ProcessHit(ShooterControl shooterControl, bool result, RaycastHit raycastHit, List<DamageHit> hits, HashSet<IDamageReceiver> victims)
		{
			if (result)
			{
				var health = raycastHit.collider.GetComponentInParent<IDamageReceiver>();
				if (health != null)
				{
					// Already hit this object, skip
					if (victims.Contains(health))
						return false;

					// Add to damaged set
					victims.Add(health);

					bool continuous = false;
					if (shooterControl?.fireType == ShooterControl.FireType.Continuous)
					{
						continuous = true;
					}

					var hit = new DamageHit(m_impactDamage, continuous)
					{
						source = gameObject,
						victim = health,
						collider = raycastHit.collider,
						origin = muzzle.position,
						contact = raycastHit.point,
					};

					// Apply impact damage to victim
					m_impactDamage.Apply(hit);
					hits.Add(hit);
				}

				// Apply splash damage at contact point
				hits.AddRange(
					m_splashDamage.Apply(raycastHit.point, gameObject));
			}

			return result;
		}

		private void ProcessTerminalPoint(Vector3 origin, Vector3 direction, List<DamageHit> hits, ShooterEventArgs args)
		{
			var terminal = origin + direction * m_impactDamage.visualRange;
			args.terminal = terminal;

			if (!m_impactDamage.hasInfiniteRange)
			{
				// Apply splash damage at terminal point in space
				hits.AddRange(
					m_splashDamage.Apply(terminal, gameObject));
			}
		}

		#endregion

		#region Physics Helper Methods

		protected bool GetRaycast(Vector3 origin, Vector3 direction, out RaycastHit hit)
		{
			return Physics.Raycast(origin, direction, out hit, m_impactDamage.range, m_layerMask, QueryTriggerInteraction.Ignore);
		}

		protected RaycastHit[] GetRaycastAll(Vector3 origin, Vector3 direction)
		{
			return Physics.RaycastAll(origin, direction, m_impactDamage.range, m_layerMask, QueryTriggerInteraction.Ignore);
		}

		protected bool GetSphereCast(Vector3 origin, Vector3 direction, out RaycastHit hit)
		{
			return Physics.SphereCast(origin, m_radius, direction, out hit, m_impactDamage.range, m_layerMask, QueryTriggerInteraction.Ignore);
		}

		protected RaycastHit[] GetSphereCastAll(Vector3 origin, Vector3 direction)
		{
			return Physics.SphereCastAll(origin, m_radius, direction, m_impactDamage.range, m_layerMask, QueryTriggerInteraction.Ignore);
		}

		#endregion

		#region Editor-Only
#if UNITY_EDITOR

		protected override void OnDrawGizmosSelected()
		{
			base.OnDrawGizmosSelected();

			var muzzle = m_muzzle != null
				? m_muzzle
				: transform;

			var range = m_impactDamage.range;
			if (float.IsInfinity(range))
			{
				range = Camera.main.farClipPlane;
			}
			var far = muzzle.position + muzzle.forward * range;

			if (m_radius > 0)
			{
				Handles.DrawWireDisc(muzzle.position, muzzle.forward, m_radius);
				Handles.DrawWireDisc(far, muzzle.forward, m_radius);

				Gizmos.DrawLine(muzzle.position + (muzzle.up * m_radius), far + (muzzle.up * m_radius));
				Gizmos.DrawLine(muzzle.position + (muzzle.up * -m_radius), far + (muzzle.up * -m_radius));
				Gizmos.DrawLine(muzzle.position + (muzzle.right * m_radius), far + (muzzle.right * m_radius));
				Gizmos.DrawLine(muzzle.position + (muzzle.right * -m_radius), far + (muzzle.right * -m_radius));
			}
			else
			{
				Gizmos.DrawLine(muzzle.position, far);
			}

			if (m_splashDamage.radius > 0f)
			{
				Gizmos.DrawWireSphere(far, m_splashDamage.radius);
			}
		}

#endif
		#endregion
	}
}