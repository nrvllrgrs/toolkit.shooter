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
		protected SerializedProperty m_onDamageDealt;

		#endregion

		#region Methods

		protected virtual void OnEnable()
		{
			m_onFiring = serializedObject.FindProperty(nameof(m_onFiring));
			m_onFired = serializedObject.FindProperty(nameof(m_onFired));
			m_onDamageDealt = serializedObject.FindProperty(nameof(m_onDamageDealt));
		}

		protected override void DrawEvents()
		{
			if (EditorGUILayoutUtility.Foldout(m_onFiring, "Events"))
			{
				EditorGUILayout.PropertyField(m_onFiring);
				EditorGUILayout.PropertyField(m_onFired);
				EditorGUILayout.PropertyField(m_onDamageDealt);

				DrawNestedEvents();
			}
		}

		#endregion
	}
}