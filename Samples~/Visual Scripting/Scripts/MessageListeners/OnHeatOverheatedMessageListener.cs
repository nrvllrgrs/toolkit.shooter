using UnityEngine;
using Unity.VisualScripting;

namespace ToolkitEngine.Shooter.VisualScripting
{
	[AddComponentMenu("")]
	public class OnHeatOverheatedMessageListener : MessageListener
	{
		private void Start() => GetComponent<IHeat>()?.onOverheated.AddListener(() =>
		{
			EventBus.Trigger(EventHooks.OnHeatOverheated, gameObject);
		});
	}
}
