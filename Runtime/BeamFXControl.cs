using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToolkitEngine.Weapons
{
	public class BeamFXControl : MonoBehaviour
	{
		#region Fields

		[SerializeField]
		private BaseShooter[] m_shooters;

		[SerializeField]
		private Spawner m_spawner;

		private Dictionary<Projectile, LineRenderer> m_ribbonMap = new();
		private Coroutine m_ribbonsThread = null;

		#endregion

		#region Methods

		private void Awake()
		{
			if (m_shooters.Length == 0)
			{
				m_shooters = GetComponents<BaseShooter>();
			}
		}

		private void OnEnable()
		{
			foreach (var shooter in m_shooters)
			{
				if (shooter is ProjectileShooter projectileShooter)
				{
					projectileShooter.onProjectileFired.AddListener(Shooter_ProjectileFired);
					projectileShooter.onDetonated.AddListener(Shooter_Detonated);
				}
				else
				{
					shooter.onFired.AddListener(Shooter_Fired);
				}
			}
		}

		private void OnDisable()
		{
			foreach (var shooter in m_shooters)
			{
				if (shooter is ProjectileShooter projectileShooter)
				{
					projectileShooter.onProjectileFired.RemoveListener(Shooter_ProjectileFired);
					projectileShooter.onDetonated.RemoveListener(Shooter_Detonated);
				}
				else
				{
					shooter.onFired.RemoveListener(Shooter_Fired);
				}
			}
		}

		private void Shooter_Fired(ShooterEventArgs e)
		{
			foreach (var hit in e.hits)
			{
				m_spawner.Instantiate(hit.contact, Quaternion.identity, Spawned, e.origin, hit.contact);
			}
		}

		private void Spawned(GameObject spawnedObject, params object[] args)
		{
			var lineRenderer = spawnedObject.GetComponent<LineRenderer>();
			if (lineRenderer == null)
				return;

			lineRenderer.SetPositions(new[] { (Vector3)args[0], (Vector3)args[1] });
		}

		private void Shooter_ProjectileFired(ProjectileEventArgs e)
		{
			if (!m_ribbonMap.ContainsKey(e.projectile))
			{
				m_spawner.Instantiate(e.projectileShooter.muzzle.position, Quaternion.identity, SpawnedProjectile, e.projectile);
			}
		}

		private void SpawnedProjectile(GameObject spawnedObject, params object[] args)
		{
			var lineRenderer = spawnedObject.GetComponent<LineRenderer>();
			if (lineRenderer == null)
				return;

			m_ribbonMap.Add((Projectile)args[0], lineRenderer);

			if (m_ribbonsThread == null)
			{
				m_ribbonsThread = StartCoroutine(AsyncUpdateRibbons());
			}
		}

		private void Shooter_Detonated(ProjectileEventArgs e)
		{
			if (m_ribbonMap.TryGetValue(e.projectile, out var ribbon))
			{
				m_ribbonMap.Remove(e.projectile);
				PoolItem.Destroy(ribbon.gameObject);
			}
		}

		private IEnumerator AsyncUpdateRibbons()
		{
			while (m_ribbonMap.Count > 0)
			{
				foreach (var p in m_ribbonMap)
				{
					p.Value.SetPositions(new[]
					{
						p.Key.projectileShooter.muzzle.position,
						p.Key.transform.position
					});
				}
				yield return null;
			}
			m_ribbonsThread = null;
		}

		#endregion
	}
}