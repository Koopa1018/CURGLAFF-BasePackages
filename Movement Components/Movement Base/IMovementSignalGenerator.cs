using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Clouds.MovementBase {
	/// <summary>
	/// Implementors of this interface are expected to produce some kind of movement signal,
	/// expected to be delivered as movement deltas from frame to frame,
	/// which at the user's discretion may or may not directly be fed into position code.
	/// </summary>
	/// <typeparam name="TOut">The desired type of the output signal.</typeparam>
	/// <typeparam name="TIn">The desired type of input for the signal generator.</typeparam>
	public interface IMovementSignalGenerator <TOut, TIn> {
		/// <summary>
		/// Generates this frame's signal step.
		/// </summary>
		/// <param name="input">Whatever value is desired as input.</param>
		/// <param name="deltaTime">The time the previous frame lasted,
		/// which can be treated as the time this frame is going to last.
		/// As usual, multiplying by this will convert an "x" signal to an "x"-per-second signal.</param>
		/// <returns>The distance moved this frame.</returns>
		TOut GenerateSignal (TIn input, float deltaTime);
	}
}