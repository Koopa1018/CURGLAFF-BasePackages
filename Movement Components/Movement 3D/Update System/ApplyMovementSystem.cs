using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.LowLevel;
using Unity.Mathematics;

using Clouds.Movement3D;
using Clouds.Collision3D;
using Clouds.CustomPlayerLoop;

namespace Clouds.Movement3D {
	public static class ApplyMovementSystem3D {
		struct MovingObject3D {
			public Transform transform;
			public Velocity3D velocity;
			public ICollisionHandler3D cols;
			public CollisionRecoil3D recoil;
			public Space coordSpace;
		}
		static Dictionary<ApplyMovement3D, MovingObject3D> movingObjects;

		//To avoid GC alloc:
#region AvoidGCAlloc
		static int i;
		static float3 originalPosition;
		static Velocity3D velocityReference;
#endregion

		static void Initialize () {
			movingObjects = new Dictionary<ApplyMovement3D, MovingObject3D>();
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		static void RegisterSystem () {
			PlayerLoopSystem applyMoveSystem = new PlayerLoopSystem();
			applyMoveSystem.updateDelegate = ApplyMovement3D;

			PlayerLoopSystem clearVeloSystem = new PlayerLoopSystem();
			clearVeloSystem.updateDelegate = ClearVelocity3D;


			PlayerLoopExtend.AddToFixedUpdate(clearVeloSystem, applyMoveSystem);

			//Initialize everything too.
			Initialize();
		}


		public static void RegisterObject (ApplyMovement3D sourceObject, Transform transform, Velocity3D velocity, ICollisionHandler3D cols, CollisionRecoil3D recoil, Space space = Space.World) {
			//If the object is already registered, no good to register it again.
			if (movingObjects.ContainsKey(sourceObject) ) {
				return;
			}
			//#Dictionary's ContainsKey value is "close to O(1)", as it's a hashtable inside.

			movingObjects.Add(
				sourceObject,
				new MovingObject3D{
					transform = transform,
					velocity = velocity,
					cols = cols,
					recoil = recoil,
					coordSpace = space
				}
			);
		}
		public static void DeregisterObject (ApplyMovement3D sourceObject) {
			movingObjects.Remove(sourceObject);
		}

		static void ApplyMovement3D () {
			//Regrettably, I can't make this less branchingous, lest runtime problems occur on, say, delete collision recoil :\
			foreach (MovingObject3D movingObject in movingObjects.Values) {
				velocityReference = movingObject.velocity;
				if (movingObject.cols == null) {
					//Apply the velocity to the position.
					movingObject.transform.Translate((Vector3)movingObject.velocity.Value, movingObject.coordSpace);
				} else if (movingObject.recoil == null) {
					//If not has recoil, just apply collisions cleanly and without hassle.
					movingObject.cols.ApplyCollisions(ref velocityReference);
					
					//Apply the velocity to the position.
					movingObject.transform.Translate((Vector3)movingObject.velocity.Value, movingObject.coordSpace);
				} else if (movingObject.cols.useSlowRecoilComputations) {
					//If has recoil and is meant to use slow computations, do slow computations.

					//Store the original position, so we can compare its raw velocity value against the collided velocity value.
					float3 originalPosition = (float3)movingObject.transform.position + velocityReference.Value;

					//Perform collisions on the velocity results.
					movingObject.cols.ApplyCollisions(ref velocityReference);

					//Apply the collided velocity to the position.
					movingObject.transform.Translate((Vector3)movingObject.velocity.Value, movingObject.coordSpace);

					//Find the distance from uncollided to collided position--the delta movement from collision.
					movingObject.recoil.Value = originalPosition - (float3)movingObject.transform.position;
				} else {
					//If has recoil but not slow computations, just difference the velocities before and after.

					float3 velocityPreCol = velocityReference.Value;

					movingObject.cols?.ApplyCollisions(ref velocityReference);

					movingObject.recoil.Value = velocityPreCol - velocityReference.Value;

					//Apply the collided velocity to the position.
					movingObject.transform.Translate((Vector3)movingObject.velocity.Value, movingObject.coordSpace);
				}
			}
		}

		static void ClearVelocity3D () {
			foreach (MovingObject3D movingObject in movingObjects.Values) {
				movingObject.velocity.Value = 0;
			}
		}

	}
}
			