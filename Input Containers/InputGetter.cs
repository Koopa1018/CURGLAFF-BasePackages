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
		void FixedUpdate () {
			//If the signal producer is non-enabled, abort and leave it hanging.
			if (!(signalProducer as MonoBehaviour).isActiveAndEnabled) {
				return;
			}
			
			//Signal producer is enabled.
			signalProducer.GenerateInputSignal();
		}

		
		void OnDisable () {
			//Wipe input, so we don't end up with hanging input.
			signalProducer.ClearInputSignal();
		}
	}
}
