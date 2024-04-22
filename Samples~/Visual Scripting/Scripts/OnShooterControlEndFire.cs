using System;
using Unity.VisualScripting;

namespace ToolkitEngine.Shooter.VisualScripting
{
	[UnitTitle("On End Fire"), UnitSurtitle("Shooter Control")]
	[UnitCategory("Events/Weapons/Shooter Control")]
	public class OnShooterControlEndFire : BaseEventUnit<ShooterControl>
	{
		public override Type MessageListenerType => typeof(OnShooterControlEndFireMessageListener);
	}
}