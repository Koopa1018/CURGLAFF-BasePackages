using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Clouds.Collision2D {
	[CustomEditor(typeof(CollisionRecoil))]
	public class CollisionRecoilEditor : Editor {
		SerializedProperty value;
		SerializedProperty showDebugInfo;

		void OnEnable () {
			value = serializedObject.FindProperty("Value");
		}

		public override void OnInspectorGUI () {
			serializedObject.Update();

			EditorGUI.BeginDisabledGroup(true);
			EditorGUILayout.Vector2Field(string.Empty, (target as CollisionRecoil).Value);
			EditorGUI.EndDisabledGroup();
		}

		public override bool RequiresConstantRepaint() {
			return EditorApplication.isPlaying;
		}

	}
}