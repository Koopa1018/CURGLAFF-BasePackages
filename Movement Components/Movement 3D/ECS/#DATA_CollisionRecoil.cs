#if UNITY_ENTITIES
	using System;
	using Unity.Collections;
	using Unity.Entities;
	using Unity.Mathematics;


	namespace Clouds.Collision3D {
		[Serializable]
		public struct CollisionRecoil3DComponent : IComponentData {
			public float3 Value;
		}
	}
#endif