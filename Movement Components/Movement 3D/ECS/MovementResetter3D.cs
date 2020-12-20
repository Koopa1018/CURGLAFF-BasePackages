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
		[UpdateBefore(typeof(MoveProcessingGroup))]
		public class MovementResetter3D : JobComponentSystem {
			[BurstCompile]
			struct MoveResetJob : IJobForEach<Velocity3DComponent> {
				public void Execute (
					ref Velocity3DComponent movement
				) {
					movement = new Velocity3DComponent { Value = 0 };
				}

			}

			protected override JobHandle OnUpdate (JobHandle handle) {
				return new MoveResetJob().Schedule(this, handle);
			}
		}
	}
#endif