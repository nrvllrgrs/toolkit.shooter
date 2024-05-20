using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ToolkitEngine.Shooter;

namespace ToolkitEditor.Shooter
{
	[CustomEditor(typeof(AmmoType))]
	public class AmmoTypeEditor : Editor
	{
		#region Fields

		protected AmmoType m_ammoType;

		protected SerializedProperty m_id;

		protected SerializedProperty m_name;
		protected SerializedProperty m_description;
		protected SerializedProperty m_icon;
		protected SerializedProperty m_color;

		#endregion

		#region Methods

		private void OnEnable()
		{
			m_ammoType = target as AmmoType;

			m_id = serializedObject.FindProperty(nameof(m_id));

			m_name = serializedObject.FindProperty(nameof(m_name));
			m_description = serializedObject.FindProperty(nameof(m_description));
			m_icon = serializedObject.FindProperty(nameof(m_icon));
			m_color = serializedObject.FindProperty(nameof(m_color));
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUI.BeginDisabledGroup(true);
			EditorGUILayout.PropertyField(m_id, new GUIContent("ID"));
			EditorGUI.EndDisabledGroup();

			EditorGUILayout.Separator();

			EditorGUILayout.LabelField("Info", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(m_name);
			EditorGUILayout.PropertyField(m_description);
			EditorGUILayout.ObjectField(m_icon, typeof(Sprite), GUILayout.Height(64), GUILayout.Width(64 + EditorGUIUtility.labelWidth));
			EditorGUILayout.PropertyField(m_color);

			serializedObject.ApplyModifiedProperties();
		}

		#endregion
	}
}