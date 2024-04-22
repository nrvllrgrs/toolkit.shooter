using UnityEngine;
using Unity.VisualScripting;

namespace ToolkitEngine.Shooter.VisualScripting
{
	[AddComponentMenu("")]
	public class OnShooterControlShotFiredMessageListener : MessageListener
	{
		private void Start() => GetComponent<ShooterControl>()?.onShotFired.AddListener((value) =>
		{
			EventBus.Trigger(EventHooks.OnShooterControlShotFired, gameObject, value);
		});
	}
}
