namespace ToolkitEngine.Shooter
{
	public class ShooterReloadModifier : BaseShooterModifier<ShooterReload>
	{
		protected override string[] propertyNames => new[]
		{
			"delay",
			"subsequentDelay"
		};
	}
}