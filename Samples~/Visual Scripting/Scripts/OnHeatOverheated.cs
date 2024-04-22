using System;
using Unity.VisualScripting;

namespace ToolkitEngine.Shooter.VisualScripting
{
	[UnitTitle("On Overheated"), UnitSurtitle("Heat")]
	[UnitCategory("Events/Weapons/Heat")]
	public class OnHeatOverheated : BaseEventUnit<EmptyEventArgs>
	{
		public override Type MessageListenerType => typeof(OnHeatOverheatedMessageListener);
	}
}