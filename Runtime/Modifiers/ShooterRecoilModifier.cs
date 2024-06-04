namespace ToolkitEngine.Shooter
{
	public class ShooterRecoilModifier : BaseShooterModifier<ShooterRecoil>
	{
		public const string RECOIL_PER_SHOT_KEY = "recoilPerShot";
		public const string RECOVERY_DELAY_KEY = "recoveryDelay";
		public const string RECOVERY_RATE_KEY = "recoveryRate";

		internal override string[] propertyNames => new[]
		{
			RECOIL_PER_SHOT_KEY,
			RECOVERY_DELAY_KEY,
			RECOVERY_RATE_KEY,
		};
	}
}