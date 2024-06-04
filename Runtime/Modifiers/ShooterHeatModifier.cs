namespace ToolkitEngine.Shooter
{
	public class ShooterHeatModifier : BaseShooterModifier<ShooterHeat>
	{
		public const string MAXIMUM_KEY = "maximum";

		internal override string[] propertyNames => new[]
		{
			MAXIMUM_KEY,
		};
	}
}