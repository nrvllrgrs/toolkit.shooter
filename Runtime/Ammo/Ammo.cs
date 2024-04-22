using NaughtyAttributes;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace ToolkitEngine.Shooter
{
	[Serializable]
	public class Ammo
	{
		#region Fields

		[SerializeField]
		private AmmoType m_ammoType;

		/// <summary>
		/// Maximum number of shots that can be carried
		/// </summary>
		[SerializeField, Min(1), Tooltip("Maximum number of shots that can be carried")]
		private int m_capacity = 1;

		/// <summary>
		/// Number of shots currently carried
		/// </summary>
		[SerializeField, Min(0), Tooltip("Number of shots currently carried")]
		private int m_count;

		/// <summary>
		/// Indicates whether ammo can regenerate
		/// </summary>
		[SerializeField, Tooltip("Indicates whether ammo can regenerate")]
		private bool m_canRegenerate;

		/// <summary>
		/// Seconds to wait before regeneration begins
		/// </summary>
		[SerializeField, Label("Delay"), Min(0f), Tooltip("Seconds to wait before regeneration begins")]
		private float m_regenerateDelay;

		/// <summary>
		/// Number of ammo regenerated per second
		/// </summary>
		[SerializeField, Label("Rate"), Min(0f), Tooltip("Number of ammo regenerated per second")]
		private float m_regenerateRate;

		private CancellationTokenSource m_cancelRegenerationTokenSource = null;

		#endregion

		#region Events

		[SerializeField]
		private UnityEvent<int> m_onCountChanged;

		#endregion

		#region Properties

		public AmmoType ammoType => m_ammoType;

		public int capacity
		{
			get => m_capacity;
			set
			{
				// Capacity cannot be below 1
				value = Mathf.Max(value, 1);

				// No change, skip
				if (m_capacity == value)
					return;

				m_capacity = value;
				count = Mathf.Max(m_capacity, m_count);
			}
		}

		public int count
		{
			get => m_count;
			set
			{
				value = Mathf.Clamp(value, 0, m_capacity);
				if (count == value)
					return;

				m_count = value;
				m_onCountChanged?.Invoke(value);

				if (m_canRegenerate && m_regenerateRate > 0f)
				{
					Task.Run(Regenerate);
				}
			}
		}

		public float normalizedCount => (float)m_count / m_capacity;

		public UnityEvent<int> onCountChanged => m_onCountChanged;

		#endregion

		#region Methods

		public async void Regenerate()
		{
			CancelRegenerate();
			m_cancelRegenerationTokenSource = new CancellationTokenSource();

			if (m_regenerateDelay > 0f)
			{
				try
				{
					await Task.Delay(TimeSpan.FromSeconds(m_regenerateDelay), m_cancelRegenerationTokenSource.Token);
				}
				catch { return; }
			}

			while (true)
			{
				try
				{
					await Task.Delay(TimeSpan.FromSeconds(1f / m_regenerateRate), m_cancelRegenerationTokenSource.Token);
				}
				catch { return; }

				++count;

				if (count == m_capacity)
					return;
			}
		}

		public void CancelRegenerate()
		{
			// Stop regeneration when player fires
			if (m_cancelRegenerationTokenSource != null)
			{
				m_cancelRegenerationTokenSource.Cancel();
			}
		}

		#endregion
	}
}