using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Clouds.PlayerInput {

	[DefaultExecutionOrder(-50)] //MUST update BEFORE the main input loop.
	[RequireComponent(typeof(IGenerateInputSignals))]
	[DisallowMultipleComponent]
	public class InputGetter : MonoBehaviour {
		//To get input signals from.
		IGenerateInputSignals signalProducer;

		//Find an input signal producer component.
		void Awake () {
			signalProducer = GetComponent<IGenerateInputSignals>();
		}

		//Every frame, have the signal producer do its thing.
		void Update () {
			//If the signal producer is non-enabled, abort.
			MonoBehaviour signalAsBeh = signalProducer as MonoBehaviour;
			if (!signalAsBeh.isActiveAndEnabled) {
				//Wipe input on producer disabled, so we don't end up with hanging input.
				signalProducer.ClearInputSignal();

				return;
			}
			
			signalProducer.GenerateInputSignal();
		}

		
		void OnDisable () {
			//Here as a stub, to show that we DO NOT clear signal inputs on disable this guy.
		}
	}
}