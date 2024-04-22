using System.Collections.Generic;
using UnityEngine;
using ToolkitEngine.Health;

namespace ToolkitEngine.Shooter
{
	public class ShooterDamageModifier : MonoBehaviour
    {
		#region Fields

		private ShooterControl m_target;
		private Dictionary<string, HashSet<Damage>> m_bonusMap = new();
		private Dictionary<string, HashSet<IBonusDamageContainer>> m_shooterMap = new();

		public const string IMPACT_DAMAGE_KEY = "impactDamage";
		public const string SPLASH_DAMAGE_KEY = "splashDamage";

		#endregion

		#region Properties

		public ShooterControl target
		{
			get => m_target;
			set
			{
				if (m_target == value)
					return;

				Cleanup();
				m_target = value;
				Setup();
			}
		}

		#endregion

		#region Methods

		public void SetTarget(GameObject obj)
		{
			target = obj?.GetComponent<ShooterControl>();
		}

		private void Awake()
		{
			// Keep track of cumulative bonuses
			m_bonusMap.Add(IMPACT_DAMAGE_KEY, new());
			m_bonusMap.Add(SPLASH_DAMAGE_KEY, new());

			m_target = m_target ?? GetComponent<ShooterControl>();
			Setup();
		}

		private void Setup()
		{
			if (m_target == null)
				return;

			m_shooterMap.Add(IMPACT_DAMAGE_KEY, new());
			m_shooterMap.Add(SPLASH_DAMAGE_KEY, new());

			foreach (var shooter in m_target.shooters)
			{
				if (shooter is IDamageShooter damageShooter)
				{
					m_shooterMap[IMPACT_DAMAGE_KEY].Add(damageShooter.impactDamage);
					AddAllBonuses(IMPACT_DAMAGE_KEY, damageShooter.impactDamage);

					m_shooterMap[SPLASH_DAMAGE_KEY].Add(damageShooter.splashDamage);
					AddAllBonuses(SPLASH_DAMAGE_KEY, damageShooter.splashDamage);
				}
			}
		}

		private void Cleanup()
		{
			if (m_target == null)
				return;

			foreach (var shooter in m_target.shooters)
			{
				if (shooter is IDamageShooter damageShooter)
				{
					RemoveAllBonuses(IMPACT_DAMAGE_KEY, damageShooter.impactDamage);
					RemoveAllBonuses(SPLASH_DAMAGE_KEY, damageShooter.splashDamage);
				}
			}

			m_shooterMap.Clear();
		}

		#endregion

		#region Modifier Methods

		public void AddBonus(string key, Damage damage)
		{
			if (damage == null)
				return;

			if (!m_bonusMap.TryGetValue(key, out var damages))
				return;

			damages.Add(damage);

			if (m_shooterMap.TryGetValue(key, out var bonuses))
			{
				foreach (var shooter in bonuses)
				{
					shooter.bonuses.Add(damage);
				}
			}
		}

		private void AddAllBonuses(string key, IBonusDamageContainer container)
		{
			if (!m_bonusMap.TryGetValue(key, out var bonuses))
				return;

			foreach (var bonus in bonuses)
			{
				container.bonuses.Add(bonus);
			}
		}

		public void RemoveBonus(string key, Damage damage)
		{
			if (damage == null)
				return;

			if (!m_bonusMap.TryGetValue(key, out var damages))
				return;

			damages.Remove(damage);

			if (m_shooterMap.TryGetValue(key, out var bonuses))
			{
				foreach (var shooter in bonuses)
				{
					shooter.bonuses.Remove(damage);
				}
			}
		}

		private void RemoveAllBonuses(string key, IBonusDamageContainer container)
		{
			if (!m_bonusMap.TryGetValue(key, out var bonuses))
				return;

			foreach (var bonus in bonuses)
			{
				container.bonuses.Remove(bonus);
			}
		}

		#endregion
	}
}