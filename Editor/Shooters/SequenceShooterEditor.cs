using UnityEditor;
using ToolkitEngine.Shooter;

namespace ToolkitEditor.Shooter
{
	[CustomEditor(typeof(SequenceShooter))]
    public class SequenceShooterEditor : BaseShooterEditor
    {
		#region Fields

		protected SerializedProperty m_steps;
		protected SerializedProperty m_skipInvalid;

		protected SerializedProperty m_onIndexChanged;

		#endregion

		#region Methods

		protected override void OnEnable()
		{
			base.OnEnable();
			m_steps = serializedObject.FindProperty(nameof(m_steps));
			m_skipInvalid = serializedObject.FindProperty(nameof(m_skipInvalid));
			m_onIndexChanged = serializedObject.FindProperty(nameof(m_onIndexChanged));
		}

		protected override void DrawProperties()
		{
			EditorGUILayout.PropertyField(m_steps);
			EditorGUILayout.PropertyField(m_skipInvalid);
		}

		protected override void DrawNestedEvents()
		{
			base.DrawNestedEvents();
			EditorGUILayout.PropertyField(m_onIndexChanged);
		}

		#endregion
	}
}