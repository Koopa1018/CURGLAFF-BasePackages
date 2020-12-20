using UnityEngine;
using Unity.Mathematics;

namespace Clouds.Collision2D {
	public class CollisionRecoil : MonoBehaviour {
		[HideInInspector] public float2 Value;
		
		public float x {get => Value.x; set => Value.x = value;}
		public float y {get => Value.y; set => Value.y = value;}
	}
}