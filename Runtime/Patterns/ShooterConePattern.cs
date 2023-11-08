using System.Collections.Generic;
using UnityEngine;

namespace ToolkitEngine.Shooter
{
	public class ShooterConePattern : BaseMuzzleShooterPattern
    {
		#region Fields

		[SerializeField, Min(2)]
		private int m_count = 2;

		#endregion

		#region Methods

		public override Ray[] GetShotRays(BaseMuzzleShooter shooter)
		{
			List<Ray> rays = new();
			for (int i = 0; i < m_count; ++i)
			{
				rays.Add(new Ray(shooter.muzzle.position, BaseMuzzleShooter.GetShotDirection(shooter)));
			}

			return rays.ToArray();
		}

		#endregion
	}
}