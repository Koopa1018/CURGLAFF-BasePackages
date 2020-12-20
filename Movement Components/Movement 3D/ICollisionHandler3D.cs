using UnityEngine;

using Clouds.Movement3D;

using Unity.Mathematics;

namespace Clouds.Collision3D {
	public interface ICollisionHandler3D {
		/// <summary>
		/// Applies collisions to a velocity vector; returns the change between the original and collided values.
		/// </summary>
		/// <param name="velocityToHandle">This is the velocity I want to process.</param>
		/// <returns>The amount the collision changed the vector.</returns>
		float3 ApplyCollisions (ref Velocity3D velocityToHandle);

		/// <summary>
		/// Should this calculate recoil by comparing transforms instead velocity?
		/// Unless you're doing something like using a different collision system (e.g. Unity's CharacterController) and setting
		/// velocity to 0 in the apply method, just set this to <c>false</c>.
		/// </summary>
		bool useSlowRecoilComputations {get;}
	}
}