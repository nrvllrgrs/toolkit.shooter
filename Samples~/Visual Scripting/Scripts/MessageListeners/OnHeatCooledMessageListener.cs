using UnityEngine;
using Unity.VisualScripting;

namespace ToolkitEngine.Shooter.VisualScripting
{
	[AddComponentMenu("")]
	public class OnHeatCooledMessageListener : MessageListener
	{
		private void Start() => GetComponent<IHeat>()?.onCooled.AddListener(() =>
		{
			EventBus.Trigger(EventHooks.OnHeatCooled, gameObject);
		});
	}
}
