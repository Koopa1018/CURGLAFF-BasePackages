using UnityEngine;
using Unity.Mathematics;

namespace Clouds.PlayerInput
{
	public abstract class AxisInputField : MonoBehaviour {
		public float2 Value {get; set;}

		public float x {get => Value.x;}
		public float y {get => Value.y;}

		public static implicit operator float2 (AxisInputField input) {
			return input.Value;
		}
	}
}