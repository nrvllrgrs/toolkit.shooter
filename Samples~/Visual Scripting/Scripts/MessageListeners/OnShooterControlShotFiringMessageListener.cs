using UnityEngine;
using Unity.VisualScripting;

namespace ToolkitEngine.Shooter.VisualScripting
{
	[AddComponentMenu("")]
	public class OnShooterControlShotFiringMessageListener : MessageListener
	{
		private void Start() => GetComponent<ShooterControl>()?.onShotFiring.AddListener((value) =>
		{
			EventBus.Trigger(EventHooks.OnShooterControlShotFiring, gameObject, value);
		});
	}
}
