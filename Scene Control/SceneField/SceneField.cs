#pragma warning disable 0219 //m_SceneAsset is used only by the Editor, so compiler doesn't see its use.

using UnityEngine;

/// <summary>
/// A field which takes a Scene asset and turns it into a usable string.
/// </summary>
/// <source>https://answers.unity.com/questions/242794/inspector-field-for-scene-asset.html</source>
[System.Serializable]
public struct SceneField
{
#if UNITY_EDITOR
	[SerializeField]
	private Object m_SceneAsset; //Used by the Editor, don't remove~
#endif
	[SerializeField]
	private string m_SceneName;
	public string SceneName
	{
		get { return m_SceneName; }
	}
	// makes it work with the existing Unity methods (LoadLevel/LoadScene)
	public static implicit operator string( SceneField sceneField )
	{
		return sceneField.SceneName;
	}

	public static implicit operator SceneField( string s) {
		return new SceneField{
#if UNITY_EDITOR
			m_SceneAsset = null,
#endif
			m_SceneName = s
		};
	}
}