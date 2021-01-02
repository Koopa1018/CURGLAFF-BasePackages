using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Clouds.SceneManagement;

/// <summary>
/// On Awake(), assigns the current active scene to the value of a detected SceneControlAccess.
/// Useful for making reset buttons, and probably not much else.
/// </summary>
public class AssignCurrentSceneOnAwake : MonoBehaviour {
	//[SerializeField] SceneControlAccess target; //Nope--more convenient for users to add this and let it find SceneControlAccess on its own.

    // Start is called before the first frame update
    void Awake() {
        GetComponent<SceneControlAccess>().targetScene = SceneManager.GetActiveScene().name;
		Destroy(this);
    }
}
