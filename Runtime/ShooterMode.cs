using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace ToolkitEngine.Weapons
{
	public class ShooterMode : MonoBehaviour
    {
		#region Fields

		[SerializeField]
		private ShooterControl[] m_shooterControls;

		private int m_index;
		private bool m_changing;

		#endregion

		#region Events

		[SerializeField]
		private UnityEvent m_onSelecting;

		[SerializeField]
		private UnityEvent m_onSelected;

		#endregion

		#region Properties

		public ShooterControl selection => m_shooterControls[m_index];

		public int count => m_shooterControls.Length;

		public int index => m_index;

		public bool changing
		{
			get => m_changing;
			private set
			{
				// No change, skip
				if (m_changing == value)
					return;

				m_changing = value;

				if (value)
				{
					m_onSelecting.Invoke();
				}
				else
				{
					m_onSelected.Invoke();
				}
			}
		}

		public UnityEvent onSelecting => m_onSelecting;
		public UnityEvent onSelected => m_onSelected;

		#endregion

		#region Methods

		private void Start()
		{
			Select(true);
		}

		[ContextMenu("Fire")]
		public void Fire()
		{
			selection.Fire();
		}

		[ContextMenu("Cancel Fire")]
		public void CancelFire()
		{
			selection.CancelFire();
		}

		[ContextMenu("Previous")]
		public void Previous()
		{
			Previous(null);
		}

		public void Previous(System.Func<bool> predicate)
		{
			Set((m_index - 1).Mod(m_shooterControls.Length), predicate);
		}

		[ContextMenu("Next")]
		public void Next()
		{
			Next(null);
		}

		public void Next(System.Func<bool> predicate)
		{
			Set((m_index + 1).Mod(m_shooterControls.Length), predicate);
		}

		public void Set(int index, System.Func<bool> predicate = null)
		{
			// Already changing, skip
			if (changing)
				return;

			StartCoroutine(AsyncSet(index, predicate));
		}

		private IEnumerator AsyncSet(int index, System.Func<bool> predicate)
		{
			changing = true;

			Select(false);

			if (predicate != null)
			{
				// Wait until predicate true
				// For example, wait until weapon done animating
				yield return new WaitUntil(predicate);
			}

			// Set index and notify change complete
			m_index = index;
			changing = false;

			Select(true);
		}

		private void Select(bool value)
		{
			if (m_index.Between(0, m_shooterControls.Length - 1))
			{
				if (value)
				{
					// Select control, if valid
					m_shooterControls[m_index].Select();
				}
				else
				{
					// Unselect control, if valid
					m_shooterControls[m_index].CancelFire();
					m_shooterControls[m_index].Unselect();
				}
			}
		}

		#endregion
	}
}