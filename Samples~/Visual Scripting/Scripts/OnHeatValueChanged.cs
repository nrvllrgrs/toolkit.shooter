using System;
using Unity.VisualScripting;

namespace ToolkitEngine.Shooter.VisualScripting
{
	[UnitTitle("On Heat Value Changed"), UnitSurtitle("Heat")]
	[UnitCategory("Events/Weapons")]
	public class OnHeatValueChanged : BaseEventUnit<float>
	{
		public override Type MessageListenerType => typeof(OnHeatValueChangedMessageListener);
	}
}