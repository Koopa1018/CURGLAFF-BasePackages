using UnityEngine;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Burst;

namespace Clouds.Raycasting 
{
	public static partial class RaycastHit2DExtensions {
			//If you Physics2D.RaycastNonAlloc(...) twice into a raycast buffer 2 wide or longer,
			//the second raycast will not fill up beginning after the first one. It's just going to
			//replace the first one.
			//Bear in mind, you still have to clear it, because null hits don't automatically do that.
		
		/// <summary>
		/// Returns the first RaycastHit2D which doesn't match myCollider.
		/// </summary>
		/// <param name="hits">The array of hits to be checked? Is extension, shouldn't show.</param>
		/// <param name="myCollider"> The collider to check against and filter out.</param>
		/// <returns>The first RaycastHit2D that didn't hit myCollider. If all hits did or array is empty, return a blank.</returns>
		public static RaycastHit2D FirstNonSelfHit (this RaycastHit2D[] hits, Collider2D myCollider) {
			//If there's no collider passed, don't even check--just return element 0.
			if (myCollider == null) {
				return hits[0];
			}
			
			RaycastHit2D returner = new RaycastHit2D();

			//If there's a first element AND it's not my collider, just use that.
			if (hits.Length > 0 & hits[0].collider != myCollider) {
				returner = hits[0];
			}
			//Now we know the first is equal to mine. If there's a second element, use that.
			else if (hits.Length > 1 /*& hits[0].collider == myCollider*/) {
				returner = hits[1];
			}			
			//Any other result (hits.Length == 0, hits.Length == 1 & hits[0].collider == myCollider)
			//will just return blank.

			return returner;				
		}
			
	}
}