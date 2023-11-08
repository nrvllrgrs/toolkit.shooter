using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ToolkitEngine.Shooter
{
	[AddComponentMenu("Weapon/Shooter Spread")]
	public class ShooterSpread : MonoBehaviour
	{
		#region Fields

		[SerializeField]
		private ShooterControl[] m_shooterControls;

		[SerializeField]
		private BaseMuzzleShooter[] m_shooters;

		[SerializeField, Range(0f, 90f), Tooltip("Degrees (pitch-yaw) changed per shot.")]
		private float m_spreadPerShot = 5f;

		[SerializeField, Tooltip("Spread range (in degrees) shooter can spread.")]
		private Vector2 m_spreadLimits = new Vector2(0f, 90f);

		[SerializeField, Min(0f), Tooltip("Seconds to wait before recovering.")]
		private float m_recoveryDelay;

		[SerializeField, Min(0f), Tooltip("Degrees changed per second.")]
		private float m_recoveryRate = 5f;

		private float m_value = 0f;
		private Coroutine m_recoveryThread = null;

		#endregion

		#region Properties

		public float minSpread { get => m_spreadLimits.x; set => m_spreadLimits.x = value; }
		public float maxSpread { get => m_spreadLimits.y; set => m_spreadLimits.y = value; }
		public bool anyExceedsMax => m_shooters.Any(x => x.spread >= maxSpread);

		#endregion

		#region Methods

		private void Awake()
		{
			m_shooterControls = ShooterControl.GetShooterControls(gameObject, m_shooterControls);

			if ((m_shooters?.Length ?? 0) == 0)
			{
				m_shooters = m_shooterControls.SelectMany(x => x.shooters)
					.Where(x => x is BaseMuzzleShooter)
					.Cast<BaseMuzzleShooter>().ToArray();
			}
		}

		private void OnEnable()
		{
			foreach (var shooterControl in m_shooterControls)
			{
				shooterControl.onShotFired.AddListener(ShooterControl_ShotFired);
			}
		}

		private void OnDisable()
		{
			foreach (var shooterControl in m_shooterControls)
			{
				shooterControl.onShotFired.RemoveListener(ShooterControl_ShotFired);
			}
		}

		private void ShooterControl_ShotFired(ShooterControl shooterControl)
		{
			if (!enabled)
				return;

			if (m_spreadPerShot == 0f)
				return;

			var delta = shooterControl.fireType != ShooterControl.FireType.Continuous
				? m_spreadPerShot
				: m_spreadPerShot * Time.deltaTime;

			UpdateSpread(delta);
			this.RestartCoroutine(AsyncRecovery(), ref m_recoveryThread);
		}

		private IEnumerator AsyncRecovery()
		{
			if (m_recoveryDelay > 0f)
			{
				yield return new WaitForSeconds(m_recoveryDelay);
			}

			while (true)
			{
				if (!UpdateSpread(-m_recoveryRate * Time.deltaTime))
					yield break;

				yield return null;
			}
		}

		private bool UpdateSpread(float delta)
		{
			bool loop = false;
			foreach (var shooter in m_shooters)
			{
				shooter.spread = Mathf.Clamp(shooter.spread + delta, minSpread, maxSpread);
				loop |= shooter.spread > minSpread;
			}

			return loop;
		}

		#endregion

		#region Editor-Only
#if UNITY_EDITOR

		private void OnDrawGizmosSelected()
		{
			IEnumerable<BaseMuzzleShooter> shooters = null;
			if (Application.isPlaying)
			{
				shooters = m_shooters?.Where(x => x is BaseMuzzleShooter)
					.Cast<BaseMuzzleShooter>();
			}
			else
			{
				shooters = m_shooters?.Where(x => x is BaseMuzzleShooter)
					.Cast<BaseMuzzleShooter>();

				if ((m_shooters?.Length ?? 0) == 0)
				{
					shooters = ShooterControl.GetShooterControls(gameObject, m_shooterControls)
						.SelectMany(x => x.shooters)
						.Where(x => x is BaseMuzzleShooter)
						.Cast<BaseMuzzleShooter>();
				}
			}

			if (shooters == null)
				return;

			foreach (var shooter in shooters)
			{
				if (minSpread == maxSpread)
				{
					GizmosUtil.DrawCone(shooter.muzzle.position, minSpread, 1f, shooter.muzzle.forward, Color.yellow);
				}
				else
				{
					GizmosUtil.DrawCone(shooter.muzzle.position, minSpread, 1f, shooter.muzzle.forward, Color.green);
					GizmosUtil.DrawCone(shooter.muzzle.position, maxSpread, 1f, shooter.muzzle.forward, Color.red);
				}
			}
		}

#endif
		#endregion
	}
}