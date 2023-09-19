using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace ToolkitEngine.Weapons
{
	[Serializable]
    public class Heat : IHeat
    {
		#region Fields

		/// <summary>
		/// Maximum allowed heat before overheating
		/// </summary>
		[SerializeField, Min(0f), Tooltip("Maximum allowed heat before overheating")]
		private float m_maximum = 10f;

		/// <summary>
		/// Seconds to wait before heat loss begins while not overheated
		/// </summary>
		[SerializeField, Min(0f), Tooltip("Seconds to wait before heat loss begins while not overheated")]
		private float m_coolDelay;

		/// <summary>
		/// Heat loss per second while not overheated
		/// </summary>
		[SerializeField, Min(0f), Tooltip("Heat loss per second while not overheated")]
		private float m_coolRate;

		/// <summary>
		/// Seconds to wait before heat loss begins while overheated
		/// </summary>
		[SerializeField, Min(0f), Tooltip("Seconds to wait before heat loss begins while overheated")]
		private float m_overheatDelay;

		/// <summary>
		/// Heat loss per second while overheated
		/// </summary>
		[SerializeField, Min(0f), Tooltip("Heat loss per second while overheated")]
		private float m_overheatRate;

		private float m_value;
		private bool m_isOverheated;
		private Coroutine m_cooldownThread = null;

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

		public float maximum
		{
			get => m_maximum;
			set
			{
				// No change, skip
				if (m_maximum == value)
					return;

				m_maximum = value;
				this.value = Mathf.Max(m_maximum, m_value);
			}
		}

		public float value
		{
			get => m_value;
			set
			{
				value = Mathf.Clamp(value, 0f, m_maximum);

				// No change, skip
				if (m_value == value)
					return;

				int compare = value.CompareTo(m_value);

				m_value = value;
				m_onValueChanged.Invoke(value);

				// If new value is greater than old value...
				if (compare > 0)
				{
					if (m_value == m_maximum)
					{
						isOverheated = true;
					}
					else
					{
						m_cooldownThread = CoroutineUtil.GlobalStartCoroutine(Cooldown(m_coolDelay, m_coolRate));
					}
				}
			}
		}

		public float normalizedValue => m_value / m_maximum;

		public bool isOverheated
		{
			get => m_isOverheated;
			protected set
			{
				// No change, skip
				if (m_isOverheated == value)
					return;

				m_isOverheated = value;

				if (value)
				{
					m_cooldownThread = CoroutineUtil.GlobalStartCoroutine(Cooldown(m_overheatDelay, m_overheatRate, () => { isOverheated = false; }));
					m_onOverheated.Invoke();
				}
				else
				{
					m_onCooled.Invoke();
				}
			}
		}
		public bool paused { get; set; }

		public UnityEvent<float> onValueChanged => m_onValueChanged;
		public UnityEvent onOverheated => m_onOverheated;
		public UnityEvent onCooled => m_onCooled;

		#endregion

		#region Methods

		public void Overheat()
		{
			value = maximum;
		}

		public void Vent()
		{
			CancelCooldown();
			value = 0f;
			isOverheated = false;
		}

		private IEnumerator Cooldown(float delay, float rate, Action action = null)
		{
			CancelCooldown();
			if (delay > 0f)
			{
				yield return new WaitForSeconds(delay);
			}

			while (value > 0f)
			{
				if (!paused)
				{
					value -= rate * Time.deltaTime;
				}
				yield return null;
			}

			action?.Invoke();
		}

		private void CancelCooldown()
		{
			CoroutineUtil.GlobalStopCoroutine(m_cooldownThread);
		}

		#endregion
	}
}