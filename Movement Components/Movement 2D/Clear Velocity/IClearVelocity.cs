using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Clouds.Movement2D {
	public interface IClearVelocity2D {
		#if UNITY_EDITOR
			Velocity EDI_velocityReference {get; set;}
		#endif
	}
}