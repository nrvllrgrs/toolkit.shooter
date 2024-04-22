using System;
using Unity.VisualScripting;

namespace ToolkitEngine.Shooter.VisualScripting
{
	[UnitTitle("On Shot Firing"), UnitSurtitle("Shooter Control")]
	[UnitCategory("Events/Weapons/Shooter Control")]
	public class OnShooterControlShotFiring : BaseEventUnit<ShooterControl>
	{
		public override Type MessageListenerType => typeof(OnShooterControlShotFiringMessageListener);
	}
}