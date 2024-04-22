using System.Collections;
using System.Collections.Generic;
using ToolkitEngine.Health;
using UnityEngine;

namespace ToolkitEngine.Shooter
{
    public class ProxyShooter : BaseShooter
    {
		#region Fields

		private ShooterControl m_target;

		#endregion

		#region Properties

		public ShooterControl target
		{
			get => m_target;
			set
			{
				// No change, skip
				if (m_target == value)
					return;

				Unregister();
				m_target = value;
				Register();
			}
		}

		#endregion

		#region Methods

		public void SetTarget(GameObject obj)
		{
			target = obj?.GetComponent<ShooterControl>();
		}

		private void Register()
		{
			if (m_target == null)
				return;

			foreach (var shooter in m_target.shooters)
			{
				shooter.onFiring.AddListener(OnFiring);
				shooter.onFired.AddListener(OnFired);
				shooter.onDamageDealt.AddListener(OnDamageDealt);
			}
		}

		private void Unregister()
		{
			if (m_target == null)
				return;

			foreach (var shooter in m_target.shooters)
			{
				shooter.onFiring.RemoveListener(OnFiring);
				shooter.onFired.RemoveListener(OnFired);
				shooter.onDamageDealt.RemoveListener(OnDamageDealt);
			}
		}

		public override void Fire(ShooterControl shooterControl)
		{
			if (m_target == null)
				return;

			m_target.Fire();
		}

		#endregion

		#region Shooter Callbacks

		private void OnFiring(ShooterEventArgs e)
		{
			m_onFiring?.Invoke(e);
		}

		private void OnFired(ShooterEventArgs e)
		{
			m_onFired?.Invoke(e);
		}

		private void OnDamageDealt(HealthEventArgs e)
		{
			m_onDamageDealt?.Invoke(e);
		}

		#endregion
	}
}