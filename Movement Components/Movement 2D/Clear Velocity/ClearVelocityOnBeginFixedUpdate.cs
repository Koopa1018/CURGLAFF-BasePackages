using UnityEngine;
using Unity.Mathematics;

namespace Clouds.Movement2D {
	[DefaultExecutionOrder(-20)]
	[AddComponentMenu("Movement/2D/Clear Velocity 2D on Begin FixedUpdate()")]
	public class ClearVelocityOnBeginFixedUpdate : MonoBehaviour, IClearVelocity2D {
		[SerializeField] Velocity velocity;
#if UNITY_EDITOR
		public Velocity EDI_velocityReference {get => velocity; set => velocity = value;}
#endif

		public ClearVelocityOnBeginFixedUpdate (Velocity velocity) {
			this.velocity = velocity;
		}

		void FixedUpdate () {
			velocity.Value = 0;
		}
	}
}