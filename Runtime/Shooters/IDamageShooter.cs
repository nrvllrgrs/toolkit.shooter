using ToolkitEngine.Health;

namespace ToolkitEngine.Shooter
{
	public interface IDamageShooter
	{
		ImpactDamage impactDamage { get; }
		SplashDamage splashDamage { get; }
	}
}