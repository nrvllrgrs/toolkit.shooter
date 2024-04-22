using System;
using Unity.VisualScripting;

namespace ToolkitEngine.Shooter.VisualScripting
{
	[UnitTitle("On Cooled"), UnitSurtitle("Heat")]
	[UnitCategory("Events/Weapons/Heat")]
	public class OnHeatCooled : BaseEventUnit<EmptyEventArgs>
	{
		public override Type MessageListenerType => typeof(OnHeatCooledMessageListener);
	}
}