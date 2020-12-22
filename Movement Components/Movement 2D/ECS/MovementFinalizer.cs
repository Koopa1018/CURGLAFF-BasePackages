using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

using Clouds.MovementBase;

namespace Clouds.Movement2D
{
	[UpdateAfter(typeof(CollisionsGroup))]
	public class MovementFinalizer : JobComponentSystem {
		[BurstCompile]
		struct MoveFinalizeJob : IJobForEach<Movement2DComponent, Translation> {
			public void Execute (
				[ReadOnly] ref Movement2DComponent movement, 
				ref Translation position
			) {
				position = new Translation {
					Value = position.Value + new float3(movement.Value, 0)
				};
			}

		}

		protected override JobHandle OnUpdate (JobHandle handle) {
			return new MoveFinalizeJob().Schedule(this,handle);
		}
	}
}