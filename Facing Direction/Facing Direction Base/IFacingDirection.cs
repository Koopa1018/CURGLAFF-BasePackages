using UnityEngine;
using Unity.Mathematics;

namespace Clouds.Facing2D
{
	public interface IFacingDirection {
		float angle();
		float signedAngle();

		float2 normalized();
	}
}