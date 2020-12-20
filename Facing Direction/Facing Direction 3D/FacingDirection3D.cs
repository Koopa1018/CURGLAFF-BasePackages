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
		[Tooltip("The direction we should start out facing on the XZ (ground) plane.")]
		/*[HideInInspector]*/ public float2 PlanarValue = 0;
		[Tooltip("The up-and-down direction we should start out facing.")]
		/*[HideInInspector]*/ public float UpDownValue = 0;

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

		[BurstCompile]
		public float3 direction () {
			float2 forwardVector = new float3(x,0,z);
			float2 sidewaysVector = new float3(-z,0,x);
			//Return forward vector rotated around sideways vector.
			return quaternion.AxisAngle(sidewaysVector, upDown) * forwardVector;
		}

		/// <summary>
		/// Calculates the angle represented by the current facing direction, up to 360* (normalized to 1).
		/// </summary>
		/// <returns>The angle represented by the current facing direction.</returns>
		[BurstCompile]
		public float angle () {
			//@TODO: Stress-test me! This function _only maybe_ works as intended.
			
			//Calculate the initial angle.
			float returner = math.acos(math.dot(new float2(0,-1), normalized())) / (2*(float)math.PI);
			//Make it cycle around all the way to 360*.
			returner = math.select(returner, 1-returner, x < 0);

			return returner;
		}

		public float signedAngle () {
			return Vector2.SignedAngle(Vector2.up, math.normalize(Value));
		}
		[BurstCompile]
		public float2 normalized () {
			return math.normalize(PlanarValue);
		}

#if UNITY_ENTITIES
		public void Convert (Entity e, EntityManager em, GameObjectConversionSystem gocs) {
			em.AddComponentData(e, new FacingDirectionComponent3D(PlanarValue));
		}
#endif

	}
}
