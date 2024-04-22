using UnityEngine;
using Unity.VisualScripting;

namespace ToolkitEngine.Shooter.VisualScripting
{
	[AddComponentMenu("")]
	public class OnChargeChargingMessageListener : MessageListener
	{
		private void Start() => GetComponent<ShooterCharge>()?.onCharging.AddListener(() =>
		{
			EventBus.Trigger(nameof(OnChargeCharging), gameObject);
		});
	}
}
