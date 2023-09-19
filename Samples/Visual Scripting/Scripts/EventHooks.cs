namespace ToolkitEngine.Weapons
{
    public static class EventHooks
    {
        // Projectile
        public const string OnProjectileDetonated = nameof(OnProjectileDetonated);

        // Ammo Cache
        public const string OnAmmoCacheCountChanged = nameof(OnAmmoCacheCountChanged);
		
		// IHeat (ShooterHeat, HeatCache)
		public const string OnHeatValueChanged = nameof(OnHeatValueChanged);
		public const string OnHeatOverheated = nameof(OnHeatOverheated);
		public const string OnHeatCooled = nameof(OnHeatCooled);
    }
}