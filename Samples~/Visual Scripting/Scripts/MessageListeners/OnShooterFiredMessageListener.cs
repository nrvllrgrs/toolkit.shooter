using UnityEngine;
using Unity.VisualScripting;

namespace ToolkitEngine.Shooter.VisualScripting
{
	[AddComponentMenu("")]
	public class OnShooterFiredMessageListener : MessageListener
	{
		private void Start() => GetComponent<BaseShooter>()?.onFired.AddListener((value) =>
		{
			EventBus.Trigger(nameof(OnShooterFired), gameObject, value);
		});
	}
}
