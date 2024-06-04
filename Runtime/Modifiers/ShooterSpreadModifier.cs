namespace ToolkitEngine.Shooter
{
	public class ShooterSpreadModifier : BaseShooterModifier<ShooterSpread>
	{
		public const string SPREAD_PER_SHOT_KEY = "spreadPerShot";
		public const string MIN_SPREAD_KEY = "minSpread";
		public const string MAX_SPREAD_KEY = "maxSpread";
		public const string RECOVERY_DELAY_KEY = "recoveryDelay";
		public const string RECOVERY_RATE_KEY = "recoveryRate";

		internal override string[] propertyNames => new[]
		{
			SPREAD_PER_SHOT_KEY,
			MIN_SPREAD_KEY,
			MAX_SPREAD_KEY,
			RECOVERY_DELAY_KEY,
			RECOVERY_RATE_KEY,
		};
	}
}