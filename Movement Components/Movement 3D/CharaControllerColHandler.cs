using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

using Clouds.Movement3D;

namespace Clouds.Collision3D
{
	[AddComponentMenu("Collision 3D/Character Controller Collision Handler")]
	public class CharaControllerColHandler : MonoBehaviour, ICollisionHandler3D {
		[SerializeField] CharacterController charaCon3D;
		[SerializeField] CollisionData3D hitOutput;
		//[SerializeField] [Min(0)] float gradualDescendSpeed = 1;
		[SerializeField] bool stickToGround = true;

		/// <summary>
		/// Should I pull the recoil calculations off the transforms instead? Yes, so this can blank velocity and still output recoil.
		/// </summary>
		public bool useSlowRecoilComputations => true;

		public float3 ApplyCollisions (ref Velocity3D velocity) {
			float3 oldPos = transform.position;

			RaycastHit cast;
				
			//Do down cast.
			Physics.SphereCast(
				charaCon3D.center + (Vector3.down * charaCon3D.height * 0.5f),
				charaCon3D.radius,
				Vector3.down,
				out cast,
				float.PositiveInfinity,
				~0, //Layer mask,
				QueryTriggerInteraction.Ignore
			);

			if (stickToGround) {
				if (cast.collider == null) {
					//Do up cast.
					Physics.SphereCast(
						charaCon3D.center + (Vector3.up * charaCon3D.height * 0.5f),
						charaCon3D.radius,
						Vector3.up,
						out cast,
						float.PositiveInfinity,
						~0, //Layer mask,
						QueryTriggerInteraction.Ignore
					);
				}

				if (cast.collider != null) {
					Vector3 movement = cast.point - transform.position;
					velocity.Value += (float3)movement;
				}

				velocity.Value += new float3(0, -1, 0);
			}

			CollisionFlags hits = charaCon3D.Move(velocity.Value);
			hits |= cast.collider != null ? CollisionFlags.Below : CollisionFlags.None;
			if (hitOutput != null) {
				hitOutput.Value = hits;
			}

			//Calculate the collision-recoil value.
			float3 returner = (oldPos + velocity.Value) - (float3)(transform.position);

			velocity.Value = 0;

			//Return the collision recoil.
			return returner;
		}
	}
}