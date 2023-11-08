using UnityEngine;
using Unity.VisualScripting;

namespace ToolkitEngine.Shooter.VisualScripting
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
