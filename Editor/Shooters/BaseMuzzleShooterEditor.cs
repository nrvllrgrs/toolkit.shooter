using UnityEditor;
using ToolkitEngine.Shooter;

namespace ToolkitEditor.Shooter
{
	[CustomEditor(typeof(BaseMuzzleShooter), true)]
    public class BaseMuzzleShooterEditor : BaseShooterEditor
    {
		#region Fields

		protected SerializedProperty m_muzzle;
		protected SerializedProperty m_spread;
		protected SerializedProperty m_distribution;
		protected SerializedProperty m_mean;
		protected SerializedProperty m_deviation;

		protected SerializedProperty m_onSpreadChanged;

		#endregion

		#region Methods

		protected override void OnEnable()
		{
			base.OnEnable();

			m_muzzle = serializedObject.FindProperty(nameof(m_muzzle));
			m_spread = serializedObject.FindProperty(nameof(m_spread));
			m_distribution = serializedObject.FindProperty(nameof(m_distribution));
			m_mean = serializedObject.FindProperty(nameof(m_mean));
			m_deviation = serializedObject.FindProperty(nameof(m_deviation));

			m_onSpreadChanged = serializedObject.FindProperty(nameof(m_onSpreadChanged));
		}

		protected override void DrawProperties()
		{
			base.DrawProperties();

			EditorGUILayout.PropertyField(m_muzzle);
			EditorGUILayout.PropertyField(m_spread);
			EditorGUILayout.PropertyField(m_distribution);

			++EditorGUI.indentLevel;
			switch ((BaseMuzzleShooter.SpreadDistribution)m_distribution.intValue)
			{
				case BaseMuzzleShooter.SpreadDistribution.Gaussian:
					EditorGUILayout.PropertyField(m_mean);
					EditorGUILayout.PropertyField(m_deviation);
					break;
			}
			--EditorGUI.indentLevel;
		}

		protected override void DrawNestedEvents()
		{
			base.DrawNestedEvents();
			EditorGUILayout.PropertyField(m_onSpreadChanged);
		}

		#endregion
	}
}