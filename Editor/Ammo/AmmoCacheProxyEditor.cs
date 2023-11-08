using UnityEditor;
using ToolkitEngine.Shooter;

namespace ToolkitEditor.Shooter
{
	[CustomEditor(typeof(AmmoCacheProxy))]
    public class AmmoCacheProxyEditor : BaseToolkitEditor
    {
		#region Fields

		protected SerializedProperty m_ammoCache;
		protected SerializedProperty m_onCountChanged;

		#endregion

		#region Methods

		private void OnEnable()
		{
			m_ammoCache = serializedObject.FindProperty(nameof(m_ammoCache));
			m_onCountChanged = serializedObject.FindProperty(nameof(m_onCountChanged));
		}

		protected override void DrawProperties()
		{
			EditorGUILayout.PropertyField(m_ammoCache);
		}

		protected override void DrawEvents()
		{
			if (EditorGUILayoutUtility.Foldout(m_onCountChanged, "Events"))
			{
				EditorGUILayout.PropertyField(m_onCountChanged);

				DrawNestedEvents();
			}
		}

		#endregion
	}
}