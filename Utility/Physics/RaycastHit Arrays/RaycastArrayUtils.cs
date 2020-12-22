using UnityEngine;

namespace Clouds.Raycasting 
{
	public static partial class RaycastHitExtensions {
			//If you Physics.RaycastNonAlloc(...) twice into a raycast buffer 2 wide or longer,
			//the second raycast will not fill up beginning after the first one. It's just going to
			//replace the first one.
			//Bear in mind, you still have to clear it, because null hits don't automatically do that.

		public static void Clear (this RaycastHit[] hits) {
			for (int i = 0; i < hits.Length; i++) {
				hits[i] = new RaycastHit();
			}
		}
			
	}
}