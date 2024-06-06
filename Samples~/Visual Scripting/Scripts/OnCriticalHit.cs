using System;
using Unity.VisualScripting;
using ToolkitEngine.Health;

namespace ToolkitEngine.Shooter.VisualScripting
{
	[UnitTitle("On Critical Hit"), UnitSurtitle("Shooter Critical")]
	[UnitCategory("Events/Weapons/Critical")]
	public class OnCriticalHit : BaseEventUnit<HealthEventArgs>
	{
		protected override bool showEventArgs => false;
		public override Type MessageListenerType => typeof(OnCriticalHitMessageListener);
	}
}