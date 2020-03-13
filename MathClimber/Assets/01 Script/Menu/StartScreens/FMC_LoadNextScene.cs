using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FMC_LoadNextScene : MonoBehaviour
{

    public bool isButtonScript;
    public bool isSplashScreen;
    public float waitingTime = 0.5f;
    public string nextSceneName = "";

    private void Awake()
    {
        if (!isButtonScript)
            LeanTween.delayedCall(waitingTime, loadScene);
    }

	private void Update (){
		if (Input.GetKeyDown (KeyCode.Space)) {
			//loadSceneByClick ();
			LeanTween.cancelAll();
			SceneManager.LoadScene(nextSceneName);
		}
	}

    private void loadScene ()
    {
        if (FLS_LoadingScreen.instance && !isSplashScreen)
            FLS_LoadingScreen.instance.loadScene(nextSceneName);
        else
        {
            Debug.LogWarning("No Loading Screen instance available.");
            SceneManager.LoadScene(nextSceneName);
        }
    }

    public void loadSceneByClick ()
    {
        if (FLS_LoadingScreen.instance)
            FLS_LoadingScreen.instance.loadScene(nextSceneName);
        else
        {
            Debug.LogWarning("No Loading Screen instance available.");
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
