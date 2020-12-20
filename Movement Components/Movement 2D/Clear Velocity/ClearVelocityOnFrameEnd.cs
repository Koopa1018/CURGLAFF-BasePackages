using UnityEngine;
using Unity.Mathematics;

namespace Clouds.Movement2D {
	[DefaultExecutionOrder(20)]
	[AddComponentMenu("Movement/2D/Clear Velocity 2D on Frame End")]
	public class ClearVelocityOnFrameEnd : MonoBehaviour, IClearVelocity2D {
		[SerializeField] Velocity velocity;
#if UNITY_EDITOR
		public Velocity EDI_velocityReference {get => velocity; set => velocity = value;}
#endif

		void LateUpdate () {//hopefully gives a bit of time to run visual-extrapolate-position systems.
			//Also camera velocity systems!
			velocity.Value = Vector2.zero;
		}
	}
}