#define DEBUG
#define NO_INIT_SCENE
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Clouds.SceneManagement {
	/// <summary>
	/// Handles all things related to scenes and scene transitions.
	/// </summary>
	public static class ScenePreprocessor {
		const string SPAWN_POINT_TAG = "SpawnPoints";
		const string DEFAULT_START_POINT_NAME = "Start Point";
		
#if UNITY_EDITOR
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		static void RegisterCleanUp () {
			Application.quitting += CleanUp;
		}
		
		static void CleanUp () {
			_foundSpawnPoint = false;
		}
#endif

		static Vector3 _selectedSpawnPoint;
		public static Vector3 SelectedSpawnPoint => _selectedSpawnPoint;

		static bool _foundSpawnPoint = false;
		public static bool SpawnPointIsValid => _foundSpawnPoint;

		internal static Vector3 FindSpawnPoint (Scene scene, LoadSceneMode mode) {
			//To track whether we've found a spawn point.
			_foundSpawnPoint = false;
			//To track the position of the desired spawn point.
			_selectedSpawnPoint = default(Vector3);

			//Populate the list of spawn points with objects tagged "SpawnPoints."
			GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag(SPAWN_POINT_TAG);

			//If no spawn points were found, find any object named "Start".
			if (spawnPoints.Length == 0) {
				//Reduce spawnPoints down to 1 length.
				spawnPoints = new GameObject[1];
				
				//Find any game object named "Start Point".
				spawnPoints[0] = GameObject.Find(DEFAULT_START_POINT_NAME);

				//If we found one, use it.
				if (spawnPoints[0] != null) {
					//Flag that we're not going to bother.
					_foundSpawnPoint = true;

					//Remember its position!
					_selectedSpawnPoint = spawnPoints[0].transform.position;
				}

				//Didn't find "Start Point"? Just leave the player where he is.

				//Return the found spawn point.
				return _selectedSpawnPoint;
			}

			//Spawn points were found.

			//Find the correct one, or the default if no "correct one" was found.
			for (int i = 0; i < spawnPoints.Length; i++) {
				//If we find an object of the right name, use its data.
				if (spawnPoints[i].name == SpawnData.NextSpawnPoint) {
					_foundSpawnPoint = true;
					_selectedSpawnPoint = spawnPoints[i].transform.position;

					//We've found our guy, we're done with this loop.
					break;
				}
				//If we find an object with the default name, remember its data, but don't stop.
				else if (spawnPoints[i].name == DEFAULT_START_POINT_NAME) {
					_foundSpawnPoint = true;
					_selectedSpawnPoint = spawnPoints[i].transform.position;

					//DON'T break, in case we find a better option.
					continue;
				}
			}

			//Return whatever we find.
			return _selectedSpawnPoint;
		}
	}
}