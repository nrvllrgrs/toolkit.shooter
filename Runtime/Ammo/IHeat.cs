using UnityEngine.Events;

namespace ToolkitEngine.Weapons
{
    public interface IHeat
    {
        float maximum { get; }
        float value { get; set; }
        float normalizedValue { get; }
        bool isOverheated { get; }
        bool paused { get; set; }
		UnityEvent<float> onValueChanged { get; }
		UnityEvent onOverheated { get; }
		UnityEvent onCooled { get; }
	}
}