using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Clouds.Collision3D {
	[CustomEditor(typeof(CollisionData3D))]
	public class CollisionData3DEditor : Editor {
		SerializedProperty value;

		void OnEnable () {
			value = serializedObject.FindProperty("_value");
		}

		public override void OnInspectorGUI () {
			serializedObject.Update();

			EditorGUI.BeginDisabledGroup(true);
			EditorGUILayout.PropertyField(value);
			EditorGUI.EndDisabledGroup();
		}

		public override bool RequiresConstantRepaint() {
			return EditorApplication.isPlaying;
		}

	}
}