using UnityEngine;
using UnityEngine.SceneManagement;

namespace Clouds.SceneManagement {
	internal static class SceneChangerInternal {
		public static AsyncOperation GoToScene (string path) {
			//SceneManager.sceneLoaded += OnSceneBegin;

			AsyncOperation returner = SceneManager.LoadSceneAsync(path, LoadSceneMode.Single);
			returner.allowSceneActivation = false;
//			Debug.Log("Scene async operation is set to activate? " + returner.allowSceneActivation);

			return returner;
		}
	}
		
}