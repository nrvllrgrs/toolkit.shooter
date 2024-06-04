using ToolkitEngine.Shooter;
using UnityEditor;
using UnityEngine;

namespace ToolkitEditor.Shooter
{
	[CustomEditor(typeof(ShooterRecoil))]
	public class ShooterRecoilEditor : BaseToolkitEditor
    {
		#region Fields

		protected SerializedProperty m_shooterControls;

		protected SerializedProperty m_target;
		protected SerializedProperty m_pivot;
		protected SerializedProperty m_recoilPerShot;
		protected SerializedProperty m_variableRecoilPerShot;
		protected SerializedProperty m_maxRecoilPerShot;
		protected SerializedProperty m_recoilCurve;
		protected SerializedProperty m_maxRecoil;
		protected SerializedProperty m_recoveryDelay;
		protected SerializedProperty m_recoveryRate;

		#endregion

		#region Methods

		private void OnEnable()
		{
			m_shooterControls = serializedObject.FindProperty(nameof(m_shooterControls));

			m_target = serializedObject.FindProperty(nameof(m_target));
			m_pivot = serializedObject.FindProperty(nameof(m_pivot));
			m_recoilPerShot = serializedObject.FindProperty(nameof(m_recoilPerShot));
			m_variableRecoilPerShot = serializedObject.FindProperty(nameof(m_variableRecoilPerShot));
			m_maxRecoilPerShot = serializedObject.FindProperty(nameof(m_maxRecoilPerShot));
			m_recoilCurve = serializedObject.FindProperty(nameof(m_recoilCurve));
			m_maxRecoil = serializedObject.FindProperty(nameof(m_maxRecoil));
			m_recoveryDelay = serializedObject.FindProperty(nameof(m_recoveryDelay));
			m_recoveryRate = serializedObject.FindProperty(nameof(m_recoveryRate));
		}

		protected override void DrawProperties()
		{
			EditorGUILayout.PropertyField(m_shooterControls);

			EditorGUILayout.Separator();

			EditorGUILayout.PropertyField(m_target);
			EditorGUILayout.PropertyField(m_pivot);

			EditorGUILayout.Separator();

			EditorGUILayout.PropertyField(m_recoilPerShot);
			EditorGUILayout.PropertyField(m_variableRecoilPerShot);

			if (m_variableRecoilPerShot.boolValue)
			{
				++EditorGUI.indentLevel;
				EditorGUILayout.PropertyField(m_maxRecoilPerShot);
				EditorGUILayout.PropertyField(m_recoilCurve);
				--EditorGUI.indentLevel;
			}

			EditorGUILayout.PropertyField(m_maxRecoil);

			EditorGUILayout.LabelField("Recovery", EditorStyles.boldLabel);
			++EditorGUI.indentLevel;
			EditorGUILayout.PropertyField(m_recoveryDelay, new GUIContent("Delay"));
			EditorGUILayout.PropertyField(m_recoveryRate, new GUIContent("Rate"));
			--EditorGUI.indentLevel;
		}

		#endregion
	}
}