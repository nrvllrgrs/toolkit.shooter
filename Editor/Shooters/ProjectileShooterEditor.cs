using ToolkitEngine.Weapons;
using UnityEngine;
using UnityEditor;

namespace ToolkitEditor.Weapons
{
	[CustomEditor(typeof(ProjectileShooter))]
	public class ProjectileShooterEditor : BaseMuzzleShooterEditor
	{
		#region Fields

		protected SerializedProperty m_projectileSpawner;
		protected SerializedProperty m_ignoredColliders;
		protected SerializedProperty m_pattern;
		protected SerializedProperty m_lifetime;
		protected SerializedProperty m_speed;
		protected SerializedProperty m_acceleration;
		protected SerializedProperty m_speedLimits;

		protected SerializedProperty m_onProjectileFired;
		protected SerializedProperty m_onMaxDistanceReached;
		protected SerializedProperty m_onCollision;
		protected SerializedProperty m_onDetonated;

		#endregion

		#region Methods

		protected override void OnEnable()
		{
			base.OnEnable();

			m_projectileSpawner = serializedObject.FindProperty(nameof(m_projectileSpawner));
			m_ignoredColliders = serializedObject.FindProperty(nameof(m_ignoredColliders));
			m_pattern = serializedObject.FindProperty(nameof(m_pattern));
			m_lifetime = serializedObject.FindProperty(nameof(m_lifetime));
			m_speed = serializedObject.FindProperty(nameof(m_speed));
			m_acceleration = serializedObject.FindProperty(nameof(m_acceleration));
			m_speedLimits = serializedObject.FindProperty(nameof(m_speedLimits));

			m_onProjectileFired = serializedObject.FindProperty(nameof(m_onProjectileFired));
			m_onMaxDistanceReached = serializedObject.FindProperty(nameof(m_onMaxDistanceReached));
			m_onCollision = serializedObject.FindProperty(nameof(m_onCollision));
			m_onDetonated = serializedObject.FindProperty(nameof(m_onDetonated));
		}

		protected override void DrawProperties()
		{
			base.DrawProperties();

			EditorGUILayout.PropertyField(m_projectileSpawner);
			EditorGUILayout.PropertyField(m_ignoredColliders);
			EditorGUILayout.PropertyField(m_pattern);
			EditorGUILayout.PropertyField(m_lifetime);
			EditorGUILayout.PropertyField(m_speed);
			EditorGUILayout.PropertyField(m_acceleration);

			if (m_acceleration.floatValue != 0f)
			{
				++EditorGUI.indentLevel;
				var speedLimitsValue = m_speedLimits.vector2Value;

				if (m_acceleration.floatValue > 0f)
				{
					speedLimitsValue.y = Mathf.Max(EditorGUILayout.FloatField("Max Speed", speedLimitsValue.y), m_speed.floatValue);
					m_speedLimits.vector2Value = speedLimitsValue;
				}
				else if (m_acceleration.floatValue < 0f)
				{
					speedLimitsValue.x = EditorGUILayout.Slider("Min Speed", speedLimitsValue.x, 0f, m_speed.floatValue);
					m_speedLimits.vector2Value = speedLimitsValue;
				}

				m_speedLimits.vector2Value = speedLimitsValue;
				--EditorGUI.indentLevel;
			}
		}

		protected override void DrawNestedEvents()
		{
			base.DrawNestedEvents();
			EditorGUILayout.LabelField("Projectile", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(m_onProjectileFired);
			EditorGUILayout.PropertyField(m_onMaxDistanceReached);
			EditorGUILayout.PropertyField(m_onCollision);
			EditorGUILayout.PropertyField(m_onDetonated);
		}

		#endregion
	}
}