using UnityEngine;
using Unity.VisualScripting;

namespace ToolkitEngine.Shooter.VisualScripting
{
	[AddComponentMenu("")]
	public class OnShooterControlBeginFireMessageListener : MessageListener
	{
		private void Start() => GetComponent<ShooterControl>()?.onBeginFire.AddListener((value) =>
		{
			EventBus.Trigger(EventHooks.OnShooterControlBeginFire, gameObject, value);
		});
	}
}
