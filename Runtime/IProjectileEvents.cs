using UnityEngine.Events;

namespace ToolkitEngine.Shooter
{
	public interface IProjectileEvents
    {
		UnityEvent<ProjectileEventArgs> onCollision { get; }
		UnityEvent<ProjectileEventArgs> onDetonated { get; }
	}
}