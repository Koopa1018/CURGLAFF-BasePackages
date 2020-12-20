using UnityEditor;

namespace Clouds.MovementBase {
	[CustomEditor(typeof(ScalarVelocity), true)]
	public class RisingFallingVelocityEditor : Editor {
		SerializedProperty showDebugInfo;

		public override void OnInspectorGUI () {
			//Run main editor.
			base.OnInspectorGUI();

			//Separate main from debug info.
			EditorGUILayout.Space();

			//Show debug info.
			serializedObject.Update();

			EditorGUI.BeginDisabledGroup(true);
			EditorGUILayout.FloatField("Value", (target as ScalarVelocity).Value);
			EditorGUILayout.FloatField("Terminal Velocity", (target as ScalarVelocity).TerminalVelocity);
			EditorGUI.EndDisabledGroup();
		}

		public override bool RequiresConstantRepaint() {
			return EditorApplication.isPlaying;
		}

	}
}