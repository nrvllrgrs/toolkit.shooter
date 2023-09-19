using UnityEngine;

namespace ToolkitEngine.Weapons
{
    public abstract class BaseMuzzleShooterPattern : MonoBehaviour
    {
		public abstract Ray[] GetShotRays(BaseMuzzleShooter shooter);
	}
}