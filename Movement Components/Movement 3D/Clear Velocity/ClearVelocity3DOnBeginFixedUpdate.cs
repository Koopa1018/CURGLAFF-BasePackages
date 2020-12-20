using UnityEngine;
using Unity.Mathematics;

namespace Clouds.Movement3D {
	[DefaultExecutionOrder(-20)]
	[AddComponentMenu("Movement/3D/Clear Velocity 3D on Begin FixedUpdate()")]
	public class ClearVelocity3DOnBeginFixedUpdate : MonoBehaviour, IClearVelocity3D {
		[SerializeField] Velocity3D velocity;
#if UNITY_EDITOR
		public Velocity3D EDI_velocityReference {get => velocity; set => velocity = value;}
#endif

		public ClearVelocity3DOnBeginFixedUpdate (Velocity3D velocity) {
			this.velocity = velocity;
		}

		void FixedUpdate () {
			velocity.Value = 0;
		}
	}
}