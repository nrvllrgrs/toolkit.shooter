using UnityEngine;

namespace ToolkitEngine.Shooter
{
	public class MuzzleFXControl : MonoBehaviour
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
			m_spawner.Instantiate(e.origin, Quaternion.LookRotation(e.direction));
		}

		#endregion
	}
}