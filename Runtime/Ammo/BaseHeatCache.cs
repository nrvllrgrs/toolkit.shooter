using UnityEngine;
using UnityEngine.Events;

namespace ToolkitEngine.Weapons
{
    public abstract class BaseHeatCache : MonoBehaviour, IHeat
    {
        public abstract float maximum { get; set; }
        public abstract float value { get; set; }
        public float normalizedValue => value / maximum;
        public abstract bool isOverheated { get; }
        public abstract bool paused { get; set; }
        public abstract UnityEvent<float> onValueChanged { get; }
        public abstract UnityEvent onOverheated { get; }
        public abstract UnityEvent onCooled { get; }
        public abstract void Overheat();
        public abstract void Vent();

    }
}