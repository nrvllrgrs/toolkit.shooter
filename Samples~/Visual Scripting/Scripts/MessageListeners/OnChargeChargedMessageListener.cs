using UnityEngine;
using Unity.VisualScripting;

namespace ToolkitEngine.Shooter.VisualScripting
{
	[AddComponentMenu("")]
	public class OnChargeChargedMessageListener : MessageListener
	{
		private void Start() => GetComponent<ShooterCharge>()?.onCharged.AddListener(() =>
		{
			EventBus.Trigger(nameof(OnChargeCharged), gameObject);
		});
	}
}
