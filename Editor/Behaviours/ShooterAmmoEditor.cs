using UnityEditor;
using ToolkitEngine.Shooter;

namespace ToolkitEditor.Shooter
{
	[CustomEditor(typeof(ShooterAmmo))]
	public class ShooterAmmoEditor : BaseToolkitEditor
    {
		#region Fields

		protected SerializedProperty m_shooterControls;
		protected SerializedProperty m_cacheSource;
		protected SerializedProperty m_ammo;
		protected SerializedProperty m_ammoCache;
		protected SerializedProperty m_ammoType;
		protected SerializedProperty m_consumedPerShot;

		protected SerializedProperty m_onCountChanged;
		protected SerializedProperty m_onEmpty;
		protected SerializedProperty m_onDryFiring;

		#endregion

		#region Methods

		protected virtual void OnEnable()
		{
			m_shooterControls = serializedObject.FindProperty(nameof(m_shooterControls));
			m_cacheSource = serializedObject.FindProperty(nameof(m_cacheSource));
			m_ammo = serializedObject.FindProperty(nameof(m_ammo));
			m_ammoCache = serializedObject.FindProperty(nameof(m_ammoCache));
			m_ammoType = serializedObject.FindProperty(nameof(m_ammoType));
			m_consumedPerShot = serializedObject.FindProperty(nameof(m_consumedPerShot));

			m_onCountChanged = serializedObject.FindProperty(nameof(m_onCountChanged));
			m_onEmpty = serializedObject.FindProperty(nameof(m_onEmpty));
			m_onDryFiring = serializedObject.FindProperty(nameof(m_onDryFiring));
		}

		protected override void DrawProperties()
		{
			EditorGUILayout.PropertyField(m_shooterControls);

			EditorGUILayout.Separator();

			EditorGUILayout.PropertyField(m_cacheSource);

			++EditorGUI.indentLevel;
			switch ((ShooterAmmo.CacheSource)m_cacheSource.intValue)
			{
				case ShooterAmmo.CacheSource.Internal:
					EditorGUILayout.PropertyField(m_ammo);
					break;

				case ShooterAmmo.CacheSource.Direct:
					EditorGUILayout.PropertyField(m_ammoCache);
					break;

				case ShooterAmmo.CacheSource.Parent:
					EditorGUILayout.PropertyField(m_ammoType);
					break;

				case ShooterAmmo.CacheSource.Custom:
					EditorGUI.BeginDisabledGroup(true);
					EditorGUILayout.PropertyField(m_ammoCache);
					EditorGUI.EndDisabledGroup();

					EditorGUILayout.PropertyField(m_ammoType);
					break;
			}
			--EditorGUI.indentLevel;

			EditorGUILayout.PropertyField(m_consumedPerShot);
		}

		protected override void DrawEvents()
		{
			if (EditorGUILayoutUtility.Foldout(m_onCountChanged, "Events"))
			{
				EditorGUILayout.PropertyField(m_onCountChanged);
				EditorGUILayout.PropertyField(m_onEmpty);
				EditorGUILayout.PropertyField(m_onDryFiring);

				DrawNestedEvents();
			}
		}

		#endregion
	}
}