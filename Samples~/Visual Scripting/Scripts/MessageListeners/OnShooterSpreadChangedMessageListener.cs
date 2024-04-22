using UnityEngine;
using Unity.VisualScripting;

namespace ToolkitEngine.Shooter.VisualScripting
{
	[AddComponentMenu("")]
	public class OnShooterSpreadChangedMessageListener : MessageListener
	{
		private void Start() => GetComponent<BaseMuzzleShooter>()?.onSpreadChanged.AddListener((value) =>
		{
			EventBus.Trigger(nameof(OnShooterSpreadChanged), gameObject, value);
		});
	}
}
