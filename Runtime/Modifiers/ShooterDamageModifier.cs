using System.Collections.Generic;
using UnityEngine;
using ToolkitEngine.Health;

namespace ToolkitEngine.Shooter
{
	public class ShooterDamageModifier : MonoBehaviour
    {
		#region Fields

		private ShooterControl m_target;

		private ImpactDamage m_overrideImpactDamage = null;
		private Dictionary<IDamageShooter, ImpactDamage> m_impactDamageMap = new();

		private SplashDamage m_overrideSplashDamage = null;
		private Dictionary<IDamageShooter, SplashDamage> m_splashDamageMap = new();

		private Dictionary<string, float> m_bonusFactors = new();
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
			m_bonusFactors.Add(IMPACT_DAMAGE_KEY, 0f);
			m_bonusFactors.Add(SPLASH_DAMAGE_KEY, 0f);

			// Keep track of cumulative bonuses
			m_bonusMap.Add(IMPACT_DAMAGE_KEY, new());
			m_bonusMap.Add(SPLASH_DAMAGE_KEY, new());

			m_target ??= GetComponent<ShooterControl>();
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
				if (shooter is not IDamageShooter damageShooter)
					continue;

				m_shooterMap[IMPACT_DAMAGE_KEY].Add(damageShooter.impactDamage);
				AddAllBonuses(IMPACT_DAMAGE_KEY, damageShooter.impactDamage);

				if (m_overrideImpactDamage != null)
				{
					CopyImpactDamage(damageShooter.impactDamage, m_overrideImpactDamage);
				}

				m_shooterMap[SPLASH_DAMAGE_KEY].Add(damageShooter.splashDamage);
				AddAllBonuses(SPLASH_DAMAGE_KEY, damageShooter.splashDamage);

				if (m_overrideSplashDamage != null)
				{
					CopySplashDamage(damageShooter.splashDamage, m_overrideSplashDamage);
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
					if (m_overrideImpactDamage != null && m_impactDamageMap.TryGetValue(damageShooter, out var srcImpactDamage))
					{
						CopyImpactDamage(damageShooter.impactDamage, srcImpactDamage);
						m_impactDamageMap.Remove(damageShooter);
					}

					RemoveAllBonuses(SPLASH_DAMAGE_KEY, damageShooter.splashDamage);
					if (m_overrideSplashDamage != null && m_splashDamageMap.TryGetValue(damageShooter, out var srcSplashDamage))
					{
						CopySplashDamage(damageShooter.splashDamage, srcSplashDamage);
						m_splashDamageMap.Remove(damageShooter);
					}
				}
			}

			m_shooterMap.Clear();
		}

		#endregion

		#region Modifier Methods

		public void ModifyFactor(string key, float value)
		{
			if (!m_bonusFactors.ContainsKey(key))
				return;

			m_bonusFactors[key] += value;

			if (m_shooterMap.TryGetValue(key, out var container))
			{
				switch (key)
				{
					case IMPACT_DAMAGE_KEY:
						foreach (var shooter in container)
						{
							if (shooter is not IDamageShooter damageShooter)
								continue;

							damageShooter.impactDamage.factor = 1f + m_bonusFactors[key];
						}
						break;

					case SPLASH_DAMAGE_KEY:
						foreach (var shooter in container)
						{
							if (shooter is not IDamageShooter damageShooter)
								continue;

							damageShooter.splashDamage.factor = 1f + m_bonusFactors[key];
						}
						break;
				}
			}
		}

		public void AddBonus(string key, Damage damage)
		{
			if (damage == null)
				return;

			if (!m_bonusMap.TryGetValue(key, out var damages))
				return;

			damages.Add(damage);

			if (m_shooterMap.TryGetValue(key, out var container))
			{
				foreach (var shooter in container)
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

			if (m_shooterMap.TryGetValue(key, out var container))
			{
				foreach (var shooter in container)
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

		#region Override Methods

		public void OverrideImpactDamage(ImpactDamage impactDamage)
		{
			if (impactDamage == null)
			{
				RestoreImpactDamage();
				return;
			}

			m_overrideImpactDamage = impactDamage;

			SetDamage(IMPACT_DAMAGE_KEY, (damageShooter) =>
			{
				var srcDamage = new ImpactDamage(damageShooter.impactDamage);
				m_impactDamageMap.Add(damageShooter, srcDamage);

				CopyImpactDamage(damageShooter.impactDamage, m_overrideImpactDamage);
			});
		}

		public void RestoreImpactDamage()
		{
			if (m_overrideImpactDamage == null)
				return;

			m_overrideImpactDamage = null;

			SetDamage(IMPACT_DAMAGE_KEY, (damageShooter) =>
			{
				if (m_impactDamageMap.TryGetValue(damageShooter, out var srcDamage))
				{
					CopyImpactDamage(damageShooter.impactDamage, srcDamage);
					m_impactDamageMap.Remove(damageShooter);
				}
			});
		}

		private void CopyImpactDamage(ImpactDamage dst, ImpactDamage src)
		{
			dst.value = src.value;
			dst.damageType = src.damageType;
			dst.factor = src.factor;
			dst.range = src.range;
		}

		public void OverrideSplashDamage(SplashDamage splashDamage)
		{
			if (splashDamage == null)
			{
				RestoreSplashDamage();
				return;
			}

			m_overrideSplashDamage = splashDamage;

			SetDamage(SPLASH_DAMAGE_KEY, (damageShooter) =>
			{
				var srcDamage = new SplashDamage(damageShooter.splashDamage);
				m_splashDamageMap.Add(damageShooter, srcDamage);

				CopySplashDamage(damageShooter.splashDamage, m_overrideSplashDamage);
			});
		}

		public void RestoreSplashDamage()
		{
			if (m_overrideSplashDamage == null)
				return;

			m_overrideSplashDamage = null;

			SetDamage(SPLASH_DAMAGE_KEY, (damageShooter) =>
			{
				if (m_splashDamageMap.TryGetValue(damageShooter, out var srcDamage))
				{
					CopySplashDamage(damageShooter.splashDamage, srcDamage);
					m_splashDamageMap.Remove(damageShooter);
				}
			});
		}

		private void CopySplashDamage(SplashDamage dst, SplashDamage src)
		{
			dst.value = src.value;
			dst.damageType = src.damageType;
			dst.factor = src.factor;
			dst.upwardModifier = src.upwardModifier;
			dst.innerRadius = src.innerRadius;
			dst.outerRadius = src.outerRadius;
		}

		private void SetDamage(string key, System.Action<IDamageShooter> action)
		{
			if (m_shooterMap.TryGetValue(key, out var container))
			{
				foreach (var shooter in container)
				{
					if (shooter is not IDamageShooter damageShooter)
						continue;

					action(damageShooter);
				}
			}
		}

		#endregion
	}
}