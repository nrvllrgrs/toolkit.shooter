using UnityEditor;
using ToolkitEngine.Shooter;

namespace ToolkitEditor.Shooter
{
	[CustomEditor(typeof(ShooterMode))]
	public class ShooterModeEditor : BaseToolkitEditor
    {
		#region Fields

		protected SerializedProperty m_shooterControls;
		protected SerializedProperty m_onSelecting;
		protected SerializedProperty m_onSelected;

		#endregion

		#region Methods

		private void OnEnable()
		{
			m_shooterControls = serializedObject.FindProperty(nameof(m_shooterControls));
			m_onSelecting = serializedObject.FindProperty(nameof(m_onSelecting));
			m_onSelected = serializedObject.FindProperty(nameof(m_onSelected));
		}

		protected override void DrawProperties()
		{
			EditorGUILayout.PropertyField(m_shooterControls);
		}

		protected override void DrawEvents()
		{
			if (EditorGUILayoutUtility.Foldout(m_onSelecting, "Events"))
			{
				EditorGUILayout.PropertyField(m_onSelecting);
				EditorGUILayout.PropertyField(m_onSelected);

				DrawNestedEvents();
			}
		}

		#endregion
	}
}