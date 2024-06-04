namespace ToolkitEngine.Shooter
{
	public class ShooterControlModifier : BaseShooterModifier<ShooterControl>
	{
		public const string TIME_BETWEEN_SHOTS_KEY = "timeBetweenShots";
		public const string TIME_BETWEEN_BURSTS_KEY = "timeBetweenBursts";
		public const string SHOTS_PER_BURST_KEY = "shotsPerBurst";

		internal override string[] propertyNames => new[]
		{
			TIME_BETWEEN_SHOTS_KEY,
			TIME_BETWEEN_BURSTS_KEY,
			SHOTS_PER_BURST_KEY,
		};
	}
}