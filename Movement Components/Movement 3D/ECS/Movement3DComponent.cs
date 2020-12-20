using Unity.Collections;
using Unity.Mathematics;

namespace Clouds.Movement3D {
	[System.Serializable]
	public struct Velocity3DComponent
#if UNITY_ENTITIES
		: Unity.Entities.IComponentData 
#endif
	{
		[UnityEngine.HideInInspector] public float3 Value;

		public Velocity3DComponent (float3 value) {
			Value = value;
		}

		public float x { get => Value.x; set => Value.x = value;}
		public float y { get => Value.y; set => Value.y = value;}
	}
}