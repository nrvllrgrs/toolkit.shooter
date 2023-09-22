using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using ToolkitEngine.Weapons;
using UnityEngine;
using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

namespace ToolkitEngine.AI.Tasks
{
	[TaskCategory("Toolkit/Weapon")]
	[TaskIcon("35939aef136a38e42bda71cd4e5a69d9", "35939aef136a38e42bda71cd4e5a69d9")]
	public class Fire : Action
	{
		#region Fields

		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject operatorGameObject;

		protected ShooterControl m_shooterControl;
		protected GameObject m_prevGameObject;

		#endregion

		#region Methods

		public override void OnStart()
		{
			var obj = GetDefaultGameObject(operatorGameObject.Value);
			if (obj != m_prevGameObject)
			{
				m_shooterControl = obj.GetComponent<ShooterControl>();
				m_prevGameObject = obj;
			}
		}

		public override TaskStatus OnUpdate()
		{
			if (m_shooterControl != null)
			{
				m_shooterControl.Fire();
				return TaskStatus.Success;
			}
			return TaskStatus.Failure;
		}

		#endregion
	}
}