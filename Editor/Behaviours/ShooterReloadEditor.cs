using UnityEngine;
using UnityEditor;
using ToolkitEngine.Shooter;

namespace ToolkitEditor.Shooter
{
	[CustomEditor(typeof(ShooterReload))]
	public class ShooterReloadEditor : BaseToolkitEditor
    {
		#region Fields

		protected SerializedProperty m_shooterControls;
		protected SerializedProperty m_shooterAmmo;
		protected SerializedProperty m_ammoCache;

		protected SerializedProperty m_autoReload;
		protected SerializedProperty m_delay;
		protected SerializedProperty m_count;
		protected SerializedProperty m_subsequentReload;
		protected SerializedProperty m_subsequentDelay;
		protected SerializedProperty m_subsequentCount;
		protected SerializedProperty m_interruptable;

		protected SerializedProperty m_onReloading;
		protected SerializedProperty m_onShotReloaded;
		protected SerializedProperty m_onReloaded;

		#endregion

		#region Methods

		protected virtual void OnEnable()
		{
			m_shooterAmmo = serializedObject.FindProperty(nameof(m_shooterAmmo));
			m_ammoCache = serializedObject.FindProperty(nameof(m_ammoCache));

			m_autoReload = serializedObject.FindProperty(nameof(m_autoReload));
			m_delay = serializedObject.FindProperty(nameof(m_delay));
			m_count = serializedObject.FindProperty(nameof(m_count));
			m_subsequentReload = serializedObject.FindProperty(nameof(m_subsequentReload));
			m_subsequentDelay = serializedObject.FindProperty(nameof(m_subsequentDelay));
			m_subsequentCount = serializedObject.FindProperty(nameof(m_subsequentCount));

			m_onReloading = serializedObject.FindProperty(nameof(m_onReloading));
			m_onShotReloaded = serializedObject.FindProperty(nameof(m_onShotReloaded));
			m_onReloaded = serializedObject.FindProperty(nameof(m_onReloaded));
		}

		protected override void DrawProperties()
		{
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(m_shooterAmmo);

			var shooterAmmo = m_shooterAmmo.objectReferenceValue as ShooterAmmo;
			int capacity = shooterAmmo?.capacity ?? 0;

			if (EditorGUI.EndChangeCheck())
			{
				m_count.intValue = capacity;
			}

			EditorGUILayout.PropertyField(m_ammoCache);

			EditorGUILayout.Separator();

			EditorGUILayout.PropertyField(m_autoReload);

			EditorGUI.BeginDisabledGroup(shooterAmmo == null);

			EditorGUILayout.PropertyField(m_delay);

			EditorGUI.BeginChangeCheck();
			EditorGUILayout.IntSlider(m_count, 1, capacity);

			int subsequentMax = Mathf.Max(capacity - m_count.intValue, 0);
			if (EditorGUI.EndChangeCheck())
			{
				m_subsequentCount.intValue = Mathf.Min(m_subsequentCount.intValue, subsequentMax);
			}

			if (m_count.intValue < capacity)
			{
				EditorGUILayout.PropertyField(m_subsequentReload);
				if (m_subsequentReload.boolValue)
				{
					++EditorGUI.indentLevel;
					EditorGUILayout.PropertyField(m_subsequentDelay, new GUIContent("Delay"));
					EditorGUILayout.IntSlider(m_subsequentCount, 1, subsequentMax, new GUIContent("Count"));
					--EditorGUI.indentLevel;
				}
			}

			EditorGUI.EndDisabledGroup();
		}

		protected override void DrawEvents()
		{
			if (EditorGUILayoutUtility.Foldout(m_onReloading, "Events"))
			{
				EditorGUILayout.PropertyField(m_onReloading);

				var shooterAmmo = m_shooterAmmo.objectReferenceValue as ShooterAmmo;
				if (m_subsequentReload.boolValue && m_count.intValue < (shooterAmmo?.capacity ?? 0))
				{
					EditorGUILayout.PropertyField(m_onShotReloaded);
				}

				EditorGUILayout.PropertyField(m_onReloaded);
			}
		}

		#endregion
	}
}