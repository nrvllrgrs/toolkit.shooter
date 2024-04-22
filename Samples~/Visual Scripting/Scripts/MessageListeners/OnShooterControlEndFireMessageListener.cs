using UnityEngine;
using Unity.VisualScripting;

namespace ToolkitEngine.Shooter.VisualScripting
{
	[AddComponentMenu("")]
	public class OnShooterControlEndFireMessageListener : MessageListener
	{
		private void Start() => GetComponent<ShooterControl>()?.onEndFire.AddListener((value) =>
		{
			EventBus.Trigger(EventHooks.OnShooterControlEndFire, gameObject, value);
		});
	}
}
