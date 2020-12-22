using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Clouds.SceneManagement;

[DefaultExecutionOrder(-100)]
public class PopToSpawnPoint : MonoBehaviour {
	// Use this for initialization
	void Start () {
		if (ScenePreprocessor.SpawnPointIsValid) {
			transform.position = ScenePreprocessor.SelectedSpawnPoint;
		}

		Destroy(this);
	}
}
