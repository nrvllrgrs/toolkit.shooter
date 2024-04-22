using UnityEngine;
using Unity.VisualScripting;

namespace ToolkitEngine.Shooter.VisualScripting
{
	[AddComponentMenu("")]
	public class OnChargeDechargingMessageListener : MessageListener
	{
		private void Start() => GetComponent<ShooterCharge>()?.onDecharging.AddListener(() =>
		{
			EventBus.Trigger(nameof(OnChargeDecharging), gameObject);
		});
	}
}
