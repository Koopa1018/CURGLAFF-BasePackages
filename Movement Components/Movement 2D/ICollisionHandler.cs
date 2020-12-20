using UnityEngine;

using Clouds.Movement2D;

using Unity.Mathematics;

namespace Clouds.Collision2D {
	public interface ICollisionHandler {
		void ApplyCollisions (ref Velocity velocityToHandle);
	}
}