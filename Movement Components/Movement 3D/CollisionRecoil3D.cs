using UnityEngine;
using Unity.Mathematics;
#if UNITY_ENTITIES
using Unity.Entities;
#endif

namespace Clouds.Collision3D {
	public class CollisionRecoil3D : MonoBehaviour
#if UNITY_ENTITIES
	, IConvertGameObjectToEntity
#endif
	{
		[HideInInspector] public float3 Value;

		public float x {get => Value.x; set => Value.x = value;}
		public float y {get => Value.y; set => Value.y = value;}
		public float z {get => Value.z; set => Value.z = value;}

#if UNITY_ENTITIES
		public void Convert (Entity e, EntityManager em, GameObjectConversionSystem gcs) {
			em.AddComponentData(e, new CollisionRecoil3DComponent { Value = this.Value } );
		}
#endif
	}
}