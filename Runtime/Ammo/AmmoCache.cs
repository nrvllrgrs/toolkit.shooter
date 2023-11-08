using UnityEngine;
using UnityEngine.Events;

namespace ToolkitEngine.Shooter
{
	public class AmmoCache : BaseAmmoCache
	{
		#region Fields

		[SerializeField]
		private Ammo m_ammo = new();

		#endregion

		#region Properties

		public override AmmoType ammoType => m_ammo.ammoType;
		public override int capacity { get => m_ammo.capacity; set => m_ammo.capacity = value; }
		public override int count { get => m_ammo.count; set => m_ammo.count = value; }
		public override UnityEvent<int> onCountChanged => m_ammo.onCountChanged;

		#endregion
	}
}