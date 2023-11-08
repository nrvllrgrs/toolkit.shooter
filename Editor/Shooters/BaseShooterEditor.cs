using UnityEditor;
using ToolkitEngine.Shooter;

namespace ToolkitEditor.Shooter
{
	[CustomEditor(typeof(BaseShooter), true)]
    public class BaseShooterEditor : BaseToolkitEditor
	{
		#region Fields

		protected SerializedProperty m_onFiring;
		protected SerializedProperty m_onFired;

		#endregion

		#region Methods

		protected virtual void OnEnable()
		{
			m_onFiring = serializedObject.FindProperty(nameof(m_onFiring));
			m_onFired = serializedObject.FindProperty(nameof(m_onFired));
		}

		protected override void DrawEvents()
		{
			if (EditorGUILayoutUtility.Foldout(m_onFiring, "Events"))
			{
				EditorGUILayout.PropertyField(m_onFiring);
				EditorGUILayout.PropertyField(m_onFired);

				DrawNestedEvents();
			}
		}

		#endregion
	}
}