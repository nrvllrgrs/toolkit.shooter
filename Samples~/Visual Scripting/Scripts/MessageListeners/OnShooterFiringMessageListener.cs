using UnityEngine;
using Unity.VisualScripting;

namespace ToolkitEngine.Shooter.VisualScripting
{
	[AddComponentMenu("")]
	public class OnShooterFiringMessageListener : MessageListener
	{
		private void Start() => GetComponent<BaseShooter>()?.onFiring.AddListener((value) =>
		{
			EventBus.Trigger(nameof(OnShooterFiring), gameObject, value);
		});
	}
}
