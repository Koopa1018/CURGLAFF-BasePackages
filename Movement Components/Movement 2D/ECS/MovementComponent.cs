#if UNITY_ENTITIES
	using Unity.Collections;
	using Unity.Mathematics;

	namespace Clouds.Movement2D {
		[System.Serializable]
		public struct Movement2DComponent : Unity.Entities.IComponentData {
			[UnityEngine.HideInInspector] public float2 Value;

			public Movement2DComponent (float2 value) {
				Value = value;
			}

			public float x { get => Value.x; set => Value.x = value;}
			public float y { get => Value.y; set => Value.y = value;}
		}
	}
#endif