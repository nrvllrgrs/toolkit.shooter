using UnityEngine;
using Unity.VisualScripting;

namespace ToolkitEngine.Shooter.VisualScripting
{
	[AddComponentMenu("")]
	public class OnChargeValueChangedMessageListener : MessageListener
	{
		private void Start() => GetComponent<ShooterCharge>()?.onValueChanged.AddListener((value) =>
		{
			EventBus.Trigger(EventHooks.OnChargeValueChanged, gameObject, value);
		});
	}
}
