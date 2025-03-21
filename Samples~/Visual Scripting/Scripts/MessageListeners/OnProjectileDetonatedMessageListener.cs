using UnityEngine;
using Unity.VisualScripting;

namespace ToolkitEngine.Shooter.VisualScripting
{
	[AddComponentMenu("")]
	public class OnProjectileDetonatedMessageListener : MessageListener
	{
		private void Start() => GetComponent<IProjectileEvents>()?.onDetonated.AddListener((value) =>
		{
			EventBus.Trigger(nameof(OnProjectileDetonated), gameObject, value);
		});
	}
}
