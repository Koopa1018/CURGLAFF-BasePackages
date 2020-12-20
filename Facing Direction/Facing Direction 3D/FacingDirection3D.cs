using UnityEngine;
using Unity.Mathematics;
using Unity.Burst;

#if UNITY_ENTITIES
using Unity.Entities;
#endif

namespace Clouds.Facing3D
{
	[AddComponentMenu("Character/Facing Direction 3D")]
	public class FacingDirection3D : MonoBehaviour
#if UNITY_ENTITIES
	, IConvertGameObjectToEntity
#endif
	 {
		[Tooltip("The direction we should start out facing.")]
		/*[HideInInspector]*/ public float2 Value = 0;

		public float horizontal {
			get => Value.x;
			set => Value.x = value % 1;
		}
		public float vertical {
			get => Value.y;
			set => Value.y = value % 1;
		}

		[BurstCompile]
		public float3 direction () {
			//Return (0,0,1) [forward on Z] rotated around X, then around Y.
			return quaternion.AxisAngle(new float3(0,1,0), horizontal) * (
				quaternion.AxisAngle(new float3(1,0,0), vertical) * new float3(0,0,1)
			);
		}

#if UNITY_ENTITIES
		public void Convert (Entity e, EntityManager em, GameObjectConversionSystem gocs) {
			em.AddComponentData(e, new FacingDirectionComponent3D(Value));
		}
#endif

	}
}
