using ToolkitEngine.Health;
using UnityEngine;
using UnityEngine.Events;

namespace ToolkitEngine.Shooter
{
	[System.Serializable]
	public class ShooterEventArgs : System.EventArgs
	{
		#region Properties

		public BaseShooter shooter;
		public Vector3 origin;
		public Vector3 terminal;
		public Vector3 direction;
		public DamageHit[] hits;

		#endregion

		#region Constructors

		public ShooterEventArgs()
		{ }

		public ShooterEventArgs(ShooterEventArgs other)
		{
			shooter = other.shooter;
			origin = other.origin;
			terminal = other.terminal;
			hits = other.hits;
		}

		#endregion
	}

	public abstract class BaseShooter : MonoBehaviour, IDamageDealer
    {
		#region Fields

		[SerializeField]
		protected UnityEvent<ShooterEventArgs> m_onFiring;

		[SerializeField]
		protected UnityEvent<ShooterEventArgs> m_onFired;

		[SerializeField]
		protected UnityEvent<HealthEventArgs> m_onDamageDealt;

		#endregion

		#region Properties

		public UnityEvent<ShooterEventArgs> onFiring => m_onFiring;
		public UnityEvent<ShooterEventArgs> onFired => m_onFired;
		public UnityEvent<HealthEventArgs> onDamageDealt => m_onDamageDealt;

		#endregion

		#region Methods

		public void Fire()
		{
			Fire(null);
		}

		public abstract void Fire(ShooterControl shooterControl);

		#endregion
	}
}