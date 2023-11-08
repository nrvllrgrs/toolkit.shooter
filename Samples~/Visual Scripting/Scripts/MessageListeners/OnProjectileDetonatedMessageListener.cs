using UnityEngine;
using Unity.VisualScripting;

namespace ToolkitEngine.Shooter.VisualScripting
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
