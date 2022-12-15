using UnityEngine;
using UnityEngine.Events;
using Unity.Mathematics;

namespace Clouds.Facing2D 
{
	public class YRotationFromFacing : MonoBehaviour {
		[SerializeField] FacingDirection facingDirection;
		[Tooltip("The angle I'll rotate to when the facing direction is pointed at (0, -1).")]
		[SerializeField] [Range(-180, 180)] float rotationEqualingDown = 0;
		[SerializeField] bool invert = false;
		[SerializeField] bool tickInFixedUpdate;
		
		void Update() {
			if (!tickInFixedUpdate) {
				tickAngle();
			}
		}

		void FixedUpdate() {
			if (tickInFixedUpdate) {
				tickAngle();
			}
		}
		

		void tickAngle() {
			//If facing is all zeroes, abort.
			if (math.lengthsq(facingDirection.Value) == 0) {
				return;
			}

			float3 rotation = transform.eulerAngles;
			rotation.y = facingDirection.signedAngle() + rotationEqualingDown;
			rotation.y *= invert? -1 : 1;
			transform.eulerAngles = rotation;
		}
	}
}