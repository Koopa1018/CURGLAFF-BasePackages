using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using static Unity.Mathematics.math;

namespace Clouds.Facing2D {
	public class FlipSpriteByFacing8Way : MonoBehaviour {
		[SerializeField] FacingDirection_8Way facingDir;
		[SerializeField] SpriteRenderer[] spritesToFlip;
		[SerializeField] bool2 axesToFlipAround;

		int2 lastFacing = 1;

		// Update is called once per frame
		void Update() {
			//Update SELECTED axes if NONZERO and DIRECTION IS OPPOSITE.
			bool2 doFlip = axesToFlipAround & FlipUtils.checkSignFlip(facingDir.Value, lastFacing);

			for (int i = 0; i < spritesToFlip.Length; i++) {
				spritesToFlip[i].flipX = doFlip.x ? !spritesToFlip[i].flipX : spritesToFlip[i].flipX;
				spritesToFlip[i].flipY = doFlip.y ? !spritesToFlip[i].flipY : spritesToFlip[i].flipY;
			}

			lastFacing = select(lastFacing, facingDir.Value, doFlip);
		}
	}
}