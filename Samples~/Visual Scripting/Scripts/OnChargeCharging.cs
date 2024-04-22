using System;
using Unity.VisualScripting;

namespace ToolkitEngine.Shooter.VisualScripting
{
	[UnitTitle("On Charging"), UnitSurtitle("Shooter Charge")]
	[UnitCategory("Events/Weapons/Charge")]
	public class OnChargeCharging : BaseEventUnit<EmptyEventArgs>
	{
		protected override bool showEventArgs => false;
		public override Type MessageListenerType => typeof(OnChargeChargingMessageListener);
	}
}