using System;
using Unity.VisualScripting;

namespace ToolkitEngine.Shooter.VisualScripting
{
	[UnitTitle("On Decharging"), UnitSurtitle("Shooter Charge")]
	[UnitCategory("Events/Weapons/Charge")]
	public class OnChargeDecharging : BaseEventUnit<EmptyEventArgs>
	{
		protected override bool showEventArgs => false;
		public override Type MessageListenerType => typeof(OnChargeDechargingMessageListener);
	}
}