using System;
using Unity.VisualScripting;

namespace ToolkitEngine.Shooter.VisualScripting
{
	[UnitTitle("On Shot Blocked"), UnitSurtitle("Shooter Control")]
	[UnitCategory("Events/Weapons/Shooter Control")]
	public class OnShooterControlShotBlocked : BaseEventUnit<ShooterControl>
	{
		public override Type MessageListenerType => typeof(OnShooterControlShotBlockedMessageListener);
	}
}