using Unity.Entities;

namespace Clouds.MovementBase
{
	/// <summary>
	/// The Input Getter group generates input for the Move Processing group. (Player input goes here.)
	/// </summary>
	[UpdateBefore(typeof(MoveProcessingGroup))]
	public sealed class InputGetterGroup : ComponentSystemGroup { }
	
	[UpdateAfter(typeof(InputGetterGroup))]
	[UpdateBefore(typeof(MoveProcessingGroup))]
	//this to make sure that MonoBehaviour can use player input components if needed.
	//[UpdateBefore(typeof(UnityEngine.Experimental.PlayerLoop.Update))]
	public sealed class InputGetterBarrier : EntityCommandBufferSystem {}

	/// <summary>
	/// The Move Processing group converts input from Input Getter group into movement for Collisions group to process.
	/// </summary>
	[UpdateAfter(typeof(InputGetterGroup))]
	[UpdateBefore(typeof(CollisionsGroup))]
	public class MoveProcessingGroup : ComponentSystemGroup {}

	[UpdateAfter(typeof(MoveProcessingGroup))]
	[UpdateBefore(typeof(CollisionsGroup))]
	public sealed class MoveProcessingBarrier : EntityCommandBufferSystem {}

	/// <summary>
	/// The Collisions group applies the movement computed in the Move Processing group.
	/// </summary>
	[UpdateAfter(typeof(InputGetterGroup))]
	[UpdateAfter(typeof(MoveProcessingGroup))]
	[UpdateBefore(typeof(Unity.Transforms.TransformSystemGroup))]
	public sealed class CollisionsGroup : ComponentSystemGroup {}
	
}