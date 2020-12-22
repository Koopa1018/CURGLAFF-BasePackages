//#define PLAYMODE_END_ON_TRANSITION
#if PLAYMODE_END_ON_TRANSITION && !UNITY_EDITOR
	#undef PLAYMODE_END_ON_TRANSITION
#endif

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
#if CLOUDS_TRANSITIONS
using Clouds.Transitions;
#endif

namespace Clouds.SceneManagement {
	public static class SceneChanger {
		static AsyncOperation sceneLoading;

#if CLOUDS_TRANSITIONS
		static TransitionSettings nextSceneTransitionIn;

		public static bool justFinishedTransition {get; private set;}

		const float CHECK_SCENE_LOADED_RESOLUTION = 0.5f;
#endif

		/// <summary>
		/// Go to a different scene.
		/// </summary>
		/// <param name="scenePath">The path of the scene. Must be a string, alas, lest we rule out AssetBundles.</param>
		/// <param name="spawner">The name of the object the player will spawn at the position of</param>
		/// <param name="settings">Settings for the transition into the next scene.</param>
		public static void GotoScene (string scenePath, string spawner
#if CLOUDS_TRANSITIONS
		, TransitionSettings settings
		) {
			GotoScene(scenePath, spawner
			, settings, settings
			);
		}

		/// <summary>
		/// Go to a different scene.
		/// </summary>
		/// <param name="scenePath">The path of the scene. Must be a string, alas, lest we rule out AssetBundles.</param>
		/// <param name="spawner">The name of the object the player will spawn at the position of</param>
		/// <param name="transitionInSettings">Settings for the transition out of this scene.</param>
		/// <param name="transitionInSettings">Settings for the transition into the next scene.</param>
		public static void GotoScene (
			string scenePath,
			string spawner
			, TransitionSettings transitionOutSettings,
			TransitionSettings transitionInSettings
#endif
		) {

#if CLOUDS_TRANSITIONS
			//Store the next scene's entrance transition.
			nextSceneTransitionIn = transitionInSettings;
#endif

#if !PLAYMODE_END_ON_TRANSITION
			//Begin loading the next scene using an AsyncOperation (because why not).
			sceneLoading = SceneChangerInternal.GoToScene(scenePath);
			//Prepare to transition in when the next scene begins.
			SceneManager.sceneLoaded += OnSceneBegin;
#endif
			//Set the next scene up to load into the correct place.
			SpawnData.NextSpawnPoint = spawner;

#if CLOUDS_TRANSITIONS
			//The technical stuff out of the way, now we can focus on the cosmetic stuff.

			//Subscribe to the transition master's end-of-transition event.
			TransitionMaster.Instance.OnTransitionFinished.AddListener(FinalizeSceneLoading);

			//Perform the outgoing transition.
			TransitionMaster.DoTransition(transitionOutSettings.transitionType, transitionOutSettings.vector, true);
#else
			sceneJumpToNext(scenePath);
#endif
		}

#if CLOUDS_TRANSITIONS
		/// <summary>
		/// To be passed to TransitionMaster.Instance.OnTransitionFinished as a listener.
		/// </summary>
		static void FinalizeSceneLoading () {
	#if !PLAYMODE_END_ON_TRANSITION
			//As this class has no GameObject, we have to parasitize the TransitionMaster to run this coroutine.
			TransitionMaster.Instance.StartCoroutine(finalizeSceneLoading());
	#else
			//Exit playmode.
			UnityEditor.EditorApplication.isPlaying = false;
	#endif
		}
		
		/// <summary>
		/// Coroutine: Wait for a scene to finish loading before cleaning up and moving on.
		/// </summary>
		static IEnumerator finalizeSceneLoading () {
			//To avoid GC.
			WaitForSecondsRealtime waitCommand = new WaitForSecondsRealtime(CHECK_SCENE_LOADED_RESOLUTION);

			//Wait until scene is loaded.
			while (sceneLoading.progress < 0.9f) {
				yield return waitCommand;
			}
			
			//Momentarily pause rendering so nobody sees the scene shifting around.
			//@TODO: This feature doesn't exist until 2019.3.

			//Now's a good time to clean up before we continue.
			cleanMemoryUp();
			
			//@TODO: loading screen for when the loading takes unusually long.

			//Mark this flag as true so anything that needs to react differently
			//can know it's time to do so.
			justFinishedTransition = true;

			//Now activate the scene.
			sceneLoading.allowSceneActivation = true;
		}
#endif

		/// <summary>
		/// Clean up unneeded things out of memory before moving to the next scene proper.
		/// </summary>
		static void cleanMemoryUp () {
			//This seems like a perfect time to clear all the garbage data!
			System.GC.Collect();
		}
			

		static void sceneJumpToNext (string scenePath) {
			//Clean up before we go to next scene.
			cleanMemoryUp();

			//Allow going to next scene.
			sceneLoading.allowSceneActivation = true;
		}

		internal static void OnSceneBegin (Scene scene, LoadSceneMode mode) {
			//Unpause rendering by resetting FPS to -1 (== native fps).
			//@TODO: This feature doesn't exist until 2019.3.

#if CLOUDS_TRANSITIONS
			//Perform scene-start transition.
			TransitionMaster.DoTransition(
				nextSceneTransitionIn,
				false
			);

			justFinishedTransition = false;
#endif

			//Have the scene preprocessor try to find our spawn point.
			//@TODO: Eventize this. Maybe move the call into PopTo?
			Vector3 position = ScenePreprocessor.FindSpawnPoint(scene, mode);

			//For the sake of efficiency, clear the scene loading reference.
			sceneLoading = null;

			SceneManager.sceneLoaded -= OnSceneBegin;
		}

	}
}