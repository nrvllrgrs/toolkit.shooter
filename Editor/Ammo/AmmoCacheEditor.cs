using UnityEditor;
using ToolkitEngine.Shooter;

namespace ToolkitEditor.Shooter
{
	[CustomEditor(typeof(AmmoCache))]
	public class AmmoCacheEditor : BaseToolkitEditor
	{
		#region Fields

		protected SerializedProperty m_ammo;
		protected SerializedProperty m_onCountChanged;

		#endregion

		#region Methods

		private void OnEnable()
		{
			m_ammo = serializedObject.FindProperty(nameof(m_ammo));
			m_onCountChanged = m_ammo.FindPropertyRelative(nameof(m_onCountChanged));
		}

		protected override void DrawProperties()
		{
			EditorGUILayout.PropertyField(m_ammo);
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