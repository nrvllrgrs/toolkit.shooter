using UnityEngine;
using UnityEditor;
using ToolkitEngine.Weapons;

namespace ToolkitEditor.Weapons
{
	[CustomEditor(typeof(ShooterControl))]
	public class ShooterControlEditor : Editor
	{
		#region Fields

		protected ShooterControl m_shooterControl;

		protected SerializedProperty m_shooters;
		protected SerializedProperty m_firingBlockers;
		protected SerializedProperty m_shotFiredBlockers;

		// Controls
		protected SerializedProperty m_fireType;
		protected SerializedProperty m_timeBetweenShots;
		protected SerializedProperty m_fireOnCancel;

		// Burst Fire
		protected SerializedProperty m_isBurstFire;
		protected SerializedProperty m_timeBetweenBursts;
		protected SerializedProperty m_shotsPerBurst;

		// Events
		protected SerializedProperty m_onBeginFire;
		protected SerializedProperty m_onShotFiring;
		protected SerializedProperty m_onShotFired;
		protected SerializedProperty m_onShotBlocked;
		protected SerializedProperty m_onEndFire;
		protected SerializedProperty m_onSelected;
		protected SerializedProperty m_onUnselected;

		private bool m_showBlockers = false;

		#endregion

		#region Methods

		private void OnEnable()
		{
			m_shooterControl = (ShooterControl)target;

			// Shooters
			m_shooters = serializedObject.FindProperty("m_shooters");
			m_firingBlockers = serializedObject.FindProperty(nameof(m_firingBlockers));
			m_shotFiredBlockers = serializedObject.FindProperty(nameof(m_shotFiredBlockers));

			// Controls
			m_fireType = serializedObject.FindProperty(nameof(m_fireType));
			m_timeBetweenShots = serializedObject.FindProperty(nameof(m_timeBetweenShots));
			m_fireOnCancel = serializedObject.FindProperty(nameof(m_fireOnCancel));

			// Burst Fire
			m_isBurstFire = serializedObject.FindProperty(nameof(m_isBurstFire));
			m_timeBetweenBursts = serializedObject.FindProperty(nameof(m_timeBetweenBursts));
			m_shotsPerBurst = serializedObject.FindProperty(nameof(m_shotsPerBurst));

			// Events
			m_onBeginFire = serializedObject.FindProperty(nameof(m_onBeginFire));
			m_onShotFiring = serializedObject.FindProperty(nameof(m_onShotFiring));
			m_onShotFired = serializedObject.FindProperty(nameof(m_onShotFired));
			m_onShotBlocked = serializedObject.FindProperty(nameof(m_onShotBlocked));
			m_onEndFire = serializedObject.FindProperty(nameof(m_onEndFire));
			m_onSelected = serializedObject.FindProperty(nameof(m_onSelected));
			m_onUnselected = serializedObject.FindProperty(nameof(m_onUnselected));

		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			m_shooterControl.shooters = m_shooterControl.GetComponents<BaseShooter>();

			// Read-only data
			EditorGUI.BeginDisabledGroup(true);
			EditorGUILayout.PropertyField(m_shooters);
			EditorGUI.EndDisabledGroup();

			m_showBlockers = EditorGUILayout.Foldout(m_showBlockers, "Blockers");
			if (m_showBlockers)
			{
				++EditorGUI.indentLevel;
				EditorGUILayout.PropertyField(m_firingBlockers, new GUIContent("Firing"));
				EditorGUILayout.PropertyField(m_shotFiredBlockers, new GUIContent("Shot Fired"));
				--EditorGUI.indentLevel;
			}

			EditorGUILayout.Separator();
			DrawControlsGUI();

			if (m_fireType.intValue != (int)ShooterControl.FireType.Continuous)
			{
				EditorGUILayout.Separator();
				DrawBurstGUI();
			}

			EditorGUILayout.Separator();

			if (EditorGUILayoutUtility.Foldout(m_fireType, "Commands"))
			{
				EditorGUI.BeginDisabledGroup(!Application.isPlaying);

				switch ((ShooterControl.FireType)m_fireType.intValue)
				{
					case ShooterControl.FireType.SemiAuto:
						if (!m_shooterControl.firing)
						{
							if (GUILayout.Button("Fire"))
							{
								m_shooterControl.Fire();
								if (!m_fireOnCancel.boolValue)
								{
									m_shooterControl.CancelFire();
								}
							}
						}
						else
						{
							if (GUILayout.Button("Cancel Fire"))
							{
								m_shooterControl.CancelFire();
							}
						}
						break;

					case ShooterControl.FireType.FullAuto:
					case ShooterControl.FireType.Continuous:
						if (!m_shooterControl.firing)
						{
							if (GUILayout.Button("Fire"))
							{
								m_shooterControl.Fire();
							}
						}
						else
						{
							if (GUILayout.Button("Cancel Fire"))
							{
								m_shooterControl.CancelFire();
							}
						}
						break;
				}

				EditorGUI.EndDisabledGroup();
			}

			DrawEventsGUI();

			serializedObject.ApplyModifiedProperties();
		}

		protected virtual void DrawControlsGUI()
		{
			EditorGUILayout.PropertyField(m_fireType);

			++EditorGUI.indentLevel;
			switch ((ShooterControl.FireType)m_fireType.intValue)
			{
				case ShooterControl.FireType.Continuous:
					// Intentionally left blank
					break;

				case ShooterControl.FireType.FullAuto:
					EditorGUILayout.PropertyField(m_timeBetweenShots);
					break;

				case ShooterControl.FireType.SemiAuto:
					EditorGUILayout.PropertyField(m_timeBetweenShots);
					EditorGUILayout.PropertyField(m_fireOnCancel);
					break;
			}
			--EditorGUI.indentLevel;
		}

		protected virtual void DrawBurstGUI()
		{
			EditorGUILayout.PropertyField(m_isBurstFire, new GUIContent("Burst Fire"));
			if (m_isBurstFire.boolValue)
			{
				++EditorGUI.indentLevel;
				EditorGUILayout.PropertyField(m_timeBetweenBursts);
				EditorGUILayout.PropertyField(m_shotsPerBurst);
				--EditorGUI.indentLevel;
			}
		}

		protected virtual void DrawEventsGUI()
		{
			if (EditorGUILayoutUtility.Foldout(m_onBeginFire, "Events"))
			{
				EditorGUILayout.PropertyField(m_onBeginFire);
				EditorGUILayout.PropertyField(m_onShotFiring);
				EditorGUILayout.PropertyField(m_onShotFired);
				EditorGUILayout.PropertyField(m_onShotBlocked);
				EditorGUILayout.PropertyField(m_onEndFire);

				EditorGUILayout.LabelField("Mode", EditorStyles.boldLabel);
				EditorGUILayout.PropertyField(m_onSelected);
				EditorGUILayout.PropertyField(m_onUnselected);
			}
		}

		#endregion
	}
}