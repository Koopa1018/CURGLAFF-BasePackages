using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

using Clouds.Movement3D;

namespace Clouds.Collision3D
{
	[AddComponentMenu("Collision 3D/Character Controller Collision Handler")]
	public class CharaControllerColHandler : MonoBehaviour, ICollisionHandler3D {
		[SerializeField] LayerMask mask;
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
			// Physics.SphereCast(
			// 	transform.position, //charaCon3D.center + (Vector3.down * charaCon3D.height * 0.5f),
			// 	charaCon3D.radius,
			// 	Vector3.down,
			// 	out cast,
			// 	Mathf.Abs(velocity.y),
			// 	~0, //Layer mask,
			// 	QueryTriggerInteraction.Ignore
			// );

			float castLength = (charaCon3D.height * 0.5f) + Mathf.Abs(velocity.y);// + charaCon3D.skinWidth;
			bool didHit = Physics.Raycast(
				transform.position + charaCon3D.center, //origin
				Vector3.down, //direction
				out cast, //output
				castLength, //max distance
				/*~0*/mask, //layer mask
				QueryTriggerInteraction.Ignore
			);
			// Debug.DrawRay(
			// 	transform.position + charaCon3D.center + Vector3.right,
			// 	Vector3.down * ((charaCon3D.height * 0.5f) + Mathf.Abs(velocity.y)),
			// 	Color.yellow
			// );

			if (didHit) {
				//Debug.Log("Desired ray distance is " + castLength);
				//Debug.Log("Hit distance is " + cast.distance);
			}

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

			charaCon3D.Move(velocity.Value);
			if (hitOutput != null) {
				hitOutput.Value = charaCon3D.collisionFlags;
				//hitOutput.Value |= (charaCon3D.isGrounded ? CollisionFlags.Below : CollisionFlags.None);
			}

			velocity.Value = 0;

			//Return 0, because this return value is unneeded.
			return 0;
		}
	}
}