using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Clouds.Facing2D._Editor {
	[CustomEditor(typeof(FacingDirection))]
	public class FacingDirectionEditor : Editor {
		//TODO: Replace the direct numerical display with an angle knob.

		public override bool RequiresConstantRepaint() => Application.isPlaying;

		SerializedProperty _value;
		void OnEnable () {
			_value = serializedObject.FindProperty("_value");
		}

		public override void OnInspectorGUI() {
			using (new EditorGUI.DisabledGroupScope(Application.isPlaying)) {
				EditorGUILayout.PropertyField(_value);
			}
		}
	}
}
