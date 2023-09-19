using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using ToolkitEngine.Weapons;
using UnityEngine;
using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

namespace ToolkitEngine.AI.Tasks
{
	[TaskCategory("Toolkit/Weapon")]
	[TaskDescription("Returns success if shooter is charging; otherwise, returns failure.")]
	[TaskIcon("41fe42c1875ecf24e9619c159d856eb1", "41fe42c1875ecf24e9619c159d856eb1")]
	public class IsCharging : Conditional
	{
		#region Fields

		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject operatorGameObject;

		private ShooterCharge m_shooterCharge;
		private GameObject m_prevGameObject;

		#endregion

		#region Methods

		public override void OnStart()
		{
			var obj = GetDefaultGameObject(operatorGameObject.Value);
			if (obj != m_prevGameObject)
			{
				m_shooterCharge = obj.GetComponent<ShooterCharge>();
				m_prevGameObject = obj;
			}
		}

		public override TaskStatus OnUpdate()
		{
			return m_shooterCharge?.isCharging ?? false
				? TaskStatus.Success
				: TaskStatus.Failure;
		}

		#endregion
	}
}