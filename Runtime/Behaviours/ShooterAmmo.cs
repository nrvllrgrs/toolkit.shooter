using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace ToolkitEngine.Weapons
{
	public class ShooterAmmo : MonoBehaviour, IAmmo
	{
		#region Enumerators

		public enum CacheSource
		{
			Internal,
			Direct,
			Parent,
			Custom
		}

		#endregion

		#region Fields

		[SerializeField]
		private ShooterControl[] m_shooterControls;

		[SerializeField]
		private CacheSource m_cacheSource;

		[SerializeField]
		private Ammo m_ammo;

		[SerializeField]
		private BaseAmmoCache m_ammoCache;

		[SerializeField]
		private AmmoType m_ammoType;

		[SerializeField, Min(1), Tooltip("Number of ammo consumed per shot (if continuous, consumed per second).")]
		private int m_consumedPerShot = 1;

		[SerializeField]
		private UnityEvent<int> m_onCountChanged;

		[SerializeField]
		private UnityEvent m_onEmpty;

		[SerializeField]
		private UnityEvent m_onDryFiring;

		private float m_partialConsumption = 0f;

		#endregion

		#region Properties

		/// <summary>
		/// Indicates whether there is enough ammo in cache to fire
		/// </summary>
		public bool canFire => count >= m_consumedPerShot;

		public AmmoType ammoType
		{
			get
			{
				switch (m_cacheSource)
				{
					case CacheSource.Internal:
						return m_ammo.ammoType;

					case CacheSource.Direct:
						return m_ammoCache?.ammoType;

					case CacheSource.Parent:
					case CacheSource.Custom:
						return m_ammoType;
				}
				return null;
			}
		}

		public int capacity
		{
			get
			{
				switch (m_cacheSource)
				{
					case CacheSource.Internal:
						return m_ammo.capacity;

					default:
						return m_ammoCache?.capacity ?? 0;
				}
			}
			set
			{
				switch (m_cacheSource)
				{
					case CacheSource.Internal:
						m_ammo.capacity = value;
						break;

					default:
						if (m_ammoCache != null)
						{
							m_ammoCache.capacity = value;
						}
						break;
				}
			}
		}

		public int count
		{
			get
			{
				switch (m_cacheSource)
				{
					case CacheSource.Internal:
						return m_ammo.count;

					default:
						return m_ammoCache?.count ?? 0;
				}
			}
			set
			{
				int prevCount;
				switch (m_cacheSource)
				{
					case CacheSource.Internal:
						prevCount = m_ammo.count;
						m_ammo.count = value;

						if (prevCount != m_ammo.count)
						{
							m_onCountChanged?.Invoke(m_ammo.count);
						}
						break;

					default:
						if (m_ammoCache != null)
						{
							prevCount = m_ammo.count;
							m_ammoCache.count = value;

							if (prevCount != m_ammo.count)
							{
								m_onCountChanged?.Invoke(m_ammoCache.count);
							}
						}
						break;
				}
			}
		}

		public float normalizedCount => (float)count / capacity;

		public UnityEvent<int> onCountChanged => m_onCountChanged;
		public UnityEvent onEmpty => m_onEmpty;
		public UnityEvent onDryFiring => m_onDryFiring;

		#endregion

		#region Methods

		private void Awake()
		{
			m_shooterControls = ShooterControl.GetShooterControls(gameObject, m_shooterControls);
			OnTransformParentChanged();
		}

		private void OnEnable()
		{
			foreach (var shooterControl in m_shooterControls)
			{
				shooterControl.onShotFiring.AddListener(ShooterControl_ShotFiring);
				shooterControl.onShotFired.AddListener(ShooterControl_ShotFired);
			}
		}

		private void OnDisable()
		{
			foreach (var shooterControl in m_shooterControls)
			{
				shooterControl.onShotFiring.RemoveListener(ShooterControl_ShotFiring);
				shooterControl.onShotFired.RemoveListener(ShooterControl_ShotFired);
			}
		}

		private void ShooterControl_ShotFiring(ShooterControl shooterControl)
		{
			if (count < m_consumedPerShot)
			{
				m_onDryFiring?.Invoke();
			}
		}

		private void ShooterControl_ShotFired(ShooterControl shooterControl)
		{
			Consume(shooterControl.fireType != ShooterControl.FireType.Continuous);
		}

		public void Consume(bool wholeNumbers = true)
		{
			if (!canFire)
				return;

			if (wholeNumbers)
			{
				count -= m_consumedPerShot;
			}
			else if (ConsumePartial())
			{
				count -= 1;
			}
		}

		private bool ConsumePartial()
		{
			m_partialConsumption += m_consumedPerShot * Time.deltaTime;
			if (m_partialConsumption >= 1f)
			{
				--m_partialConsumption;
				return true;
			}

			return false;
		}

		private void OnTransformParentChanged()
		{
			if (m_cacheSource == CacheSource.Parent)
			{
				m_ammoCache = GetComponentsInParent<AmmoCache>()
					.FirstOrDefault(x => Equals(x.ammoType, m_ammoType));
			}
		}

		#endregion
	}
}