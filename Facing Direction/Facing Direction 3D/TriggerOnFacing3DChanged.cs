using UnityEngine;
using UnityEngine.Events;
using Unity.Mathematics;

namespace Clouds.Facing2D
{
	public class TriggerOnFacing3DChanged : MonoBehaviour {
		/// <summary>
		/// Event which accepts a facing direction's value as an argument.
		/// </summary>
		[System.Serializable]
		public sealed class FacingEvent : UnityEvent<float2> {}

		//Reference to the facing direction in question.
		[SerializeField] FacingDirection facingDirectionToCheck;

		[Tooltip("These events will trigger when the facing direction is changed.")]
		[SerializeField] public FacingEvent onFacingChanged = new FacingEvent();

		//To track the previous value of facingDirectionToCheck, so we know whether it's changed!
		float2 lastFacingDirection = 0;

		void Awake() {
#if UNITY_EDITOR
			// If we wake up and there's no facing direction component, raise a fuss!
			if (facingDirectionToCheck == null) {
				//Display a console warning complaining about the missing reference.
				Debug.LogError(
					"This trigger is trying to watch no facing component for changes.",
					this
				);
				//And then return, because we can't set lastFacingDirection from no component,
				//and Update is set up to just gracefully exit before something bad happens.
				return;
			}
#endif

			//Read the initial value of the facing component into lastFacingDirection.
			//Otherwise, we'd get a hit as soon as the game starts, and we'd fall apart from that.
			lastFacingDirection = facingDirectionToCheck.Value;
		}

		// Update is called once per frame
		void Update() {
#if UNITY_EDITOR
			//If no facing-direction component is there to watch, gracefully exit (Awake raises the stink).
			if (facingDirectionToCheck == null) return;
#endif

			//If the facing value this frame is different from last, call the event!
			if (math.any(facingDirectionToCheck.Value != lastFacingDirection)) {
				//Call the event, passing in the new facing direction as argument.
				onFacingChanged.Invoke(facingDirectionToCheck.Value);
			}

			//Update lastFacingDirection for the next time we check (next frame).
			lastFacingDirection = facingDirectionToCheck.Value;			
		}

}	}