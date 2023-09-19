using UnityEngine;
using UnityEngine.Events;

namespace ToolkitEngine.Weapons
{
	public class SequenceShooter : BaseShooter, IPoolItemRecyclable
    {
		#region Fields

		[SerializeField]
		private StepShooter[] m_steps;

		[SerializeField, Tooltip("Indicates whether step is skipped if invalid (advancing to next valid step).")]
		private bool m_skipInvalid;

		private int m_index = -1;

		#endregion

		#region Events

		[SerializeField]
		private UnityEvent<int> m_onIndexChanged;

		#endregion

		#region Properties

		public int index
		{
			get => m_index;
			private set
			{
				// No change, skip
				if (m_index == value)
					return;

				m_index = value;
				m_onIndexChanged?.Invoke(value);
			}
		}

		public UnityEvent<int> onIndexChanged => m_onIndexChanged;

		#endregion

		#region Methods

		public void Recycle()
		{
			m_index = -1;
			UpdateIndex();
		}

		private void Awake()
		{
			UpdateIndex();
		}

		private void UpdateIndex()
		{
			int startingIndex = (m_index + 1).Mod(m_steps.Length);
			int index = startingIndex;

			// Loop through until find valid step
			while (m_steps[index].blockers.isTrueAndEnabled)
			{
				// Step is invalid, not skipping 
				if (!m_skipInvalid)
					return;

				// Advance to next step
				index = (index + 1).Mod(m_steps.Length);

				// Looped through all indicies and nothing is valid, skip
				if (startingIndex == index)
					return;
			}

			// Update selected index
			this.index = index;
		}

		public override void Fire(ShooterControl shooterControl)
		{
			var shooter = m_steps[m_index].shooter;
			shooter.onFiring.AddListener(Shooter_OnFiring);
			shooter.onFired.AddListener(Shooter_OnFired);

			shooter.Fire(shooterControl);

			shooter.onFiring.RemoveListener(Shooter_OnFiring);
			shooter.onFired.RemoveListener(Shooter_OnFired);

			UpdateIndex();
		}

		private void Shooter_OnFiring(ShooterEventArgs e)
		{
			var copy = new ShooterEventArgs(e);
			copy.shooter = this;

			m_onFiring?.Invoke(copy);
		}

		private void Shooter_OnFired(ShooterEventArgs e)
		{
			var copy = new ShooterEventArgs(e);
			copy.shooter = this;

			m_onFired?.Invoke(copy);
		}

		public void ResetIndex() => m_index = -1;

		#endregion

		#region Structures

		[System.Serializable]
		public class StepShooter
		{
			public BaseShooter shooter;
			public UnityCondition blockers = new UnityCondition(UnityCondition.ConditionType.Any);
		}

		#endregion
	}
}