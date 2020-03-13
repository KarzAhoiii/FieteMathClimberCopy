using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FLS_LoadingScreen : MonoBehaviour
{

    public static FLS_LoadingScreen instance;

    public Camera pinholeCamera;
    public SpriteRenderer pinholeSprite;
    public Canvas Canvas;
    public RectTransform loadingBarParent;
    public Image loadingBarImage;
    [Range (0.0f, 1.0f)] public float pinholeAnimationTime;
    public AnimationCurve pinholeClose;
    public AnimationCurve pinholeOpen;
    public AudioClip pinholeSwoosh;

    private float cameraHeight;
    private float cameraWidth;
    private Vector2 cameraPosition;
    private Material pinholeSpriteMaterial;
    private string sceneToLoad;
    private AsyncOperation async = null;
    private float openingPercentage;

    private void Awake ()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }

        setLayout();
        pinholeCamera.gameObject.SetActive(true);
        pinholeSprite.gameObject.SetActive(true);
        Canvas.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    public void setLayout ()
    {
        if (pinholeCamera)
        {
            cameraHeight = pinholeCamera.orthographicSize * 2.0f;
            cameraWidth = cameraHeight * Camera.main.aspect;
            cameraPosition = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y);
        }

        if (pinholeSprite)
        {
            pinholeSprite.size = new Vector2(cameraWidth, cameraHeight);
           // pinholeSprite.transform.position = new Vector3(cameraPosition.x, cameraPosition.y, pinholeSprite.transform.position.z);
            pinholeSpriteMaterial = pinholeSprite.sharedMaterial;
            pinholeSpriteMaterial.SetFloat("_maximumDistance", Mathf.Sqrt(Mathf.Pow(cameraWidth * 0.5f, 2) + Mathf.Pow(cameraHeight * 0.5f, 2)));
        }
        
    }

    public void loadScene (string sceneName)
    {
        gameObject.SetActive(true);
        pinholeSpriteMaterial.SetFloat("_openingPercentage", 1.0f);
        loadingBarImage.fillAmount = 0.0f;
        loadingBarParent.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
        sceneToLoad = sceneName;
        LeanTween.value(gameObject, tweenPinhole, 1.0f, 0.0f, pinholeAnimationTime).setEase(pinholeClose).setOnComplete(openLoadingBar);
        if (pinholeSwoosh)
            LeanAudio.play(pinholeSwoosh, 0.15f);
    }

    private void tweenPinhole (float value)
    {
        pinholeSpriteMaterial.SetFloat("_openingPercentage", value);
    }

    private void openLoadingBar ()
    {
        LeanTween.scale(loadingBarParent, new Vector3(1.0f, 1.0f, 1.0f), 0.3f).setOnComplete(startLoadingScene);
    }

    private void startLoadingScene ()
    {
        async = SceneManager.LoadSceneAsync(sceneToLoad);
    }

    private void closeLoadingBar ()
    {
        LeanTween.scale(loadingBarParent, new Vector3(0.0f, 0.0f, 0.0f), 0.3f).setOnComplete(openPinhole);
    }

    private void openPinhole ()
    {
        LeanTween.value(gameObject, tweenPinhole, 0.0f, 1.0f, pinholeAnimationTime).setEase(pinholeOpen).setOnComplete(resetLoadingScreen);
        if (pinholeSwoosh)
            LeanAudio.play(pinholeSwoosh, 0.15f);
    }

    private void resetLoadingScreen ()
    {
        gameObject.SetActive(false);
    }

    private void Update ()
    {
        if (async != null)
        {
            loadingBarImage.fillAmount = async.progress;
            if (async.progress == 1.0f)
            {
                async = null;
                LeanTween.delayedCall(0.1f, closeLoadingBar);
            }
        }
    }

}
