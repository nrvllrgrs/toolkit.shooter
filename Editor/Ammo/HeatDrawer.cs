using UnityEngine;
using UnityEditor;
using ToolkitEngine.Shooter;

namespace ToolkitEditor.Shooter
{
	[CustomPropertyDrawer(typeof(Heat))]
    public class HeatDrawer : PropertyDrawer
    {
		#region Methods

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);

			var maximumProperty = property.FindPropertyRelative("m_maximum");
			EditorGUIRectLayout.PropertyField(ref position, maximumProperty);

			var heat = property.GetValue<Heat>();

			EditorGUI.BeginDisabledGroup(true);
			EditorGUI.Slider(position, "Value", heat.value, 0f, maximumProperty.floatValue);
			position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
			EditorGUI.EndDisabledGroup();

			var coolDelayProperty = property.FindPropertyRelative("m_coolDelay");
			if (EditorGUIRectLayout.Foldout(ref position, coolDelayProperty, "Cool"))
			{
				++EditorGUI.indentLevel;
				EditorGUIRectLayout.PropertyField(ref position, coolDelayProperty, new GUIContent("Delay"));
				EditorGUIRectLayout.PropertyField(ref position, property.FindPropertyRelative("m_coolRate"), new GUIContent("Rate"));
				--EditorGUI.indentLevel;
			}

			var overheatDelayProperty = property.FindPropertyRelative("m_overheatDelay");
			if (EditorGUIRectLayout.Foldout(ref position, overheatDelayProperty, "Overheat"))
			{
				++EditorGUI.indentLevel;
				EditorGUIRectLayout.PropertyField(ref position, overheatDelayProperty, new GUIContent("Delay"));
				EditorGUIRectLayout.PropertyField(ref position, property.FindPropertyRelative("m_overheatRate"), new GUIContent("Rate"));
				--EditorGUI.indentLevel;
			}

			EditorGUI.EndProperty();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			float height = EditorGUI.GetPropertyHeight(property.FindPropertyRelative("m_maximum"))
				+ (EditorGUIUtility.singleLineHeight * 3f)
				+ (EditorGUIUtility.standardVerticalSpacing * 4f);

			var coolDelayProperty = property.FindPropertyRelative("m_coolDelay");
			if (coolDelayProperty.isExpanded)
			{
				height += EditorGUI.GetPropertyHeight(coolDelayProperty)
					+ EditorGUI.GetPropertyHeight(property.FindPropertyRelative("m_coolRate"))
					+ (EditorGUIUtility.standardVerticalSpacing * 2f);
			}

			var overheatDelayProperty = property.FindPropertyRelative("m_overheatDelay");
			if (overheatDelayProperty.isExpanded)
			{
				height += EditorGUI.GetPropertyHeight(overheatDelayProperty)
					+ EditorGUI.GetPropertyHeight(property.FindPropertyRelative("m_overheatRate"))
					+ (EditorGUIUtility.standardVerticalSpacing * 2f);
			}

			return height;
		}

		#endregion
	}
}