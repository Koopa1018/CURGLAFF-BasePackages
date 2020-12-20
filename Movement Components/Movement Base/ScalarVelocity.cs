using UnityEngine;
using Unity.Mathematics;
using System.Collections;

namespace Clouds.MovementBase {
	/// <summary>
	/// A component representing one-dimensional velocity.
	/// </summary>
	[DisallowMultipleComponent]
	[AddComponentMenu("Hidden/Platformer/Scalar Velocity")]
	public class ScalarVelocity : MonoBehaviour {
		[Tooltip(
@"The terminal-velocity value to start out with. Velocity can never be stronger than the set terminal velocity in either direction.

This value will be stored after initial use; it can be reverted to by calling RevertTerminalVelocity().

Set 0 to disable terminal velocity."
		)]
		[SerializeField] [Min(0)] float initialTerminalVelocity = 0;
		[Tooltip(
@"When checking whether velocity is rising or falling, this value is the minimum velocity that will be reported.
This does not affect the actual velocity value."
		)]
		[SerializeField] [Min(0)] float smallestValueReported = 0.000003f;

		float _value = 0;
		/// <summary>
		/// The actual rising/falling velocity value currently stored.
		/// This will never have a scalar magnitude higher than <c>TerminalVelocity</c>, if it is set.
		/// </summary>
		public float Value {
			get => _value;
			set {
				//Clamp the set value to scalarMagnitude <= TerminalVelocity (unless TVeloc == 0).
				_value = TerminalVelocity == 0 ? 
					value :
					math.clamp(value,-TerminalVelocity,TerminalVelocity)
				;
				//On account of variant terminal velocity, reset that to default after one use.
				RevertTerminalVelocity();
			}
		}

		/// <summary>
		/// Revert stored velocity to 0.
		/// </summary>
		public void RevertValue () {
			_value = 0;
		}

		float _terminal = 0;
		/// <summary>
		/// The highest magnitude the velocity can have in either direction.
		/// Signs will be discarded when given. Can be disabled by setting as 0.
		/// </summary>
		public float TerminalVelocity {get => _terminal; set => _terminal = math.abs(value);}

		/// <summary>
		/// Revert terminal velocity to the value set in the editor.
		/// </summary>
		public void RevertTerminalVelocity () {
			TerminalVelocity = initialTerminalVelocity;
		}

		/// <summary>
		/// Constructs a new ScalarVelocity component with default values.
		/// </summary>
		public ScalarVelocity () {
			TerminalVelocity = initialTerminalVelocity;
		}
		/// <summary>
		/// Constructs a new ScalarVelocity component with given editor parameters.
		/// </summary>
		/// <param name="initialTerminalVelocity">The terminal-velocity value to start out with.
		/// Velocity can never be stronger than the set terminal velocity in either direction.
		/// This value will be stored after initial use; it can be reverted to by calling RevertTerminalVelocity().
		/// Sign will be ignored if set to negative; go right ahead, if you feel you must.</param>
		/// <param name="smallestValueReported">When checking whether velocity is rising or falling, 
		/// this value is the minimum velocity that will be reported.
		/// This does not affect the actual velocity value.</param>
		public ScalarVelocity (float initialTerminalVelocity, float smallestValueReported = 0.000003f) {
			this.initialTerminalVelocity = initialTerminalVelocity;
			this.smallestValueReported = smallestValueReported;

			TerminalVelocity = initialTerminalVelocity;
		}


		/// <summary>
		/// Is the velocity [functionally] nonzero with a positive sign?
		/// </summary>
		public bool isPositive () => Value > smallestValueReported;
		/// <summary>
		/// Is the velocity [functionally] nonzero with a negative sign?
		/// </summary>
		public bool isNegative () => Value < -smallestValueReported;
		/// <summary>
		/// Is the velocity close to zero, within configurable margin?
		/// </summary>
		public bool isApproximatelyZero () => !isPositive() && !isNegative();
		/// <summary>
		/// Is the velocity _literally_ zero?
		/// </summary>
		public bool isZero () => Value == 0;


		public static implicit operator float (ScalarVelocity sv) {
			return sv.Value;
		}

	}
}