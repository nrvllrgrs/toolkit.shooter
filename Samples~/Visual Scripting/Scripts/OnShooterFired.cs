using System;
using Unity.VisualScripting;

namespace ToolkitEngine.Shooter.VisualScripting
{
	[UnitTitle("On Shooter Fired"), UnitSurtitle("Shooter")]
	[UnitCategory("Events/Weapons")]
	public class OnShooterFired : BaseEventUnit<ShooterEventArgs>
	{
		public override Type MessageListenerType => typeof(OnShooterFiredMessageListener);
	}
}