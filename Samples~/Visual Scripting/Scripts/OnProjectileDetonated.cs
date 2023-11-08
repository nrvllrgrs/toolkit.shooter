using System;
using Unity.VisualScripting;

namespace ToolkitEngine.Shooter.VisualScripting
{
    [UnitTitle("On Detonated"), UnitSurtitle("Projectile")]
    public class OnProjectileDetonated : BaseProjectileEventUnit
    {
        public override Type MessageListenerType => typeof(OnProjectileDetonatedMessageListener);
    }
}