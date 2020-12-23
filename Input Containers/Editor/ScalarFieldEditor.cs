using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Clouds.PlayerInput {
	[CustomEditor(typeof(ScalarInputField), true)]
	public class ScalarInputEditor : Editor {
		SerializedProperty value;
		SerializedProperty showDebugInfo;

		void OnEnable () {
			value = serializedObject.FindProperty("Value");
		}

		public override void OnInspectorGUI () {
			serializedObject.Update();

			EditorGUI.BeginDisabledGroup(true);
			//TODO: This should be a slider. (Probably scale so that 1.5 or 2 is visible?)
			EditorGUILayout.FloatField(string.Empty, (target as ScalarInputField).Value);
			EditorGUI.EndDisabledGroup();
		}

		public override bool RequiresConstantRepaint() {
			return EditorApplication.isPlaying;
		}

	}
}