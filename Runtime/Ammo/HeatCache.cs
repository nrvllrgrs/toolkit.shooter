using UnityEngine;
using UnityEngine.Events;

namespace ToolkitEngine.Weapons
{
	public class HeatCache : BaseHeatCache
	{
		#region Fields

		[SerializeField]
		private Heat m_heat = new();

		#endregion

		#region Properties

		public override float maximum { get => m_heat.maximum; set => m_heat.maximum = value; }
		public override float value { get => m_heat.value; set => m_heat.value = value; }
		public override bool isOverheated => m_heat.isOverheated;
		public override bool paused { get => m_heat.paused; set => m_heat.paused = value; }
		public override UnityEvent<float> onValueChanged => m_heat.onValueChanged;
		public override UnityEvent onOverheated => m_heat.onOverheated;
		public override UnityEvent onCooled => m_heat.onCooled;

		#endregion

		#region Methods

		public override void Overheat()
		{
			m_heat.Overheat();
		}

		public override void Vent()
		{
			m_heat.Vent();
		}

		#endregion
	}
}