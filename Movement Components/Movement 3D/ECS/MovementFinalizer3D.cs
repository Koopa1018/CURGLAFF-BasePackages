#if UNITY_ENTITIES
	using Unity.Burst;
	using Unity.Collections;
	using Unity.Entities;
	using Unity.Jobs;
	using Unity.Mathematics;
	using Unity.Transforms;

	using Clouds.MovementBase;

	namespace Clouds.Movement3D
	{
		[UpdateAfter(typeof(CollisionsGroup))]
		public class MovementFinalizer3D : JobComponentSystem {
			[BurstCompile]
			struct MoveFinalizeJob : IJobForEach<Velocity3DComponent, Translation> {
				public void Execute (
					[ReadOnly] ref Velocity3DComponent movement, 
					ref Translation position
				) {
					position = new Translation {
						Value = position.Value + movement.Value
					};
				}

			}


			protected override JobHandle OnUpdate (JobHandle handle) {
				return new MoveFinalizeJob().Schedule(this,handle);
			}
		}
	}
#endif