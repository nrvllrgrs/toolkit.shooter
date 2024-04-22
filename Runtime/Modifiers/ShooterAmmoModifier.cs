namespace ToolkitEngine.Shooter
{
    public class ShooterAmmoModifier : BaseShooterModifier<ShooterAmmo>
    {
        protected override string[] propertyNames => new[]
        {
            "capacity",
        };
	}
}