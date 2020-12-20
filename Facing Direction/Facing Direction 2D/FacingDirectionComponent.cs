#if UNITY_ENTITIES
using Unity.Entities;
using Unity.Mathematics;
using Unity.Burst;

namespace Clouds.Facing2D
{
	public struct FacingDirectionComponent : IComponentData {
		public float2 Value;

		public float x {
			get => Value.x;
			set => Value.x = value;
		}
		public float y {
			get => Value.y;
			set => Value.y = value;
		}

		public FacingDirectionComponent (float2 facingXY) {
			Value = facingXY;
		}
		public FacingDirectionComponent (float facingX, float facingY) {
			Value = new float2(facingX, facingY);
		}

	}
}
#endif