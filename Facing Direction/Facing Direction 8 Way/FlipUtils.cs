using Unity.Mathematics;
using static Unity.Mathematics.math;

namespace Clouds.Facing2D {
	public static class FlipUtils {
		/// <summary>
		/// Checks if the sign of left is opposite compared to the sign of right. Returns false if either sign is 0.
		/// </summary>
		public static bool checkSignFlip (int left, int right) {
			return left != 0 & sign(left) == sign(-right);
		}
		/// <summary>
		/// Checks if the sign of left is opposite compared to the sign of right. Returns false if either sign is 0.
		/// </summary>
		public static bool2 checkSignFlip (int2 left, int2 right) {
			return left != 0 & sign(left) == sign(-right);
		}
		/// <summary>
		/// Checks if the sign of left is opposite compared to the sign of right. Returns false if either sign is 0.
		/// </summary>
		public static bool3 checkSignFlip (int3 left, int3 right) {
			return left != 0 & sign(left) == sign(-right);
		}
		/// <summary>
		/// Checks if the sign of left is opposite compared to the sign of right. Returns false if either sign is 0.
		/// </summary>
		public static bool4 checkSignFlip (int4 left, int4 right) {
			return left != 0 & sign(left) == sign(-right);
		}
	}
}
		