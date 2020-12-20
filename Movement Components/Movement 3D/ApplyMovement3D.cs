using UnityEngine;
using Unity.Mathematics;

using Clouds.Collision3D;

namespace Clouds.Movement3D {
	[DefaultExecutionOrder(20)]
	[AddComponentMenu("Movement/3D/Apply 3D Movement")]
	public class ApplyMovement3D : MonoBehaviour {
		[SerializeField] Transform myTransform;
		[SerializeField] Velocity3D velocity;
		[SerializeField] CollisionRecoil3D recoil;
		bool hasRecoil;
		[SerializeField] Space preferredTransformSpace;
		ICollisionHandler3D collisionHandler;
		bool hasCols;

		void Awake () {
			collisionHandler = GetComponent<ICollisionHandler3D>();
#if UNITY_EDITOR
			hasCols = collisionHandler != null;
			if (!hasCols) {
				Debug.Log("Movement on this object is being applied without collision handling.", this);
			}
#endif
			hasRecoil = recoil != null;
		}

#if CLOUDS_SYSTEM_MOVE3D
		void OnEnable () {
			ApplyMovementSystem3D.RegisterObject(this, myTransform, velocity, collisionHandler, recoil, preferredTransformSpace);
		}
		void OnDisable () {
			ApplyMovementSystem3D.DeregisterObject(this);
		}
#else
		void Update () {
			if (!hasCols) {
				//Apply the velocity to the position.
				transform.Translate(velocity.Value, preferredTransformSpace);
			} else if (!hasRecoil) {
				//If not has recoil, just apply collisions cleanly and without hassle.
				collisionHandler.ApplyCollisions(ref velocity);
				
				//Apply the velocity to the position.
				transform.Translate(velocity.Value, preferredTransformSpace);
			} else if (collisionHandler.useSlowRecoilComputations) {
				//If has recoil and is meant to use slow computations, do slow computations.

				//Store the original position, so we can compare its raw velocity value against the collided velocity value.
				float3 originalPosition = (float3)transform.position + velocity.Value;

				//Perform collisions on the velocity results.
				collisionHandler.ApplyCollisions(ref velocity);

				//Apply the collided velocity to the position.
				transform.Translate(velocity.Value, preferredTransformSpace);

				//Find the distance from uncollided to collided position--the delta movement from collision.
				recoil.Value = originalPosition - (float3)transform.position;
			} else {
				//If has recoil but not slow computations, just difference the velocities before and after.

				float3 velocityPreCol = velocity.Value;

				collisionHandler?.ApplyCollisions(ref velocity);

				recoil.Value = velocityPreCol - velocity.Value;

				//Apply the collided velocity to the position.
				transform.Translate(velocity.Value, preferredTransformSpace);
			}
		}
#endif
	}
}