using UnityEngine;
using UnityEngine.Events;

namespace ToolkitEngine.Shooter
{
	public abstract class BaseMuzzleShooter : BaseShooter
	{
		#region Enumerators

		public enum SpreadDistribution
		{
			Random,
			Gaussian,
		}

		#endregion

		#region Fields

		[SerializeField]
		protected Transform m_muzzle;

		[SerializeField, Range(0f, 90f)]
		protected float m_spread;

		[SerializeField]
		protected SpreadDistribution m_distribution;

		[SerializeField, Min(0f)]
		protected float m_mean;

		[SerializeField, Min(0f)]
		protected float m_deviation;

		[SerializeField]
		private UnityEvent<float> m_onSpreadChanged;

		#endregion

		#region Properties

		public Transform muzzle
		{
			get
			{
				if (m_muzzle == null)
				{
					m_muzzle = transform;
				}
				return m_muzzle;
			}
		}

		public float spread
		{
			get => m_spread;
			set
			{
				// No change, skip
				if (m_spread == value)
					return;

				m_spread = value;
				m_onSpreadChanged?.Invoke(value);
			}
		}

		public SpreadDistribution distribution => m_distribution;

		public UnityEvent<float> onSpreadChanged => m_onSpreadChanged;

		#endregion

		#region Static Methods

		public static Vector3 GetShotDirection(BaseMuzzleShooter shooter)
		{
			if (Mathf.Approximately(shooter.spread, 0f))
				return shooter.muzzle.forward;

			float radius = 0f;
			switch (shooter.distribution)
			{
				case SpreadDistribution.Random:
					var halfSpread = shooter.spread * 0.5f;
					radius = Random.Range(-halfSpread, halfSpread);
					break;

				case SpreadDistribution.Gaussian:
					radius = Mathf.Clamp(MathUtil.NextGaussian(0f, 0.2f), -1f, 1f) * shooter.spread;
					break;
			}

			float angle = Random.Range(-180f, 180f);
			return (Quaternion.AngleAxis(angle, shooter.muzzle.forward) * Quaternion.AngleAxis(radius, shooter.muzzle.right) * shooter.muzzle.forward).normalized;
		}

		#endregion

		#region Editor-Only
#if UNITY_EDITOR

		protected virtual void OnDrawGizmosSelected()
		{
			var muzzle = m_muzzle != null
				? m_muzzle
			: transform;

			GizmosUtil.DrawCone(muzzle.position, spread, 1f, muzzle.forward, Color.white);
		}

#endif
		#endregion
	}
}