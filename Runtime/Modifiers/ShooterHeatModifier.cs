namespace ToolkitEngine.Shooter
{
	public class ShooterHeatModifier : BaseShooterModifier<ShooterHeat>
	{
		protected override string[] propertyNames => new[]
		{
			"maximum",
		};
	}
}