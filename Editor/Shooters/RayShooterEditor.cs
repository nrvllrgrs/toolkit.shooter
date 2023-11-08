using UnityEditor;
using UnityEngine;
using ToolkitEngine.Shooter;

namespace ToolkitEditor.Shooter
{
	[CustomEditor(typeof(RayShooter))]
    public class RayShooterEditor : BaseMuzzleShooterEditor
    {
		#region Fields

		protected SerializedProperty m_radius;
		protected SerializedProperty m_pattern;
		protected SerializedProperty m_layerMask;
		protected SerializedProperty m_penetrateCount;
		protected SerializedProperty m_blockingMask;
		protected SerializedProperty m_impactDamage;
		protected SerializedProperty m_splashDamage;
		protected SerializedProperty m_onPenetrated;

		#endregion

		#region Methods

		protected override void OnEnable()
		{
			base.OnEnable();

			m_radius = serializedObject.FindProperty(nameof(m_radius));
			m_pattern = serializedObject.FindProperty(nameof(m_pattern));
			m_layerMask = serializedObject.FindProperty(nameof(m_layerMask));
			m_penetrateCount = serializedObject.FindProperty(nameof(m_penetrateCount));
			m_blockingMask = serializedObject.FindProperty(nameof(m_blockingMask));
			m_impactDamage = serializedObject.FindProperty(nameof(m_impactDamage));
			m_splashDamage = serializedObject.FindProperty(nameof(m_splashDamage));
			m_onPenetrated = serializedObject.FindProperty(nameof(m_onPenetrated));
		}

		protected override void DrawProperties()
		{
			base.DrawProperties();

			EditorGUILayout.PropertyField(m_radius);
			EditorGUILayout.PropertyField(m_pattern);
			EditorGUILayout.PropertyField(m_layerMask);

			if (m_penetrateCount.intValue >= 0)
			{
				EditorGUILayout.PropertyField(m_penetrateCount);
			}
			else
			{
				EditorGUI.BeginChangeCheck();
				float value = EditorGUILayout.FloatField(m_penetrateCount.displayName, float.PositiveInfinity);

				if (EditorGUI.EndChangeCheck())
				{
					m_penetrateCount.intValue = Mathf.FloorToInt(value);
				}
			}

			if (m_penetrateCount.intValue != 0)
			{
				++EditorGUI.indentLevel;
				EditorGUILayout.PropertyField(m_blockingMask);
				--EditorGUI.indentLevel;
			}

			EditorGUILayout.PropertyField(m_impactDamage);
			EditorGUILayout.PropertyField(m_splashDamage);
		}

		protected override void DrawNestedEvents()
		{
			base.DrawNestedEvents();

			if (m_penetrateCount.intValue != 0)
			{
				EditorGUILayout.PropertyField(m_onPenetrated);
			}
		}

		#endregion
	}
}