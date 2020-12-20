#if UNITY_ENTITIES
using Unity.Entities;
using Unity.Mathematics;
using Unity.Burst;

namespace Clouds.Facing2D
{
	public struct FacingDirectionComponent3D : IComponentData {
		public float2 Value;

		public float h {
			get => Value.x;
			set => Value.x = value % 1;
		}
		public float v {
			get => Value.y;
			set => Value.y = value % 1;
		}

		public FacingDirectionComponent (float2 facingHV) {
			Value = facingHV % 1;
		}
		public FacingDirectionComponent (float facingLR, float facingUD) {
			Value = new float2(facingLR, facingUD) % 1;
		}

	}
}
#endif