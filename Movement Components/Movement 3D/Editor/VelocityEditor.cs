using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Clouds.Movement3D {
	[CustomEditor(typeof(Velocity3D))]
	public class Velocity3DEditor : Editor {
		SerializedProperty value;
		SerializedProperty showDebugInfo;

		void OnEnable () {
			value = serializedObject.FindProperty("Value");
		}

		public override void OnInspectorGUI () {
			serializedObject.Update();

			EditorGUI.BeginDisabledGroup(true);
			EditorGUILayout.Vector3Field(string.Empty, (target as Velocity3D).Value);
			EditorGUI.EndDisabledGroup();
		}

		public override bool RequiresConstantRepaint() {
			return EditorApplication.isPlaying;
		}

	}
}