#if UNITY_NEW_INPUT_SYSTEM && FORCE_UNITY_INPUTMANAGER
	#undef UNITY_NEW_INPUT_SYSTEM
#endif

using UnityEngine;
#if UNITY_NEW_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

using Clouds.PlayerInput;

namespace Clouds.Platformer.CharacterControl {
	public class PlayerButtonInput : MonoBehaviour, IGenerateInputSignals {
		#if UNITY_NEW_INPUT_SYSTEM
		[Header("Inputs")]
		[SerializeField] InputActionAsset inputMap;
		[SerializeField] string actionMap = "Weapons";
		[SerializeField] InputActionReference inputAction;
		#else
		[Header("Inputs")]
		[SerializeField] string buttonName = "Fire1";
		#endif

		[Header("Output Fields")]
		[SerializeField] ButtonInputField buttonOutput;

		#if UNITY_NEW_INPUT_SYSTEM
		void Awake () {
			inputMap.FindActionMap(actionMap, true).Enable();
		}
		#endif

		/// <summary>
		/// Fetch input from component data.
		/// NOTE: Sourcing it from here allows for easy switching out of control code:
		/// should you want AI controlled characters, just write to these from an
		/// enemy-AI component instead of a player-input-fetching component.
		/// </summary>
		public void GenerateInputSignal () {
			#if UNITY_NEW_INPUT_SYSTEM
				buttonOutput.Value = inputAction.action.ReadValue<float>() > 0;
			#else
				buttonOutput.Value = Input.GetButton (buttonName);
			#endif
		}

		public void ClearInputSignal() {
			buttonOutput.Value = false;
		} 

	}
}