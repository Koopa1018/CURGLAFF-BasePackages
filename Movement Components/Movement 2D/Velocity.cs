using UnityEngine;
using Unity.Mathematics;

namespace Clouds.Movement2D {
	[AddComponentMenu("Movement/2D/Velocity 2D")]
	public class Velocity : MonoBehaviour {
		public float2 Value {get; set;}

		public Velocity () {
			Value = 0;
		}

		public float x {
			get => Value.x;
			set => Value = new float2(value, Value.y);
		}
		public float y {
			get => Value.y;
			set => Value = new float2(Value.x, value);
		}
	}
}