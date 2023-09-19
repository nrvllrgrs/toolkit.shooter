using UnityEngine;
using Unity.VisualScripting;

namespace ToolkitEngine.Weapons.VisualScripting
{
	[AddComponentMenu("")]
	public class OnProjectileDetonatedMessageListener : MessageListener
	{
		private void Start() => GetComponent<Projectile>()?.onDetonated.AddListener((value) =>
		{
			EventBus.Trigger(EventHooks.OnProjectileDetonated, gameObject, value);
		});
	}
}
