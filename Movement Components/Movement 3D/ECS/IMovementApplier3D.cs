using UnityEngine;

namespace Clouds.Movement3D {
	public interface IMovementApplier3D {
		void Move (Vector3 moveAmount);
	}
}