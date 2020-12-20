using UnityEngine;
using Unity.Mathematics;

using Clouds.Collision2D;

namespace Clouds.Movement2D {
	[DefaultExecutionOrder(20)]
	[AddComponentMenu("Movement/2D/Apply 2D Movement")]
	public class ApplyMovement : MonoBehaviour {
		[SerializeField] Transform myTransform;
		[SerializeField] Velocity velocity;
		/*[SerializeReference]*/ ICollisionHandler collisionHandler;

		public static void ApplyFromVelocity(Transform transform, float2 velocity) {
			transform.position += (Vector3)new float3(velocity,0);
		}

		void Awake () {
			collisionHandler = GetComponentInChildren<ICollisionHandler>();
		}

		void FixedUpdate () {
			collisionHandler?.ApplyCollisions(ref velocity);
			ApplyFromVelocity(myTransform, velocity.Value);
		}
	}
}