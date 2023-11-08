using System;
using Unity.VisualScripting;

namespace ToolkitEngine.Shooter.VisualScripting
{
	[UnitTitle("On Cooled"), UnitSurtitle("Heat")]
	[UnitCategory("Events/Weapons")]
	public class OnHeatCooled : BaseEventUnit<Null>
	{
		public override Type MessageListenerType => typeof(OnHeatCooledMessageListener);
	}
}