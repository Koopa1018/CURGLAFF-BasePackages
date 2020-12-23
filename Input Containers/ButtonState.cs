namespace Clouds.PlayerInput {
	/// <summary>
	/// The state of a button.
	/// Please take note: this enum is designed to REMEMBER whether or not it was _pressed_ to enter a hold state!
	/// </summary>
	[System.Flags]
	public enum ButtonState : byte {
		None = 0,
		IsDown = 1,
		WasDown = 2,
		Held = 3,
		HoldStartedOnAPress = 4 | 2
	}

	/// <summary>
	/// The state of a button using an older system.
	/// Intended to help the transition to new enum. Not very well supported.
	/// In this system, Pressed stays true from button down through Released, until becomes None again.
	/// </summary>
	public enum ButtonStateOld : byte {
		None = 0,
		Pressed = 1, //Stays true from button down through Released, until becomes None again.
		Held = 2, //Functionally equivalent to Pressed in the new version.
		Released = 4
	}

	public static class ButtonStateMethods {
		public static void TakeInputs (
			ref this ButtonState state,
			bool button,
			bool lastButton
		) {
			//Cache this info from last state.
			bool holdStartedOnAPress = state == ButtonState.IsDown;

			//Clear the state now.
			state = ButtonState.None;

			//Set state from information cached.
			if (holdStartedOnAPress) {
				state |= ButtonState.HoldStartedOnAPress;
			}

			if (button) {
				state |= ButtonState.IsDown;
			}
			if (lastButton) {
				state |= ButtonState.WasDown;
			}
		}

		public static void TakeInputs (
			ref this ButtonState state,
			bool button
		) {
			TakeInputs(ref state, button, state.HasFlag(ButtonState.IsDown));
		}

		public static void TakeInputs (
			ref this ButtonStateOld state,
			bool button,
			bool lastButton
		) {
			//Drop last frame's Hold and Release flags. (Remember if the button was pressed earlier!)
			state &= ButtonStateOld.Pressed;

			//If the button is down, flag the state as Held.
			if (button) {
				//If the button was not down last frame, flag state anew as Pressed.
				//If it was, flag state anew as Held.
				state |= lastButton ? ButtonStateOld.Held : ButtonStateOld.Pressed;
			}
			//If button was down last frame but not this one, set state to Released.
			if (lastButton && !button) {
				state |= ButtonStateOld.Released;
			}
			//If button was up last frame and still is, let go of the Pressed flag.
			if (!button && !lastButton) {
				state = ButtonStateOld.None;
			}
		}
	}
}