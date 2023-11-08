using UnityEditor;
using ToolkitEngine.Shooter;

namespace ToolkitEditor.Shooter
{
	[CustomEditor(typeof(Projectile), true)]
    public class ProjectileEditor : BaseToolkitEditor
    {
		#region Fields

		protected Projectile m_projectile;

		protected SerializedProperty m_onFired;
		protected SerializedProperty m_onMaxDistanceReached;
		protected SerializedProperty m_onCollision;
		protected SerializedProperty m_onDetonated;

		#endregion

		#region Methods

		private void OnEnable()
		{
			m_projectile = target as Projectile;

			m_onFired = serializedObject.FindProperty(nameof(m_onFired));
			m_onMaxDistanceReached = serializedObject.FindProperty(nameof(m_onMaxDistanceReached));
			m_onCollision = serializedObject.FindProperty(nameof(m_onCollision));
			m_onDetonated = serializedObject.FindProperty(nameof(m_onDetonated));
		}

		protected override void DrawProperties()
		{
			EditorGUI.BeginDisabledGroup(true);
			EditorGUILayout.FloatField("Lifetime", m_projectile.lifetime);
			EditorGUILayout.FloatField("Distance", m_projectile.distance);
			EditorGUI.EndDisabledGroup();
		}

		protected override void DrawEvents()
		{
			if (EditorGUILayoutUtility.Foldout(m_onFired, "Events"))
			{
				EditorGUILayout.PropertyField(m_onFired);
				EditorGUILayout.PropertyField(m_onMaxDistanceReached);
				EditorGUILayout.PropertyField(m_onCollision);
				EditorGUILayout.PropertyField(m_onDetonated);
			}
		}

		#endregion
	}
}