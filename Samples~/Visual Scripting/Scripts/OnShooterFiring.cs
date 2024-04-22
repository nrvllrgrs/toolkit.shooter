using System;
using Unity.VisualScripting;

namespace ToolkitEngine.Shooter.VisualScripting
{
	[UnitTitle("On Shooter Firing"), UnitSurtitle("Shooter")]
	[UnitCategory("Events/Weapons")]
	public class OnShooterFiring : BaseEventUnit<ShooterEventArgs>
	{
		public override Type MessageListenerType => typeof(OnShooterFiringMessageListener);
	}
}