using ToolkitEngine.Health;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

namespace ToolkitEngine.Shooter
{
	public class ShooterCritical : MonoBehaviour
    {
		#region Fields

		[SerializeField]
		private ShooterControl[] m_shooterControls;

		[SerializeField, Range(0f, 1f), Tooltip("Percentage damage will invoke critical hit.")]
		private float m_chance = 0f;

		[SerializeField, Tooltip("Multiplier applied when critical hit occurs.")]
		private float m_multiplier = 1f;

		#endregion

		#region Events

		[SerializeField, Foldout("Events")]
		private UnityEvent<HealthEventArgs> m_onCriticalHit;

		#endregion

		#region Properties

		public float chance { get => m_chance; set => m_chance = Mathf.Clamp01(value); }
		public float multiplier { get => m_multiplier; set => m_multiplier = value; }

		public UnityEvent<HealthEventArgs> onCriticalHit => m_onCriticalHit;

		#endregion

		#region Methods

		private void Awake()
		{
			m_shooterControls = ShooterControl.GetShooterControls(gameObject, m_shooterControls);
		}

		private void OnEnable()
		{
			foreach (var control in m_shooterControls)
			{
				foreach (var shooter in control.shooters)
				{
					shooter.onDamageDealing.AddListener(Shooter_DamageDealing);
				}
			}
		}

		private void OnDisable()
		{
			foreach (var control in m_shooterControls)
			{
				foreach (var shooter in control.shooters)
				{
					shooter.onDamageDealing.RemoveListener(Shooter_DamageDealing);
				}
			}
		}

		private void Shooter_DamageDealing(HealthEventArgs e)
		{
			if (Random.Range(0f, 1f) > m_chance)
				return;

			e.postDamageFactor += m_multiplier;
			m_onCriticalHit?.Invoke(e);
		}

		#endregion
	}
}