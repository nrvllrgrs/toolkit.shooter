using UnityEngine;

namespace ToolkitEngine.Shooter
{
    public abstract class BaseMuzzleShooterPattern : MonoBehaviour
    {
		public abstract Ray[] GetShotRays(BaseMuzzleShooter shooter);
	}
}