using UnityEngine;
using Unity.Mathematics;
using Clouds.PlayerInput;

namespace Clouds.Facing2D {
#if CLOUDS_INPUT
	public class FacingFromInput_8Way : MonoBehaviour {
		[SerializeField] AxisInputField axisInput;
		bool hasAxisInput = false;
		[SerializeField] ScalarInputField xInput;
		bool hasXInput = false;
		[SerializeField] ScalarInputField yInput;
		bool hasYInput = false;
		[SerializeField] [Range(0,1)] float xThreshold;
		[SerializeField] [Range(0,1)] float yThreshold;
		[SerializeField] bool2 invertAxes = false;

		[Space()]

		[SerializeField] FacingDirection_8Way output;

		void OnEnable () {
			hasAxisInput = axisInput != null;
			hasXInput = xInput != null;
			hasYInput = yInput != null;
		}

		int thresholded (float input, float threshold) {
			return input > threshold ?
					1 :
					input < -threshold ?
						-1 :
						0
					;
		}

		// Update is called once per frame
		void Update() {
			if (hasAxisInput) {
				output.x = thresholded(axisInput.x * (invertAxes.x ? -1 : 1), xThreshold);
				output.y = thresholded(axisInput.y * (invertAxes.y ? -1 : 1), yThreshold);
			} else {
				if (hasXInput) {
					output.x = thresholded(xInput.Value * (invertAxes.x ? -1 : 1), xThreshold);
				}
				if (hasYInput) {
					output.y = thresholded(yInput.Value * (invertAxes.y ? -1 : 1), yThreshold);
				}
			}
		}
	}
#else
	/// <summary>
	/// This class requires the package "clouds.input.base" in order to function.
	/// </summary>
	public class FacingFromInput_8Way : MonoBehaviour {
		//Empty.
	}
#endif
}