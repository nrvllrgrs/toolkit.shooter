using UnityEngine;
using Unity.VisualScripting;

namespace ToolkitEngine.Weapons.VisualScripting
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
