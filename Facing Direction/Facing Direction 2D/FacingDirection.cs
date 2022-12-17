using UnityEngine;
using Unity.Mathematics;
using Unity.Burst;

#if UNITY_ENTITIES
using Unity.Entities;
#endif

namespace Clouds.Facing2D
{
	[HelpURL("https://github.com/Koopa1018/CURGLAFF-BasePackages/wiki/Facing-Direction")]
	[AddComponentMenu("Character/Facing Direction")]
	public class FacingDirection : MonoBehaviour, IFacingDirection
#if UNITY_ENTITIES
	, IConvertGameObjectToEntity
#endif
	 {
		[Tooltip("The direction we should start out facing.")]
		[UnityEngine.Serialization.FormerlySerializedAs("Value")]
		[SerializeField] float2 _value = 0;
		public float2 Value {
			get => _value;
			set {
				//Debug.Log("Setting facing to " + value, this);
				_value = value;
			}
		}

		public float x {
			get => _value.x;
			set => _value.x = value;
		}
		public float y {
			get => _value.y;
			set => _value.y = value;
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
			return Vector2.SignedAngle(Vector2.down, normalized());
		}



		[BurstCompile]
		public float2 normalized () {
			return math.any(_value != 0)? math.normalize(_value) : 0;
		}

#if UNITY_ENTITIES
		public void Convert (Entity e, EntityManager em, GameObjectConversionSystem gocs) {
			em.AddComponentData(e, new FacingDirectionComponent(_value));
		}
#endif

	}
}
