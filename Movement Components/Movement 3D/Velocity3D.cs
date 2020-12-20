using UnityEngine;
using Unity.Mathematics;
using System.Collections.Generic;

namespace Clouds.Movement3D {
	[AddComponentMenu("Movement/3D/Velocity 3D")]
	public class Velocity3D : MonoBehaviour {
		public float3 Value {get; set;}

		public Velocity3D () {
			Value = 0;
		}

		public float x {
			get => Value.x;
			set => Value = new float3(value, Value.y, Value.z);
		}
		public float y {
			get => Value.y;
			set => Value = new float3(Value.x, value, Value.z);
		}
		public float z {
			get => Value.z;
			set => Value = new float3(Value.x, Value.y, value);
		}
	}
}