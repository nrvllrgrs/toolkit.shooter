using UnityEngine;
using UnityEngine.Events;

namespace ToolkitEngine.Shooter
{
	public class AmmoCacheProxy : BaseAmmoCache
	{
		#region Fields

		[SerializeField]
		private AmmoCache m_ammoCache;

		[SerializeField]
		private UnityEvent<int> m_onCountChanged;

		#endregion

		#region Properties

		public override AmmoType ammoType => m_ammoCache?.ammoType;

		public override int capacity
		{
			get
			{
				return m_ammoCache != null
					? m_ammoCache.capacity
					: -1;
			}
			set
			{
				if (m_ammoCache != null)
				{
					m_ammoCache.capacity = value;
				}
			}
		}

		public override int count
		{
			get
			{
				return m_ammoCache != null
					? m_ammoCache.count
					: -1;
			}
			set
			{
				if (m_ammoCache != null)
				{
					m_ammoCache.count = value;
				}
			}
		}

		public override UnityEvent<int> onCountChanged => m_onCountChanged;

		#endregion

		#region Methods

		private void OnEnable()
		{
			Register();
		}

		private void OnDisable()
		{
			Unregister();
		}

		private void Register()
		{
			if (m_ammoCache != null)
			{
				// Start listening to events
				m_ammoCache.onCountChanged.AddListener(AmmoCache_CountChanged);
				AmmoCache_CountChanged(count);
			}
		}

		private void Unregister()
		{
			if (m_ammoCache != null)
			{
				// Stop listening to events
				m_ammoCache.onCountChanged.RemoveListener(AmmoCache_CountChanged);
			}
		}

		public void Assign(AmmoCache ammoCache)
		{
			// Ensure existing cache is unassigned
			Unassign();

			m_ammoCache = ammoCache;
			Register();
		}

		public void Unassign()
		{
			if (m_ammoCache == null)
				return;

			Unregister();
			m_ammoCache = null;
		}

		private void AmmoCache_CountChanged(int count)
		{
			onCountChanged?.Invoke(count);
		}

		#endregion
	}
}