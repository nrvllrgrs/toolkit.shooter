using System;
using Unity.VisualScripting;

namespace ToolkitEngine.Shooter.VisualScripting
{
	[UnitTitle("On Begin Fire"), UnitSurtitle("Shooter Control")]
	[UnitCategory("Events/Weapons/Shooter Control")]
	public class OnShooterControlBeginFire : BaseEventUnit<ShooterControl>
	{
		public override Type MessageListenerType => typeof(OnShooterControlBeginFireMessageListener);
	}
}