namespace ToolkitEngine.Shooter
{
	public class ShooterSpreadModifier : BaseShooterModifier<ShooterSpread>
	{
		protected override string[] propertyNames => new[]
		{
			"spreadPerShot",
			"minSpread",
			"maxSpread",
			"recoveryDelay",
			"recoveryRate",
		};
	}
}