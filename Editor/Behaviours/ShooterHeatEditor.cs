using UnityEditor;
using ToolkitEngine.Weapons;
using UnityEngine;

namespace ToolkitEditor.Weapons
{
	[CustomEditor(typeof(ShooterHeat))]
    public class ShooterHeatEditor : BaseToolkitEditor
    {
		#region Fields

		protected SerializedProperty m_shooterControls;
		protected SerializedProperty m_cacheSource;
		protected SerializedProperty m_heat;
		protected SerializedProperty m_heatCache;
		protected SerializedProperty m_heatPerShot;

		protected SerializedProperty m_onValueChanged;
		protected SerializedProperty m_onOverheated;
		protected SerializedProperty m_onCooled;

		#endregion

		#region Methods

		protected virtual void OnEnable()
		{
			m_shooterControls = serializedObject.FindProperty(nameof(m_shooterControls));
			m_cacheSource = serializedObject.FindProperty(nameof(m_cacheSource));
			m_heat = serializedObject.FindProperty(nameof(m_heat));
			m_heatCache = serializedObject.FindProperty(nameof(m_heatCache));
			m_heatPerShot = serializedObject.FindProperty(nameof(m_heatPerShot));

			m_onValueChanged = serializedObject.FindProperty(nameof(m_onValueChanged));
			m_onOverheated = serializedObject.FindProperty(nameof(m_onOverheated));
			m_onCooled = serializedObject.FindProperty(nameof(m_onCooled));

			if (Application.isPlaying)
			{
				var shooterHeat = target as ShooterHeat;
				shooterHeat.onValueChanged.AddListener(ValueChanged);
			}
		}

		protected virtual void OnDisable()
		{
			if (Application.isPlaying)
			{
				var shooterHeat = target as ShooterHeat;
				shooterHeat.onValueChanged.RemoveListener(ValueChanged);
			}
		}

		protected override void DrawProperties()
		{
			EditorGUILayout.PropertyField(m_shooterControls);

			EditorGUILayout.Separator();

			EditorGUILayout.PropertyField(m_cacheSource);

			++EditorGUI.indentLevel;

			switch ((ShooterHeat.CacheSource)m_cacheSource.intValue)
			{
				case ShooterHeat.CacheSource.Internal:
					EditorGUILayout.PropertyField(m_heat);
					break;

				case ShooterHeat.CacheSource.Direct:
					EditorGUILayout.PropertyField(m_heatCache);
					break;

				case ShooterHeat.CacheSource.Parent:
					break;

				case ShooterHeat.CacheSource.Custom:
					EditorGUI.BeginDisabledGroup(true);
					EditorGUILayout.PropertyField(m_heatCache);
					EditorGUI.EndDisabledGroup();
					break;
			}
			--EditorGUI.indentLevel;

			EditorGUILayout.PropertyField (m_heatPerShot);

			EditorGUILayout.Separator();

			EditorGUI.BeginDisabledGroup(true);

			var shooterHeat = target as ShooterHeat;
			float shotsToOverheat = shooterHeat.maximum / shooterHeat.heatPerShot;
			EditorGUILayout.LabelField("Shots to Overheat", shotsToOverheat.ToString("F4"));

			if (shooterHeat.shooterControls != null)
			{
				if (shooterHeat.shooterControls.Length > 1)
				{
					EditorGUILayout.LabelField("Time to Overheat");
					++EditorGUI.indentLevel;

					foreach (var shooterControl in shooterHeat.shooterControls)
					{
						DrawTimeToOverheat(shotsToOverheat, shooterControl);
					}

					--EditorGUI.indentLevel;
				}
				else if (shooterHeat.shooterControls.Length == 1)
				{
					DrawTimeToOverheat(shotsToOverheat, shooterHeat.shooterControls[0], "Time to Overheat");
				}
			}

			EditorGUI.EndDisabledGroup();
		}

		private void DrawTimeToOverheat(float shotsToOverheat, ShooterControl shooterControl, string label = null)
		{
			float timeToOverheat = 0f;
			switch (shooterControl.fireType)
			{
				case ShooterControl.FireType.Continuous:
					timeToOverheat = shotsToOverheat;
					break;

				default:
					if (!shooterControl.isBurstFire)
					{
						timeToOverheat = shotsToOverheat * shooterControl.timeBetweenShots;
					}
					else
					{
						int burstsToOverheat = (int)shotsToOverheat / (int)shooterControl.burstShotCount;
						int overflowShots = (int)shotsToOverheat % (int)shooterControl.burstShotCount;

						timeToOverheat = burstsToOverheat * shooterControl.timeBetweenBursts;
						timeToOverheat += Mathf.Max(shooterControl.burstShotCount - 1, 0) * shooterControl.timeBetweenShots
							+ Mathf.Max(overflowShots - 1, 0) * shooterControl.timeBetweenShots;

						if (overflowShots == 0)
						{
							timeToOverheat -= shooterControl.timeBetweenBursts;
						}
					}
					break;
			}

			EditorGUILayout.LabelField(label ?? shooterControl.name, timeToOverheat.ToString("F4"));
		}

		protected override void DrawEvents()
		{
			if (EditorGUILayoutUtility.Foldout(m_onValueChanged, "Events"))
			{
				EditorGUILayout.PropertyField(m_onValueChanged);
				EditorGUILayout.PropertyField(m_onOverheated);
				EditorGUILayout.PropertyField(m_onCooled);
			}
		}

		private void ValueChanged(float value)
		{
			Repaint();
		}

		#endregion
	}
}