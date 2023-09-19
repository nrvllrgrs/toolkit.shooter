using UnityEngine;

namespace ToolkitEngine.Weapons
{
	public class SplashFXControl : MonoBehaviour
	{
		#region Fields

		[SerializeField]
		private BaseShooter[] m_shooters;

		[SerializeField]
		private Spawner m_spawner;

		#endregion

		#region Methods

		private void Awake()
		{
			if (m_shooters.Length == 0)
			{
				m_shooters = GetComponents<BaseShooter>();
			}
		}

		private void OnEnable()
		{
			foreach (var shooter in m_shooters)
			{
				shooter.onFired.AddListener(Shooter_Fired);
			}
		}

		private void OnDisable()
		{
			foreach (var shooter in m_shooters)
			{
				shooter.onFired.RemoveListener(Shooter_Fired);
			}
		}

		private void Shooter_Fired(ShooterEventArgs e)
		{
			foreach (var hit in e.hits)
			{
				m_spawner.Instantiate(hit.contact, Quaternion.identity);
			}
		}

		#endregion
	}
}