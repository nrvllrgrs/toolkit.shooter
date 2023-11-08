using UnityEditor;
using ToolkitEngine.Shooter;

namespace ToolkitEditor.Shooter
{
    [CustomEditor(typeof(HeatCache))]
    public class HeatCacheEditor : BaseToolkitEditor
    {
        #region Fields

        protected SerializedProperty m_heat;
        protected SerializedProperty m_onValueChanged;
        protected SerializedProperty m_onOverheated;
        protected SerializedProperty m_onCooled;

        #endregion

        #region Methods

        private void OnEnable()
        {
            m_heat = serializedObject.FindProperty(nameof(m_heat));
            m_onValueChanged = m_heat.FindPropertyRelative(nameof(m_onValueChanged));
            m_onOverheated = m_heat.FindPropertyRelative(nameof(m_onOverheated));
            m_onCooled = m_heat.FindPropertyRelative(nameof(m_onCooled));
        }

        protected override void DrawProperties()
        {
            EditorGUILayout.PropertyField(m_heat);
        }

        protected override void DrawEvents()
        {
            if (EditorGUILayoutUtility.Foldout(m_onValueChanged, "Events"))
            {
                EditorGUILayout.PropertyField(m_onValueChanged);
                EditorGUILayout.PropertyField(m_onOverheated);
                EditorGUILayout.PropertyField(m_onCooled);

                DrawNestedEvents();
            }
        }

        #endregion
    }
}