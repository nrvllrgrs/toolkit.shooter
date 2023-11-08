using UnityEditor;
using ToolkitEngine.Shooter;
using UnityEngine;

namespace ToolkitEditor.Shooter
{
	[CustomEditor(typeof(ShooterCharge))]
    public class ShooterChargeEditor : BaseToolkitEditor
    {
		#region Fields

		protected SerializedProperty m_shooterControls;
		protected SerializedProperty m_maxCharge;
		protected SerializedProperty m_resetOnEndFire;

		protected SerializedProperty m_onValueChanged;
		protected SerializedProperty m_onCharging;
		protected SerializedProperty m_onCharged;
		protected SerializedProperty m_onDecharging;
		protected SerializedProperty m_onDecharged;

		#endregion

		#region Methods

		private void OnEnable()
		{
			m_shooterControls = serializedObject.FindProperty(nameof(m_shooterControls));
			m_maxCharge = serializedObject.FindProperty(nameof(m_maxCharge));
			m_resetOnEndFire = serializedObject.FindProperty(nameof(m_resetOnEndFire));

			m_onValueChanged = serializedObject.FindProperty(nameof(m_onValueChanged));
			m_onCharging = serializedObject.FindProperty(nameof(m_onCharging));
			m_onCharged = serializedObject.FindProperty(nameof(m_onCharged));
			m_onDecharging = serializedObject.FindProperty(nameof(m_onDecharging));
			m_onDecharged = serializedObject.FindProperty(nameof(m_onDecharged));
		}

		protected override void DrawProperties()
		{
			EditorGUILayout.PropertyField(m_shooterControls);

			EditorGUILayout.Separator();

			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(m_maxCharge);

			if (EditorGUI.EndChangeCheck() && Application.isPlaying)
			{
				var shooterCharge = target as ShooterCharge;
				shooterCharge.maxCharge = m_maxCharge.floatValue;
			}

			EditorGUILayout.PropertyField(m_resetOnEndFire);
		}

		protected override void DrawEvents()
		{
			if (EditorGUILayoutUtility.Foldout(m_onValueChanged, "Events"))
			{
				EditorGUILayout.PropertyField(m_onValueChanged);
				EditorGUILayout.PropertyField(m_onCharging);
				EditorGUILayout.PropertyField(m_onCharged);
				EditorGUILayout.PropertyField(m_onDecharging);
				EditorGUILayout.PropertyField(m_onDecharged);
			}
		}

		#endregion
	}
}