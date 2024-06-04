namespace ToolkitEngine.Shooter
{
    public class ShooterAmmoModifier : BaseShooterModifier<ShooterAmmo>
    {
        public const string CAPACITY_KEY = "capacity";

		internal override string[] propertyNames => new[]
        {
			CAPACITY_KEY,
        };
	}
}