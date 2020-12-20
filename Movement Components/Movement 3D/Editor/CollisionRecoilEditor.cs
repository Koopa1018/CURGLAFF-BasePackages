using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Clouds.Collision3D {
	[CustomEditor(typeof(CollisionRecoil3D))]
	public class CollisionRecoil3DEditor : Editor {
		SerializedProperty value;
		SerializedProperty showDebugInfo;

		void OnEnable () {
			value = serializedObject.FindProperty("Value");
		}

		public override void OnInspectorGUI () {
			serializedObject.Update();

			EditorGUI.BeginDisabledGroup(true);
			EditorGUILayout.Vector3Field(string.Empty, (target as CollisionRecoil3D).Value);
			EditorGUI.EndDisabledGroup();
		}

		public override bool RequiresConstantRepaint() {
			return EditorApplication.isPlaying;
		}

	}
}