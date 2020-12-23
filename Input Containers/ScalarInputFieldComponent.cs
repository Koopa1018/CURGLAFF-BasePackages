using UnityEngine;

namespace Clouds.PlayerInput
{
	public abstract class ScalarInputField : MonoBehaviour {
		public float Value {get; set;}

		public static implicit operator float (ScalarInputField input) {
			return input.Value;
		}
	}
}