using UnityEngine;
using Unity.VisualScripting;

namespace ToolkitEngine.Shooter.VisualScripting
{
	[AddComponentMenu("")]
	public class OnShooterControlShotBlockedMessageListener : MessageListener
	{
		private void Start() => GetComponent<ShooterControl>()?.onShotBlocked.AddListener((value) =>
		{
			EventBus.Trigger(EventHooks.OnShooterControlShotBlocked, gameObject, value);
		});
	}
}
