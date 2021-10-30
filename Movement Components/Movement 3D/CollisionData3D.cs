using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionData3D : MonoBehaviour {
#if UNITY_EDITOR
	[HideInInspector] [SerializeField] CollisionFlags _value;
	public CollisionFlags Value {get => _value; set => _value = value;}
#else
	public CollisionFlags Value {get; set;}
#endif
}
