namespace ToolkitEngine.Shooter
{
	public class ShooterChargeModifier : BaseShooterModifier<ShooterCharge>
	{
		public const string MAX_CHARGE_KEY = "maxCharge";

		protected override string[] propertyNames => new[]
		{
			MAX_CHARGE_KEY,
		};
	}
}