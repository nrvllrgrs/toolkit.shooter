using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace ToolkitEngine.Shooter
{
	[AddComponentMenu("Weapon/Shooter Control")]
	public class ShooterControl : MonoBehaviour
    {
		#region Enumerators

		public enum FireType
		{
			Continuous,
			FullAuto,
			SemiAuto,
		}

		#endregion

		#region Fields

		[SerializeField, ReadOnly, Tooltip("Collection of shooters")]
		private BaseShooter[] m_shooters;

		[SerializeField, Tooltip("Prevents (or cancels) control from firing if ANY condition is true.")]
		private UnityCondition m_firingBlockers = new UnityCondition(UnityCondition.ConditionType.Any);

		[SerializeField, Tooltip("Prevents shot from being fired if ANY condition is true.")]
		private UnityCondition m_shotFiredBlockers = new UnityCondition(UnityCondition.ConditionType.Any);

		[SerializeField]
		private FireType m_fireType = FireType.FullAuto;

		/// <summary>
		/// Seconds between shots
		/// </summary>
		[SerializeField, Min(0f), Tooltip("Seconds between shots")]
		private float m_timeBetweenShots;

		/// <summary>
		/// Indicates whether "Fire" action occurs on button down
		/// </summary>
		[SerializeField, Tooltip("Indicates whether \"Fire\" action occurs on cancel")]
		private bool m_fireOnCancel = false;

		/// <summary>
		/// Indicates whether "Fire" action triggers multiple shots
		/// </summary>
		[SerializeField, Tooltip("Indicates whether \"Fire\" action triggers multiple shots")]
		private bool m_isBurstFire = false;

		/// <summary>
		/// Seconds between bursts
		/// </summary>
		[SerializeField, Min(0f), Tooltip("Seconds between bursts")]
		private float m_timeBetweenBursts;

		/// <summary>
		/// Number of shots per burst
		/// </summary>
		[SerializeField, Min(2), Tooltip("Number of shots per burst")]
		private int m_shotsPerBurst = 2;

		private bool m_firing = false;

		private float m_lastShotTime;
		private int m_burstShotCount;
		private Coroutine m_fireThread, m_burstThread;

		#endregion

		#region Events

		[SerializeField, Tooltip("Invoked when player depresses button")]
		private UnityEvent<ShooterControl> m_onBeginFire;

		[SerializeField]
		private UnityEvent<ShooterControl> m_onShotFiring;

		[SerializeField]
		private UnityEvent<ShooterControl> m_onShotFired;

		[SerializeField]
		private UnityEvent<ShooterControl> m_onShotBlocked;

		[SerializeField, Tooltip("Invoked when player releases button")]
		private UnityEvent<ShooterControl> m_onEndFire;

		[SerializeField, Tooltip("Invoked when ShooterMode selects control.")]
		private UnityEvent<ShooterControl> m_onSelected;

		[SerializeField, Tooltip("Invoked when ShooterMode unselects control.")]
		private UnityEvent<ShooterControl> m_onUnselected;

		#endregion

		#region Properties

		public BaseShooter[] shooters { get => m_shooters; internal set => m_shooters = value; }
		public FireType fireType => m_fireType;
		public float timeBetweenShots => m_timeBetweenShots;
		public bool fireOnCancel => m_fireOnCancel;
		public bool isBurstFire => m_isBurstFire;
		public float timeBetweenBursts => m_timeBetweenBursts;
		public float burstShotCount => m_burstShotCount;

		/// <summary>
		/// Indicates whether weapon is currently in firing state.
		/// Note: "Firing" does not necessarily mean shooting shots.
		/// </summary>
		public bool firing
		{
			get => m_firing;
			protected set
			{
				// No change, skip
				if (m_firing == value)
					return;

				m_firing = value;
				if (value)
				{
					m_onBeginFire?.Invoke(this);
				}
				else
				{
					m_onEndFire?.Invoke(this);
				}
			}
		}

		/// <summary>
		/// Indicates whether weapon is currently in burst sequence.
		/// </summary>
		public bool bursting => m_burstThread != null;

		/// <summary>
		/// Indicates whether weapon can by fired
		/// </summary>
		public bool canFire => !firing && !bursting && canFireByTime && !m_firingBlockers.isTrueAndEnabled;

		public bool canFireByTime
		{
			get
			{
				// Weapon has not been fired OR continous (i.e. every frame)
				if (m_lastShotTime <= 0f || fireType == FireType.Continuous)
					return true;

				// Wait for time between shots if NOT burst fire or between shots during burst
				if (!m_isBurstFire || m_burstShotCount > 0)
				{
					return Time.time >= m_lastShotTime + m_timeBetweenShots;
				}

				// Wait for time between bursts (because using burst fire)
				return Time.time >= m_lastShotTime + m_timeBetweenBursts;
			}
		}

		public UnityEvent<ShooterControl> onBeginFire => m_onBeginFire;
		public UnityEvent<ShooterControl> onShotFiring => m_onShotFiring;
		public UnityEvent<ShooterControl> onShotFired => m_onShotFired;
		public UnityEvent<ShooterControl> onShotBlocked => m_onShotBlocked;
		public UnityEvent<ShooterControl> onEndFire => m_onEndFire;
		public UnityEvent<ShooterControl> onSelected => m_onSelected;
		public UnityEvent<ShooterControl> onUnselected => m_onUnselected;

		#endregion

		#region Methods

		private void OnDisable()
		{
			m_firing = false;
			this.CancelCoroutine(ref m_fireThread);
			this.CancelCoroutine(ref m_burstThread);
		}

		public void Fire()
		{
			// Already firing, skip
  			if (firing || bursting || m_firingBlockers.isTrueAndEnabled)
				return;

			firing = true;

			switch (m_fireType)
			{
				case FireType.SemiAuto:
					if (!m_fireOnCancel)
					{
						AttemptFire();

						if (!m_isBurstFire)
						{
							CancelFire();
						}
					}
					break;

				default:
					m_fireThread = StartCoroutine(AsyncFire());
					break;
			}
		}

		public void CancelFire(bool ignoreFireOnCancel = false)
		{
			CancelFire(false, ignoreFireOnCancel);
		}

		private void CancelFire(bool stopBursting, bool ignoreFireOnCancel)
		{
			// Already not firing, skip
			if (!firing)
				return;

			// Exit async loop, if exists
			this.CancelCoroutine(ref m_fireThread);

			if (stopBursting && bursting)
			{
				this.CancelCoroutine(ref m_burstThread);
				ResetBurst();
			}

			if (!ignoreFireOnCancel && m_fireOnCancel && fireType == FireType.SemiAuto)
			{
				AttemptFire();
			}

			firing = false;
		}

		private void AttemptFire()
		{
			if (!canFireByTime)
				return;

			// Firing conditions not met BEFORE SHOT, exit firing state
			if (m_firingBlockers.isTrueAndEnabled)
			{
				CancelFire(true, false);
				return;
			}

			if (!m_shotFiredBlockers.isTrueAndEnabled)
			{
				m_onShotFiring?.Invoke(this);

				foreach (var shooter in m_shooters)
				{
					if (shooter == null)
						continue;

					shooter.Fire(this);
				}

				m_onShotFired?.Invoke(this);
				UpdateFireTime();
			}

			// Firing conditions not met AFTER SHOT, exit firing state
 			if (m_firingBlockers.isTrueAndEnabled)
			{
				CancelFire(true, false);
			}
		}

		private void UpdateFireTime()
		{
			m_lastShotTime = Time.time;

			if (fireType != FireType.Continuous && m_isBurstFire)
			{
				++m_burstShotCount;

				// First shot of burst-fire needs to start coroutine to fire remaining shots
				if (!bursting)
				{
					m_burstThread = StartCoroutine(AsyncBurstFire());
				}
			}
		}

		private void ResetBurst()
		{
			m_burstShotCount = 0;
			m_burstThread = null;
		}

		private IEnumerator AsyncFire()
		{
			while (firing)
			{
				if (!bursting)
				{
					AttemptFire();
				}
				yield return null;
			}
		}

		private IEnumerator AsyncBurstFire()
		{
			while (m_burstShotCount < m_shotsPerBurst)
			{
				yield return new WaitForSeconds(m_timeBetweenShots);
				AttemptFire();
			}

			if (fireType == FireType.SemiAuto || !m_firing)
			{
				ResetBurst();
				CancelFire();
				yield break;
			}

			// Wait for time between bursts
			yield return new WaitForSeconds(m_timeBetweenBursts);
			ResetBurst();
		}

		public void Select()
		{
			m_onSelected?.Invoke(this);
		}

		public void Unselect()
		{
			m_onUnselected?.Invoke(this);
		}

		#endregion

		#region Static Methods

		public static ShooterControl[] GetShooterControls(GameObject obj, ShooterControl[] shooterControls)
		{
			if ((shooterControls?.Length ?? 0) == 0)
			{
				return obj.GetComponentsInChildren<ShooterControl>();
			}
			return shooterControls;
		}

		#endregion
	}
}