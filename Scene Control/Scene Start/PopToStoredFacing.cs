using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

#if CLOUDS_FACING_2D
using FacingDirection = Clouds.Facing2D.FacingDirection;
#endif

namespace Clouds.SceneManagement {
#if CLOUDS_FACING_2D
	/// <summary>
	/// On scene start, loads the facing direction given by spawn data over this object's facing direction.
	/// </summary>
	[RequireComponent(typeof(FacingDirection))]
	[DefaultExecutionOrder(-99)]
	public class PopToStoredFacing : MonoBehaviour {
		// Start is called before the first frame update
		void Start() {
			//If our stored facing direction is valid, or not (0,0), write it to the facing component.
			if (math.any(SpawnData.Facing != 0)) {
				Debug.Log("Retrieving facing data from spawn data...");
				//Find facing direction component.
				FacingDirection dir = GetComponent<FacingDirection>();

				//Set its value from spawn data.
				dir.Value = SpawnData.Facing;

				Debug.Log("New facing is " + dir.Value);
			}

			//Remove this component so it doesn't bog us down.
			Destroy(this);
		}

	}
#else 
	/// <summary>
	/// On scene start, loads the facing direction given by spawn data. Requires package "Clouds.Facing2D;" does nothing otherwise.
	/// </summary>
	public class PopToStoredFacing : MonoBehaviour {

	}
#endif

}