using System;
using UnityEngine;

namespace ToolkitEngine.Shooter
{
    [CreateAssetMenu(menuName = "Toolkit/Weapon/Ammo")]
    public class AmmoType : ScriptableObject
    {
		#region Fields

		[SerializeField]
		private string m_id = Guid.NewGuid().ToString();

		[SerializeField]
		private string m_name;

		[SerializeField, TextArea]
		private string m_description;

		[SerializeField]
		private Sprite m_icon;

		[SerializeField]
		private Color m_color = Color.white;

		#endregion

		#region Properties

		public string id => m_id;
		public new string name { get => m_name; set => m_name = value; }
		public string description => m_description;
		public Sprite icon => m_icon;
		public Color color => m_color;

		#endregion
	}
}