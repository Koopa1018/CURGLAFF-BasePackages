using UnityEngine;
using Unity.Mathematics;

namespace Clouds.SceneManagement {
	public struct SpawnData {
		public static string NextSpawnPoint;
#if CLOUDS_FACING_2D || CLOUDS_FACING_8WAY
		public static float2 Facing;
#endif
	}
}