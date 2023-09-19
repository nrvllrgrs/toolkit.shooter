using System;
using Unity.VisualScripting;

namespace ToolkitEngine.Weapons.VisualScripting
{
	[UnitTitle("On Overheated"), UnitSurtitle("Heat")]
	[UnitCategory("Events/Weapons")]
	public class OnHeatOverheated : BaseEventUnit<Null>
	{
		public override Type MessageListenerType => typeof(OnHeatOverheatedMessageListener);
	}
}