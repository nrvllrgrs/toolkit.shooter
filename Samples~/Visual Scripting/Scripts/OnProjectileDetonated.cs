using System;
using Unity.VisualScripting;

namespace ToolkitEngine.Weapons.VisualScripting
{
    [UnitTitle("On Detonated"), UnitSurtitle("Projectile")]
    public class OnProjectileDetonated : BaseProjectileEventUnit
    {
        public override Type MessageListenerType => typeof(OnProjectileDetonatedMessageListener);
    }
}