using System;
using Unity.VisualScripting;

namespace ToolkitEngine.Shooter.VisualScripting
{
	[UnitTitle("On Charged"), UnitSurtitle("Shooter Charge")]
	[UnitCategory("Events/Weapons/Charge")]
	public class OnChargeValueChanged : BaseEventUnit<float>
	{
		public override Type MessageListenerType => typeof(OnChargeValueChangedMessageListener);
	}
}