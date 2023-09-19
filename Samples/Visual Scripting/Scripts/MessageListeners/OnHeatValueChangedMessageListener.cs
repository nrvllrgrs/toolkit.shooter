using UnityEngine;
using Unity.VisualScripting;

namespace ToolkitEngine.Weapons.VisualScripting
{
	[AddComponentMenu("")]
	public class OnHeatValueChangedMessageListener : MessageListener
	{
		private void Start() => GetComponent<IHeat>()?.onValueChanged.AddListener((value) =>
		{
			EventBus.Trigger(EventHooks.OnHeatValueChanged, gameObject, value);
		});
	}
}
