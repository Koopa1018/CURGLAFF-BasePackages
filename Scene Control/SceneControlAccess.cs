using UnityEngine;
using UnityEngine.SceneManagement;
#if CLOUDS_TRANSITIONS
using Clouds.Transitions;
#endif

namespace Clouds.SceneManagement {
	[AddComponentMenu("Game Flow/Scene Control Access")]
	public sealed class SceneControlAccess : MonoBehaviour {

		[UnityEngine.Serialization.FormerlySerializedAs("targetScene")]
		[SerializeField] SceneField _targetScene;
		[SerializeField] string targetSpawnerName;
#if CLOUDS_TRANSITIONS
		[SerializeField] Transition transitionAnimation;
		[SerializeField] public Transform irisCenter;
		//[SerializeField] float transitionDuration = 1;
#endif

		public string targetScene {
			get {
				return _targetScene;
			} set {
				_targetScene = value;
			}
		}

		public void GoToNewScene () {
			SceneChanger.GotoScene(
				_targetScene.SceneName,
				targetSpawnerName
#if CLOUDS_TRANSITIONS
				, new TransitionSettings(
					transitionAnimation, 
					irisCenter ? irisCenter.position : Vector3.zero
				)
#endif
			);
		}

	}
}