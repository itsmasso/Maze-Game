using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager instance {get; private set;}
	public int streak;
	public void Awake() {
		streak = 0;
		if(instance != null)
		{
			Destroy(gameObject);
			return;
		}
		
		instance = this;
		DontDestroyOnLoad(gameObject);
	}
	
	public void IncrementStreak()
	{
		streak++;
	}

	public void ReloadScene()
	{
		StartCoroutine(LoadSceneAsync(SceneManager.GetActiveScene().name));
	}
	private IEnumerator LoadSceneAsync(string sceneName)
	{
		
		var scene = SceneManager.LoadSceneAsync(sceneName);
		AsyncOperation loadOperation = scene;
		
		while (!loadOperation.isDone)
		{
			//add progress bar here if needed
			yield return null;
		}
		
	}
}
