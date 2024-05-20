using System;
using Unity.VisualScripting;

namespace ToolkitEngine.Shooter.VisualScripting
{
	[UnitTitle("On Collision"), UnitSurtitle("Projectile")]
	public class OnProjectileCollision : BaseProjectileEventUnit
	{
		public override Type MessageListenerType => typeof(OnProjectileCollisionMessageListener);
	}
}