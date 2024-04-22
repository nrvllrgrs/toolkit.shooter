using System;
using Unity.VisualScripting;

namespace ToolkitEngine.Shooter.VisualScripting
{
	[UnitTitle("On Decharged"), UnitSurtitle("Shooter Charge")]
	[UnitCategory("Events/Weapons/Charge")]
	public class OnChargeDecharged : BaseEventUnit<EmptyEventArgs>
	{
		protected override bool showEventArgs => false;
		public override Type MessageListenerType => typeof(OnChargeDechargedMessageListener);
	}
}