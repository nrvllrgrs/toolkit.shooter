using UnityEngine;
using UnityEngine.Events;

namespace ToolkitEngine.Shooter
{
	[AddComponentMenu("Weapon/Shooter Charge")]
	public class ShooterCharge : MonoBehaviour, IPoolItemRecyclable
    {
		#region Fields

		[SerializeField]
		private ShooterControl[] m_shooterControls;

		/// <summary>
		/// Seconds to charge
		/// </summary>
		[SerializeField, Min(0f), Tooltip("Number of seconds before control can shoot")]
		private float m_maxCharge;

		/// <summary>
		/// Indicates whether charge is reset when "Fire" ends (otherwise, value charges down)
		/// </summary>
		[SerializeField, Tooltip("Indicates whether charge is reset when firing ends (otherwise, value charges down)")]
		private bool m_resetOnEndFire;

		private bool m_isCharging = false;
		private TimedCurve m_timedCurve = null;

		#endregion

		#region Events

		[SerializeField]
		private UnityEvent<float> m_onValueChanged;

		[SerializeField]
		private UnityEvent m_onCharging;

		[SerializeField]
		private UnityEvent m_onCharged;

		[SerializeField]
		private UnityEvent m_onDecharging;

		[SerializeField]
		private UnityEvent m_onDecharged;

		#endregion

		#region Properties

		public float value => m_timedCurve.time;
		public float normalizedValue => m_timedCurve.normalizedTime;
		public float maxCharge
		{
			get => m_maxCharge;
			set => m_maxCharge = m_timedCurve.duration = value;
		}

		/// <summary>
		/// Indicates whether is being charged.
		/// </summary>
		public bool isCharging
		{
			get => m_isCharging;
			private set
			{
				// No change, skip
				if (m_isCharging == value)
					return;

				m_isCharging = value;

				if (value)
				{
					m_onCharging?.Invoke();
					m_timedCurve.PlayForward();
				}
				else
				{
					m_onDecharging?.Invoke();
					if (m_resetOnEndFire)
					{
						ForceDecharged();
					}
					else
					{
						m_timedCurve.PlayBackwards();
					}
				}
			}
		}

		public bool isCharged => normalizedValue == 1f;

		public UnityEvent<float> onValueChanged => m_onValueChanged;
		public UnityEvent onCharging => m_onCharging;
		public UnityEvent onCharged => m_onCharged;
		public UnityEvent onDecharging => m_onDecharging;
		public UnityEvent onDecharged => m_onDecharged;

		#endregion

		#region Methods

		public void Recycle()
		{
			ForceDecharged(true);
		}

		private void Awake()
		{
			m_shooterControls = ShooterControl.GetShooterControls(gameObject, m_shooterControls);

			var obj = new GameObject();
			obj.hideFlags |= HideFlags.HideAndDontSave;
			obj.transform.SetParent(transform, false);

			m_timedCurve = obj.AddComponent<TimedCurve>();
			m_timedCurve.duration = m_maxCharge;
			m_timedCurve.OnValueChanged.AddListener(TimedCurve_ValueChanged);
			m_timedCurve.OnBeginCompleted.AddListener(TimedCurve_BeginCompleted);
			m_timedCurve.OnEndCompleted.AddListener(TimedCurve_EndCompleted);
		}

		private void OnEnable()
		{
			foreach (var shooterControl in m_shooterControls)
			{
				shooterControl.onBeginFire.AddListener(ShooterControl_BeginFire);
				shooterControl.onEndFire.AddListener(ShooterControl_EndFire);
			}
		}

		private void OnDisable()
		{
			foreach (var shooterControl in m_shooterControls)
			{
				shooterControl.onBeginFire.RemoveListener(ShooterControl_BeginFire);
				shooterControl.onEndFire.RemoveListener(ShooterControl_EndFire);
			}
			ForceDecharged(true);
		}

		private void ForceDecharged(bool silent = false)
		{
			m_isCharging = false;

			if (m_timedCurve != null)
			{
				m_timedCurve.Pause();
				m_timedCurve.time = 0f;
			}

			if (!silent)
			{
				// Invoke decharged event
				TimedCurve_BeginCompleted(m_timedCurve);
			}
		}

		private void ShooterControl_BeginFire(ShooterControl shooterControl)
		{
			isCharging = true;
		}

		private void ShooterControl_EndFire(ShooterControl shooterControl)
		{
			isCharging = false;
		}

		private void TimedCurve_ValueChanged(float value)
		{
			m_onValueChanged?.Invoke(value);
		}

		private void TimedCurve_BeginCompleted(TimedCurve timedCurve)
		{
			m_onDecharged?.Invoke();
		}

		private void TimedCurve_EndCompleted(TimedCurve timedCurve)
		{
			m_onCharged?.Invoke();
		}

		#endregion
	}
}