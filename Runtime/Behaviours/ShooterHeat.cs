using UnityEngine;
using UnityEngine.Events;

namespace ToolkitEngine.Weapons
{
	[AddComponentMenu("Weapon/Shooter Heat")]
	public class ShooterHeat : MonoBehaviour, IPoolItemRecyclable, IHeat
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
		private Heat m_heat;

		[SerializeField]
		private BaseHeatCache m_heatCache;

		/// <summary>
		/// Heat per shot
		/// </summary>
		[SerializeField, Min(0f), Tooltip("Heat per shot (if continuous, heat per second)")]
		private float m_heatPerShot = 1f;

		#endregion

		#region Events

		[SerializeField]
		private UnityEvent<float> m_onValueChanged;

		[SerializeField]
		private UnityEvent m_onOverheated;

		[SerializeField]
		private UnityEvent m_onCooled;

		#endregion

		#region Properties

		public ShooterControl[] shooterControls => m_shooterControls;
		public float heatPerShot => m_heatPerShot;

		public float maximum
		{
			get => heatSource?.maximum ?? 0f;
			set
			{
				switch (m_cacheSource)
				{
					case CacheSource.Internal:
						m_heat.maximum = value;
						break;

					default:
						if (m_heatCache != null)
						{
							m_heatCache.maximum = value;
						}
						break;
				}
			}
		}

		public float value
		{
			get => heatSource?.value ?? 0f;
			set
			{
				switch (m_cacheSource)
				{
					case CacheSource.Internal:
						m_heat.value = value;
						break;

					default:
						if (m_heatCache != null)
						{
							m_heatCache.value = value;
						}
						break;
				}
			}
		}

		public float normalizedValue => value / maximum;

		public bool canConsumeWithoutOverheating => value + m_heatPerShot < maximum;

		public bool isOverheated => heatSource?.isOverheated ?? false;

		public bool paused
		{
			get => heatSource.paused;
			set
			{
				switch (m_cacheSource)
				{
					case CacheSource.Internal:
						m_heat.paused = value;
						break;

					default:
						if (m_heatCache != null)
						{
							m_heatCache.paused = value;
						}
						break;
				}
			}
		}

		protected IHeat heatSource
		{
			get
			{
				switch (m_cacheSource)
				{
					case CacheSource.Internal:
						return m_heat;

					default:
						return m_heatCache;
				}
			}
		}

		public UnityEvent<float> onValueChanged => m_onValueChanged;
		public UnityEvent onOverheated => m_onOverheated;
		public UnityEvent onCooled => m_onCooled;

		#endregion

		#region Methods

		public void Recycle()
		{
			Vent();
		}

		private void Awake()
		{
			m_shooterControls = ShooterControl.GetShooterControls(gameObject, m_shooterControls);
			OnTransformParentChanged();
		}

		private void OnEnable()
		{
			foreach (var shooterControl in m_shooterControls)
			{
				shooterControl.onShotFired.AddListener(ShooterControl_ShotFired);
			}

			RegisterEvents();
		}

		private void OnDisable()
		{
			foreach (var shooterControl in m_shooterControls)
			{
				shooterControl.onShotFired.RemoveListener(ShooterControl_ShotFired);
			}

			UnregisterEvents();
		}

		private void RegisterEvents()
		{
			heatSource?.onValueChanged.AddListener(HeatSouce_ValueChanged);
			heatSource?.onOverheated.AddListener(HeatSource_Overheated);
			heatSource?.onCooled.AddListener(HeatSource_Cooled);
		}

		private void UnregisterEvents()
		{
			heatSource?.onValueChanged.RemoveListener(HeatSouce_ValueChanged);
			heatSource?.onOverheated.RemoveListener(HeatSource_Overheated);
			heatSource?.onCooled.RemoveListener(HeatSource_Cooled);
		}

		private void ShooterControl_ShotFired(ShooterControl shooterControl)
		{
			if (!enabled)
				return;

			Consume(shooterControl.fireType == ShooterControl.FireType.Continuous);
		}

		private void HeatSouce_ValueChanged(float value)
		{
			m_onValueChanged?.Invoke(value);
		}

		private void HeatSource_Overheated()
		{
			m_onOverheated?.Invoke();
		}

		private void HeatSource_Cooled()
		{
			m_onCooled?.Invoke();
		}

		private void OnTransformParentChanged()
		{
			if (m_cacheSource == CacheSource.Parent)
			{
				UnregisterEvents();
				m_heatCache = GetComponentInParent<HeatCache>();
				RegisterEvents();
			}
		}

		public void Consume(bool continuous = false)
		{
			value += continuous
				? m_heatPerShot * Time.deltaTime
				: m_heatPerShot;
		}

		public void Overheat()
		{
			switch (m_cacheSource)
			{
				case CacheSource.Internal:
					m_heat.Overheat();
					break;

				default:
					m_heatCache?.Overheat();
					break;
			}
		}

		public void Vent()
		{
			switch (m_cacheSource)
			{
				case CacheSource.Internal:
					m_heat.Vent();
					break;

				default:
					m_heatCache?.Vent();
					break;
			}
		}

		#endregion
	}
}