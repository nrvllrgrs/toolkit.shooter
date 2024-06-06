using UnityEngine;
using Unity.VisualScripting;

namespace ToolkitEngine.Shooter.VisualScripting
{
	[AddComponentMenu("")]
	public class OnCriticalHitMessageListener : MessageListener
	{
		private void Start() => GetComponent<ShooterCritical>()?.onCriticalHit.AddListener((value) =>
		{
			EventBus.Trigger(nameof(OnCriticalHit), gameObject, value);
		});
	}
}
