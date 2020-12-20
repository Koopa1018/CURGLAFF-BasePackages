using UnityEngine;
using Unity.Mathematics;
using Unity.Burst;

#if UNITY_ENTITIES
using Unity.Entities;
#endif

namespace Clouds.Facing2D
{
	[AddComponentMenu("Character/Facing Direction")]
	public class FacingDirection : MonoBehaviour, IFacingDirection
#if UNITY_ENTITIES
	, IConvertGameObjectToEntity
#endif
	 {
		[Tooltip("The direction we should start out facing.")]
		/*[HideInInspector]*/ public int2 Value = 0;

		public int x {
			get => Value.x;
			set => Value.x = value;
		}
		public int y {
			get => Value.y;
			set => Value.y = value;
		}

		/// <summary>
		/// Calculates the angle represented by the current facing direction, up to 360* (normalized to 1).
		/// </summary>
		/// <returns>The angle represented by the current facing direction.</returns>
		[BurstCompile]
		public float angle () {
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
			return math.normalize(Value);
		}

#if UNITY_ENTITIES
		public void Convert (Entity e, EntityManager em, GameObjectConversionSystem gocs) {
			em.AddComponentData(e, new FacingDirectionComponent(Value));
		}
#endif

	}
}
