namespace ToolkitEngine.Shooter
{
	public class ShooterRecoilModifier : BaseShooterModifier<ShooterRecoil>
	{
		protected override string[] propertyNames => new[]
		{
			"recoilPerShot",
			"recoveryDelay",
			"recoveryRate",
		};
	}
}