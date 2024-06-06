namespace ToolkitEngine.Shooter
{
    public class ShooterCriticalModifier : BaseShooterModifier<ShooterCritical>
    {
        public const string CHANCE_KEY = "chance";
        public const string MULTIPLIER_KEY = "multiplier";

        protected override string[] propertyNames => new[]
        {
            CHANCE_KEY,
            MULTIPLIER_KEY
        };
    }
}