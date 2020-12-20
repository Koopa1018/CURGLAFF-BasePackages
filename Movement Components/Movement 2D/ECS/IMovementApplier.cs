using UnityEngine;

namespace Clouds.Movement2D {
	public interface IMovementApplier {
		void Move (Vector2 moveAmount);
	}
}