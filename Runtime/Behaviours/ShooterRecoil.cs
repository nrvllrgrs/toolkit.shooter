using System.Collections;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ToolkitEngine.Shooter
{
	[AddComponentMenu("Weapon/Shooter Recoil")]
	public class ShooterRecoil : MonoBehaviour, IPoolItemRecyclable
	{
		#region Fields

		[SerializeField]
		private ShooterControl[] m_shooterControls;

		[SerializeField]
		private Transform m_target;

		[SerializeField]
		private Vector3 m_pivot;

		[SerializeField, Range(0f, 90f), Tooltip("Degrees (pitch) changed per shot.")]
		private float m_recoilPerShot = 5f;

		[SerializeField]
		private bool m_variableRecoilPerShot = false;

		[SerializeField, Min(0f), Tooltip("Maximum degrees (pitch) changed per shot at maximum recoil.")]
		private float m_maxRecoilPerShot = 5f;

		[SerializeField]
		private AnimationCurve m_recoilCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		[SerializeField, Range(0f, 90f), Tooltip("Maximum degress (pitch) shooter can recoil.")]
		private float m_maxRecoil = 90f;

		[SerializeField, Min(0f), Tooltip("Seconds to wait before recovering.")]
		private float m_recoveryDelay;

		[SerializeField, Min(0f), Tooltip("Degrees (pitch) changed per second.")]
		private float m_recoveryRate = 5f;

		private Vector3 m_offsetPosition;
		private float m_totalRecoil = 0f;
		private Coroutine m_recoveryThread = null;

		#endregion

		#region Properties

		private Vector3 pivot => transform.position + transform.rotation * m_offsetPosition;

		public float recoilPerShot { get => m_recoilPerShot; set => m_recoilPerShot = value; }
		public float maxRecoil => m_maxRecoil;
		public float recoveryDelay { get => m_recoveryDelay; set => m_recoveryDelay = value; }
		public float recoveryRate { get => m_recoveryRate; set => m_recoveryRate = value; }

		public bool exceedsLimit => m_totalRecoil > m_maxRecoil;

		#endregion

		#region Methods

		public void Recycle()
		{
			if (m_totalRecoil != 0f)
			{
				m_target.RotateAround(pivot, transform.right, m_totalRecoil);
				m_totalRecoil = 0f;
			}
		}

		private void Awake()
		{
			m_shooterControls = ShooterControl.GetShooterControls(gameObject, m_shooterControls);

			if (m_target == null)
			{
				m_target = transform;
				m_offsetPosition = m_pivot;
			}
			else
			{
				m_offsetPosition = transform.InverseTransformPoint(m_target.position) + m_pivot;
			}
		}

		private void OnEnable()
		{
			foreach (var shooterControl in m_shooterControls)
			{
				shooterControl.onShotFired.AddListener(ShooterControl_ShotFired);
			}
		}

		private void OnDisable()
		{
			foreach (var shooterControl in m_shooterControls)
			{
				shooterControl.onShotFired.RemoveListener(ShooterControl_ShotFired);
			}
		}

		private void ShooterControl_ShotFired(ShooterControl shooterControl)
		{
			if (!enabled)
				return;

			float recoilPerShot = m_recoilPerShot;
			if (m_variableRecoilPerShot)
			{
				recoilPerShot = MathUtil.Remap01(
					m_recoilCurve.Evaluate(Mathf.Clamp01(m_totalRecoil / m_maxRecoil)),
					m_recoilPerShot,
					m_maxRecoilPerShot);
			}

			m_totalRecoil += shooterControl.fireType == ShooterControl.FireType.Continuous
				? recoilPerShot * Time.deltaTime
				: recoilPerShot;

			m_target.RotateAround(pivot, transform.right, -recoilPerShot);
			this.RestartCoroutine(AsyncRecovery(), ref m_recoveryThread);
		}

		private IEnumerator AsyncRecovery()
		{
			if (m_recoveryDelay > 0f)
			{
				yield return new WaitForSeconds(m_recoveryDelay);
			}

			while (m_totalRecoil > 0f)
			{
				var recoveryStep = m_recoveryRate * Time.deltaTime;
				if (recoveryStep > m_totalRecoil)
				{
					recoveryStep = m_totalRecoil;
				}

				m_target.RotateAround(pivot, transform.right, recoveryStep);
				m_totalRecoil -= recoveryStep;
				yield return null;
			}
		}

		#endregion

		#region Editor-Only
#if UNITY_EDITOR

		[ContextMenu("Update Pivot")]
		private void UpdatePivot()
		{
			m_pivot = m_target != null
				? transform.InverseTransformPoint(m_target.position)
				: Vector3.zero;
		}

		private void OnDrawGizmosSelected()
		{
			if (Application.isPlaying)
				return;

			var point = transform.position + transform.rotation * m_pivot;

			// Draw recoil
			Handles.color = Color.green;
			Handles.DrawWireArc(point, transform.right, transform.forward, -m_recoilPerShot, 0.5f);
			Handles.DrawDottedLine(point, point + transform.forward * 0.5f, 4f);
			Handles.DrawDottedLine(point, point + Quaternion.AngleAxis(-m_recoilPerShot, transform.right) * transform.forward * 0.5f, 4f);

			// Draw blocked recoil
			var from = Quaternion.AngleAxis(-m_maxRecoil, transform.right) * transform.forward;
			Gizmos.color = Handles.color = Color.red;
			Gizmos.DrawRay(point, from * 0.5f);
			Gizmos.DrawRay(point, Quaternion.AngleAxis(-m_recoilPerShot, transform.right) * from * 0.5f);
			Handles.DrawWireArc(point, transform.right, from, -m_recoilPerShot, 0.5f);
		}

#endif
		#endregion
	}
}