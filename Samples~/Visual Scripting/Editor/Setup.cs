using System;
using System.Collections.Generic;
using UnityEditor;
using ToolkitEngine.Shooter;

namespace ToolkitEditor.Shooter.VisualScripting
{
	[InitializeOnLoad]
	public static class Setup
	{
		static Setup()
		{
			var types = new List<Type>()
			{
				typeof(Ammo),
				typeof(AmmoCache),
				typeof(AmmoCache),
				typeof(AmmoType),
				typeof(Heat),
				typeof(HeatCache),

				// Behaviors
				typeof(ShooterAmmo),
				typeof(ShooterCharge),
				typeof(ShooterHeat),
				typeof(ShooterRecoil),
				typeof(ShooterReload),
				typeof(ShooterSpread),

				// Modifiers
				typeof(ShooterAmmoModifier),
				typeof(ShooterChargeModifier),
				typeof(ShooterHeatModifier),
				typeof(ShooterRecoilModifier),
				typeof(ShooterReloadModifier),
				typeof(ShooterSpreadModifier),
				typeof(ShooterDamageModifier),

				// Shooters
				typeof(BaseShooter),
				typeof(ProjectileShooter),
				typeof(RayShooter),
				typeof(SequenceShooter),
				typeof(TriggerShooter),
				typeof(ProxyShooter),

				typeof(ShooterControl),
				typeof(ShooterMode),
				typeof(Projectile),
				typeof(ProjectileEventArgs),
				typeof(ShooterEventArgs),
			};

			ToolkitEditor.VisualScripting.Setup.Initialize("ToolkitEngine.Shooter", types);
		}
	}
}