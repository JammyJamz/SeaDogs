using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : Monobehavior
{
	public int index;
	[SerializeField]
	private Image loadingBar;	

	IEnumerator LoadAsyncOperation()
	{
		AsyncOperation gameLevel = SceneManager.loadSceneAsync(index);
		while (gameLevel.progress < 1)
		{
			loadingBar.fillAmount = gameLevel.progress;
			yield return new WaitForEndOfFrame();
		}
	}
}
