using UnityEngine.Events;

namespace ToolkitEngine.Shooter
{
    public interface IAmmo
    {
        AmmoType ammoType { get; }
		int capacity { get; set; }
		int count { get; set; }
        float normalizedCount { get; }
        UnityEvent<int> onCountChanged { get; }
    }
}