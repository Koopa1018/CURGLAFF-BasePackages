using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

using Clouds.MovementBase;

namespace Clouds.Movement2D
{
	[UpdateBefore(typeof(MoveProcessingGroup))]
	public class MovementResetter2D : JobComponentSystem {
		[BurstCompile]
		struct MoveResetJob : IJobForEach<Movement2DComponent> {
			public void Execute (
				ref Movement2DComponent movement
			) {
				movement = new Movement2DComponent { Value = 0 };
			}

		}

		protected override JobHandle OnUpdate (JobHandle handle) {
			return new MoveResetJob().Schedule(this, handle);
		}
	}
}