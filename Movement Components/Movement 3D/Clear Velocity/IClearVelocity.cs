using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Clouds.Movement3D {
	public interface IClearVelocity3D {
		#if UNITY_EDITOR
			Velocity3D EDI_velocityReference {get; set;}
		#endif
	}
}