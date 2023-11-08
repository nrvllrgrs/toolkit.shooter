using ToolkitEngine.Shooter;
using UnityEngine;
using UnityEditor;

namespace ToolkitEditor.Shooter
{
	[CustomEditor(typeof(ShooterSpread))]
	public class ShooterSpreadEditor : BaseToolkitEditor
    {
		#region Fields

		protected SerializedProperty m_shooterControls;
		protected SerializedProperty m_shooters;

		protected SerializedProperty m_spreadPerShot;
		protected SerializedProperty m_spreadLimits;
		protected SerializedProperty m_recoveryDelay;
		protected SerializedProperty m_recoveryRate;

		#endregion

		#region Methods

		private void OnEnable()
		{
			m_shooterControls = serializedObject.FindProperty(nameof(m_shooterControls));
			m_shooters = serializedObject.FindProperty(nameof(m_shooters));

			m_spreadPerShot = serializedObject.FindProperty(nameof(m_spreadPerShot));
			m_spreadLimits = serializedObject.FindProperty(nameof(m_spreadLimits));
			m_recoveryDelay = serializedObject.FindProperty(nameof(m_recoveryDelay));
			m_recoveryRate = serializedObject.FindProperty(nameof(m_recoveryRate));
		}

		protected override void DrawProperties()
		{
			EditorGUILayout.PropertyField(m_shooterControls);
			EditorGUILayout.PropertyField(m_shooters);

			EditorGUILayout.Separator();

			EditorGUILayout.PropertyField(m_spreadPerShot);
			EditorGUILayoutUtility.MinMaxSlider(m_spreadLimits, 0f, 90f);

			//float min = m_spreadLimits.vector2Value.x;
			//float max = m_spreadLimits.vector2Value.y;

			//EditorGUI.BeginChangeCheck();
			//EditorGUILayout.MinMaxSlider(m_spreadLimits.displayName, ref min, ref max, 0f, 90f);

			//if (EditorGUI.EndChangeCheck())
			//{
			//	m_spreadLimits.vector2Value = new Vector2(min, max);
			//}

			EditorGUILayout.LabelField("Recovery", EditorStyles.boldLabel);
			++EditorGUI.indentLevel;
			EditorGUILayout.PropertyField(m_recoveryDelay, new GUIContent("Delay"));
			EditorGUILayout.PropertyField(m_recoveryRate, new GUIContent("Rate"));
			--EditorGUI.indentLevel;
		}

		#endregion
	}
}