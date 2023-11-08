using UnityEngine;
using UnityEngine.Events;

namespace ToolkitEngine.Shooter
{
    public abstract class BaseAmmoCache : MonoBehaviour, IAmmo
    {
        public abstract AmmoType ammoType { get; }
        public abstract int capacity { get; set; }
        public abstract int count { get; set; }
        public float normalizedCount => (float)count / capacity;
        public abstract UnityEvent<int> onCountChanged { get; }
    }
}
