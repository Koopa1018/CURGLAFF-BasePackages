using UnityEngine;

namespace Clouds.PlayerInput
{
	public abstract class ButtonInputField : MonoBehaviour {
		ButtonState _state = ButtonState.None;
		public ButtonState state => _state;
		
		/// <summary>
		/// Was the button pressed this frame?
		/// * The button is considered "pressed" if it's down this frame, but was not in the last one.
		/// </summary>
		public bool Pressed => _state.HasFlag(ButtonState.IsDown) && !_state.HasFlag(ButtonState.WasDown);
		/// <summary>
		/// Was the button held down this frame AND last frame?
		/// * If you need to check whether it was down this frame, read from this.Value.
		/// </summary>
		public bool HeldOver => _state.HasFlag(ButtonState.Held);
		/// <summary>
		/// Was the button released this frame?
		/// * The button is considered "released" if it was down last frame, but is not in this one.
		/// </summary>
		public bool Released => _state.HasFlag(ButtonState.WasDown) && !_state.HasFlag(ButtonState.IsDown);

		/// <summary>
		/// Was the button held this frame?
		/// </summary>
		public bool Value {
		 	get => _state.HasFlag(ButtonState.IsDown);
		 	set {
		 		_state.TakeInputs(value);
		 	}
		}

	}
}