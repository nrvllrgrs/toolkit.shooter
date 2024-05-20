namespace ToolkitEngine.Shooter
{
    public static class EventHooks
    {
        // Shooter Control
        public const string OnShooterControlBeginFire = nameof(OnShooterControlBeginFire);
        public const string OnShooterControlShotFiring = nameof(OnShooterControlShotFiring);
		public const string OnShooterControlShotFired = nameof(OnShooterControlShotFired);
		public const string OnShooterControlShotBlocked = nameof(OnShooterControlShotBlocked);
		public const string OnShooterControlEndFire = nameof(OnShooterControlEndFire);

        // Ammo Cache
        public const string OnAmmoCacheCountChanged = nameof(OnAmmoCacheCountChanged);
		
		// IHeat (ShooterHeat, HeatCache)
		public const string OnHeatValueChanged = nameof(OnHeatValueChanged);
		public const string OnHeatOverheated = nameof(OnHeatOverheated);
		public const string OnHeatCooled = nameof(OnHeatCooled);

        // ShooterCharge
        public const string OnChargeCharging = nameof(OnChargeCharging);
        public const string OnChargeCharged = nameof(OnChargeCharged);
        public const string OnChargeDecharging = nameof(OnChargeDecharging);
        public const string OnChargeDecharged = nameof(OnChargeDecharged);
        public const string OnChargeValueChanged = nameof(OnChargeValueChanged);
    }
}