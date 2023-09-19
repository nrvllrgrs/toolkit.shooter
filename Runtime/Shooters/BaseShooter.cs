using ToolkitEngine.Health;
using UnityEngine;
using UnityEngine.Events;

namespace ToolkitEngine.Weapons
{
	[System.Serializable]
	public class ShooterEventArgs : System.EventArgs
	{
		#region Properties

		public BaseShooter shooter;
		public Vector3 origin;
		public Vector3 terminal;
		public DamageHit[] hits;

		public Vector3 direction => terminal - origin;

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

	public abstract class BaseShooter : MonoBehaviour
    {
		#region Fields

		[SerializeField]
		protected UnityEvent<ShooterEventArgs> m_onFiring;

		[SerializeField]
		protected UnityEvent<ShooterEventArgs> m_onFired;

		#endregion

		#region Properties

		public UnityEvent<ShooterEventArgs> onFiring => m_onFiring;
		public UnityEvent<ShooterEventArgs> onFired => m_onFired;

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