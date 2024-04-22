using System;
using Unity.VisualScripting;

namespace ToolkitEngine.Shooter.VisualScripting
{
	[UnitTitle("On Shot Fired"), UnitSurtitle("Shooter Control")]
	[UnitCategory("Events/Weapons/Shooter Control")]
	public class OnShooterControlShotFired : BaseEventUnit<ShooterControl>
	{
		public override Type MessageListenerType => typeof(OnShooterControlShotFiredMessageListener);
	}
}