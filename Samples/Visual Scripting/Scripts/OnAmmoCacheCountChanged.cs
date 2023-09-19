using System;
using Unity.VisualScripting;

namespace ToolkitEngine.Weapons.VisualScripting
{
	[UnitTitle("On Ammo Cache Count Changed"), UnitSurtitle("Ammo Cache")]
	[UnitCategory("Events/Weapons")]
	public class OnAmmoCacheCountChanged : BaseEventUnit<int>
	{
		public override Type MessageListenerType => typeof(OnAmmoCacheCountChangedMessageListener);
	}
}