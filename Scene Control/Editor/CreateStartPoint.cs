using UnityEngine;
using UnityEditor;

namespace Galbet.Editor {
	public static partial class CreateUtilityObjects {
		[MenuItem("GameObject/Spawn Point", false, 10)]
		static void CreateSpawnPoint () {
			//Lock all undo operations into a single group.
			Undo.SetCurrentGroupName("Add Spawn Point");
			int group = Undo.GetCurrentGroup();

			GameObject spawner = new GameObject(
				"Spawn Point"
			);
			spawner.tag = "SpawnPoints";
			Undo.RegisterCreatedObjectUndo(spawner, "Add Spawn Point");

			GameObject parent = GameObject.Find("Spawn Points");
			if (parent == null) {
				parent = new GameObject (
					"Spawn Points"
				);
				Undo.RegisterCreatedObjectUndo(parent, "Add Spawn Point Folder");
			}
			spawner.transform.SetParent(parent.transform);
			Undo.SetTransformParent(spawner.transform, parent.transform, "Move Spawn Point Into Folder");
			
			Selection.activeGameObject = spawner;
			
			//Ensure that all operations are treated as a single action.
			Undo.CollapseUndoOperations(group);
		}
	}
}