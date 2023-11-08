using UnityEngine;
using UnityEditor;
using ToolkitEngine.Shooter;

namespace ToolkitEditor.Shooter
{
	[CustomPropertyDrawer(typeof(Ammo))]
    public class AmmoDrawer : PropertyDrawer
    {
		#region Methods

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);

			var ammoTypeProperty = property.FindPropertyRelative("m_ammoType");
			var capacityProperty = property.FindPropertyRelative("m_capacity");
			var countProperty = property.FindPropertyRelative("m_count");

			EditorGUIRectLayout.PropertyField(ref position, ammoTypeProperty);
			EditorGUIRectLayout.PropertyField(ref position, capacityProperty);
			EditorGUIRectLayout.IntSlider(ref position, countProperty, 0, capacityProperty.intValue);

			EditorGUI.EndProperty();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return EditorGUI.GetPropertyHeight(property.FindPropertyRelative("m_ammoType"))
				+ EditorGUI.GetPropertyHeight(property.FindPropertyRelative("m_capacity"))
				+ EditorGUI.GetPropertyHeight(property.FindPropertyRelative("m_count"))
				+ (EditorGUIUtility.standardVerticalSpacing * 3f);
		}

		#endregion
	}
}