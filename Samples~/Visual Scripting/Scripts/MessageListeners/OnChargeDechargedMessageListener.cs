using UnityEngine;
using Unity.VisualScripting;

namespace ToolkitEngine.Shooter.VisualScripting
{
	[AddComponentMenu("")]
	public class OnChargeDechargedMessageListener : MessageListener
	{
		private void Start() => GetComponent<ShooterCharge>()?.onDecharged.AddListener(() =>
		{
			EventBus.Trigger(nameof(OnChargeDecharged), gameObject);
		});
	}
}
