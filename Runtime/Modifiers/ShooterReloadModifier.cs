namespace ToolkitEngine.Shooter
{
	public class ShooterReloadModifier : BaseShooterModifier<ShooterReload>
	{
		public const string COUNT_KEY = "count";
		public const string DELAY_KEY = "delay";
		public const string SUBSEQUENT_DELAY_KEY = "subsequentDelay";

		internal override string[] propertyNames => new[]
		{
			COUNT_KEY,
			DELAY_KEY,
			SUBSEQUENT_DELAY_KEY,
		};
	}
}