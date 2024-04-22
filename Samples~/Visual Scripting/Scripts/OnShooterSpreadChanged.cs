using System;
using Unity.VisualScripting;

namespace ToolkitEngine.Shooter.VisualScripting
{
	[UnitTitle("On Shooter Spread Changed"), UnitSurtitle("Shooter")]
	[UnitCategory("Events/Weapons")]
	public class OnShooterSpreadChanged : BaseEventUnit<ShooterEventArgs>
	{
		public override Type MessageListenerType => typeof(OnShooterSpreadChangedMessageListener);
	}
}