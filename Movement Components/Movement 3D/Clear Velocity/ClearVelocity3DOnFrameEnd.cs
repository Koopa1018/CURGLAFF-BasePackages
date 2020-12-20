using UnityEngine;
using Unity.Mathematics;

namespace Clouds.Movement3D {
	[DefaultExecutionOrder(20)]
	[AddComponentMenu("Movement/3D/Clear Velocity 2D on Frame End")]
	public class ClearVelocity3DOnFrameEnd : MonoBehaviour, IClearVelocity3D {
		[SerializeField] Velocity3D velocity;
#if UNITY_EDITOR
		public Velocity3D EDI_velocityReference {get => velocity; set => velocity = value;}
#endif

		void LateUpdate () {//hopefully gives a bit of time to run visual-extrapolate-position systems.
			//Also camera velocity systems!
			velocity.Value = Vector3.zero;
		}
	}
}