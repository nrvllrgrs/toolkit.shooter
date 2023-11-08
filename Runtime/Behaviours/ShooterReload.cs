using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace ToolkitEngine.Shooter
{
	public class ShooterReload : MonoBehaviour
    {
		#region Fields

		[SerializeField, Required]
		private ShooterAmmo m_shooterAmmo;

		[SerializeField]
		private BaseAmmoCache m_ammoCache;

		[SerializeField]
		private bool m_autoReload;

		[SerializeField, Min(0f)]
		private float m_delay;

		[SerializeField]
		private int m_count;

		[SerializeField]
		private bool m_subsequentReload;

		[SerializeField, Min(0f)]
		private float m_subsequentDelay;

		[SerializeField]
		private int m_subsequentCount;

		[SerializeField]
		private UnityEvent m_onReloading;

		[SerializeField]
		private UnityEvent m_onShotReloaded;

		[SerializeField]
		private UnityEvent m_onReloaded;

		private int m_lastCount;
		private bool m_reloading;
		private CancellationTokenSource m_cancelReloadTokenSource = null;

		#endregion

		#region Properties

		public bool reloading
		{
			get => m_reloading;
			private set
			{
				// No change, skip
				if (m_reloading == value)
					return;

				m_reloading = value;
				if (value)
				{
					m_onReloading?.Invoke();
				}
				else
				{
					m_onReloaded?.Invoke();
				}
			}
		}

		public UnityEvent onReloading => m_onReloading;
		public UnityEvent onShotReloaded => m_onShotReloaded;
		public UnityEvent onReloaded => m_onReloaded;

		#endregion

		#region Methods

		private void OnEnable()
		{
			m_shooterAmmo.onCountChanged.AddListener(ShooterAmmo_CountChanged);
			m_lastCount = m_shooterAmmo.count;
		}

		private void OnDisable()
		{
			m_shooterAmmo.onCountChanged.RemoveListener(ShooterAmmo_CountChanged);
		}

		private void ShooterAmmo_CountChanged(int count)
		{
			// Ammo decremented, stop reload
			if (count < m_lastCount)
			{
				m_cancelReloadTokenSource?.Cancel();
			}

			m_lastCount = count;

			if (m_autoReload && count == 0)
			{
				Reload();
			}
		}

		[ContextMenu("Reload")]
		public async void Reload()
		{
			// Already reloading, skip
			if (reloading)
				return;

			m_cancelReloadTokenSource = new CancellationTokenSource();
			reloading = true;

			try
			{
				if (m_delay > 0f)
				{
					await Task.Delay(TimeSpan.FromSeconds(m_delay), m_cancelReloadTokenSource.Token);
				}
				Reload(m_count);

				if (m_subsequentReload)
				{
					if (m_subsequentDelay > 0f)
					{
						while (CanReload(m_subsequentCount))
						{
							await Task.Delay(TimeSpan.FromSeconds(m_delay), m_cancelReloadTokenSource.Token);
							Reload(m_subsequentCount);
						}
					}
					else
					{
						// Using subsequent reload without a delay is just reloading all
						// Be gracious and handle bad data
						Reload(m_shooterAmmo.capacity - m_count);
					}
				}
			}
			catch
			{ }

			m_reloading = false;
		}

		private void Reload(int count)
		{
			count = Mathf.Min(count, m_ammoCache?.count ?? int.MaxValue);

			m_shooterAmmo.count += count;
			if (m_ammoCache != null)
			{
				m_ammoCache.count -= count;
			}
		}

		public bool CanReload(int count)
		{
			if (m_shooterAmmo.count == m_shooterAmmo.capacity)
				return false;

			if (m_ammoCache != null && m_ammoCache.count < count)
				return false;

			return true;
		}

		#endregion
	}
}