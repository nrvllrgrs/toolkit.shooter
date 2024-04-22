namespace ToolkitEngine.Shooter
{
	public class ShooterChargeModifier : BaseShooterModifier<ShooterCharge>
	{
		protected override string[] propertyNames => new[]
		{
			"mxCharge",
		};
	}
}