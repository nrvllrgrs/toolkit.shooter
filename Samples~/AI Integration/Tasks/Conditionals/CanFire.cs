using BehaviorDesigner.Runtime.Tasks;
using ToolkitEngine.Weapons;

namespace ToolkitEngine.AI.Tasks
{
	[TaskCategory("Toolkit/Weapon")]
	[TaskDescription("Returns success if shooter can fire; otherwise, returns failure.")]
	[TaskIcon("35939aef136a38e42bda71cd4e5a69d9", "35939aef136a38e42bda71cd4e5a69d9")]
	public class CanFire : ToolkitConditional<ShooterControl>
	{
		#region Methods

		public override TaskStatus OnUpdate()
		{
			return m_component?.canFireByTime ?? false
				? TaskStatus.Success
				: TaskStatus.Failure;
		}

		#endregion
	}
}