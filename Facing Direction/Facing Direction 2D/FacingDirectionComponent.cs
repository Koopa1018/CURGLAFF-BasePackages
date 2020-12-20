#if UNITY_ENTITIES
using Unity.Entities;
using Unity.Mathematics;
using Unity.Burst;

namespace Clouds.Facing2D
{
	public struct FacingDirectionComponent : IComponentData {
		public int2 Value;

		public int x {
			get => Value.x;
			set => Value.x = value;
		}
		public int y {
			get => Value.y;
			set => Value.y = value;
		}

		public FacingDirectionComponent (int2 facingXY) {
			Value = facingXY;
		}
		public FacingDirectionComponent (int facingX, int facingY) {
			Value = new int2(facingX, facingY);
		}

	}
}
#endif