using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.Mathematics;

using Clouds.Collision2D;

namespace Clouds.Movement2D {
	[CustomEditor(typeof(ApplyMovement), true)]
	public class ApplyMovementEditor : Editor {
		SerializedProperty myVelocityField;

		void OnEnable () {
			myVelocityField = serializedObject.FindProperty("velocity");
		}

		public override void OnInspectorGUI () {
			//Run main editor.
			base.OnInspectorGUI();

			//Separate main from debug info.
			EditorGUILayout.Space();

			//Show debug info.
			ReportVelocityApplier();
			ReportCollisionHandler();
		}

		void ReportCollisionHandler () {
			MonoBehaviour collisionComponent = (target as MonoBehaviour)
												.gameObject
												.GetComponentInChildren<ICollisionHandler>()
												as MonoBehaviour;

			if (collisionComponent != null) {
				GameObject owner = collisionComponent.gameObject;
				string objectName = 
					owner == (target as MonoBehaviour).gameObject ?
						"this object"
					:
						owner.name
				;


				EditorGUILayout.HelpBox(
	$"ApplyMovement will also reference {objectName}'s {collisionComponent.GetType().ToString()} at runtime.",
					MessageType.Info
				);

			} else {
				EditorGUILayout.HelpBox(
	@"ApplyMovement doesn't have a collision-handler component on its GameObject.
Output will not be affected by collisions.",
					MessageType.Warning
				);
			}
		}

		void ReportVelocityApplier () {
			IClearVelocity2D[] handlerCandidates = (target as MonoBehaviour)
													.gameObject
													.GetComponentsInChildren<IClearVelocity2D>();
			MonoBehaviour veloHandler = null;
			Velocity myVelocity = (Velocity)myVelocityField.objectReferenceValue;

			//Early exit if velocity is unset.
			if (myVelocity == null) {
				return;
			}
			
			foreach (IClearVelocity2D c in handlerCandidates) {
				if (c.EDI_velocityReference == (Velocity)myVelocityField.objectReferenceValue) {
					veloHandler = (MonoBehaviour)c;
					break;
				}
			}

			if (veloHandler != null) {
				GameObject owner = veloHandler.gameObject;
				string objectName = 
					owner == (target as MonoBehaviour).gameObject ?
						"this object"
					:
						owner.name
				;


				EditorGUILayout.HelpBox(
	$"ApplyMovement will reference {objectName}'s {veloHandler.GetType().ToString()} at runtime.",
					MessageType.Info
				);

			} else {
				GameObject owner = (target as MonoBehaviour).gameObject;
				EditorGUILayout.HelpBox(
	@"There is no ClearVelocityOn_____ component within this game object hierarchy.
To avoid unexpected behavior, please ensure that some such component clears this velocity.",
					MessageType.Warning
				);
				if (GUILayout.Button("Add ClearVelocityOnBeginFixedUpdate")) {
					var veloClearer = owner.AddComponent<ClearVelocityOnBeginFixedUpdate>();
					veloClearer.EDI_velocityReference = myVelocity;
				}
			}
		}

	}
}