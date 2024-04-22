using System;
using Unity.VisualScripting;

namespace ToolkitEngine.Shooter.VisualScripting
{
	[UnitTitle("On Charged"), UnitSurtitle("Shooter Charge")]
	[UnitCategory("Events/Weapons/Charge")]
	public class OnChargeCharged : BaseEventUnit<EmptyEventArgs>
	{
		protected override bool showEventArgs => false;
		public override Type MessageListenerType => typeof(OnChargeChargedMessageListener);
	}
}