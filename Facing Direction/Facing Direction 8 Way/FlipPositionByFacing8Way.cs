using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using static Unity.Mathematics.math;

namespace Clouds.Facing2D {
	public class FlipPositionByFacing8Way : MonoBehaviour {
		[SerializeField] FacingDirection_8Way facingDir;
		[SerializeField] Transform[] objectsToFlip;
		[SerializeField] bool2 axesToFlipAround;

		int2 lastFacing = 1;

		// Update is called once per frame
		void Update() {
			//Update SELECTED axes if NONZERO and DIRECTION IS OPPOSITE.
			bool2 doFlip = axesToFlipAround & FlipUtils.checkSignFlip(facingDir.Value, lastFacing);

			float3 flipper = new float3(
				doFlip.x ? -1 : 1,
				doFlip.y ? -1 : 1,
				1 //always remain the same when multiplied~
			);

			for (int i = 0; i < objectsToFlip.Length; i++) {
				objectsToFlip[i].localPosition *= flipper;
			}

			lastFacing = select(lastFacing, facingDir.Value, doFlip);
		}
	}
}