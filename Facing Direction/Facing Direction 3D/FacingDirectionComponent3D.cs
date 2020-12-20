#if UNITY_ENTITIES
using Unity.Entities;
using Unity.Mathematics;
using Unity.Burst;

namespace Clouds.Facing2D
{
	public struct FacingDirectionComponent3D : IComponentData {
		public float2 PlanarValue;
		public float UpDownValue;

		/// <summary>
		/// The X component of the planar facing direction.
		/// </summary>
		public float x {
			get => PlanarValue.x;
			set => PlanarValue.x = math.clamp(value, -1, 1);
		}
		/// <summary>
		/// The Z component of the planar facing direction.
		/// </summary>
		public float z {
			get => PlanarValue.y;
			set => PlanarValue.y = math.clamp(value, -1, 1);
		}

		/// <summary>
		/// The up/down facing direction stored as an angle between -1 (down) and 1 (up).
		/// </summary>
		public float upDown {
			get => UpDownValue;
			set => UpDownValue = math.clamp(value, -1, 1);
		}

		public FacingDirectionComponent (float2 facingXZ, float facingUD = 0) {
			PlanarValue = math.clamp(facingXZ, -1, 1);
			UpDownValue = math.clamp(facingUD, -1, 1);
		}

	}
}
#endif