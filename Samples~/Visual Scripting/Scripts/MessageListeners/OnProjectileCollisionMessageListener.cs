using UnityEngine;
using Unity.VisualScripting;

namespace ToolkitEngine.Shooter.VisualScripting
{
	[AddComponentMenu("")]
	public class OnProjectileCollisionMessageListener : MessageListener
	{
		private void Start() => GetComponent<IProjectileEvents>()?.onDetonated.AddListener((value) =>
		{
			EventBus.Trigger(nameof(OnProjectileCollision), gameObject, value);
		});
	}
}
