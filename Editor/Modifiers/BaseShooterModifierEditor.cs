using UnityEngine;
using UnityEditor;
using ToolkitEngine;
using ToolkitEngine.Shooter;

namespace ToolkitEditor.Shooter
{
    [CustomEditor(typeof(BaseShooterModifier), true)]
    public class BaseShooterModifierEditor : BaseToolkitEditor
    {
		#region Fields

		protected BaseShooterModifier m_shooterModifer;
		protected SerializedProperty m_filterKeys;
		protected SerializedProperty m_filterValues;

		#endregion

		#region Methods

		protected void OnEnable()
		{
			m_shooterModifer = target as BaseShooterModifier;

			var filters = serializedObject.FindProperty("m_filters");
			m_filterKeys = filters.FindPropertyRelative("keys");
			m_filterValues = filters.FindPropertyRelative("values");
		}

		protected override void DrawProperties()
		{
			for (int i = 0; i < m_filterKeys.arraySize; ++i)
			{
				var key = m_filterKeys.GetArrayElementAtIndex(i).stringValue;
				var filterProp = m_filterValues.GetArrayElementAtIndex(i);
				EditorGUILayout.PropertyField(filterProp, new GUIContent($"{key.CamelCaseToTitleCase()} Filter"));
			}
		}

		#endregion
	}
}