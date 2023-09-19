using UnityEngine;
using Unity.VisualScripting;

namespace ToolkitEngine.Weapons.VisualScripting
{
	[AddComponentMenu("")]
	public class OnAmmoCacheCountChangedMessageListener : MessageListener
	{
		private void Start() => GetComponent<BaseAmmoCache>()?.onCountChanged.AddListener((value) =>
		{
			EventBus.Trigger(EventHooks.OnAmmoCacheCountChanged, gameObject, value);
		});
	}
}
