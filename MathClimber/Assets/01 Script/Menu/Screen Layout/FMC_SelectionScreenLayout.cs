using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR 
using UnityEditor;
#endif

public class FMC_SelectionScreenLayout : MonoBehaviour
{

    public GameObject background;
    public FMC_StoryModeBoxLayout storyModeBox;
    public FMC_PracticeBoxLayout practiceModeBox;
    public FMC_TalentsBoxLayout talentsBox;
    public FMC_LearningProgressLayout learningProgressBox;
    public FMC_GameProgressBoxLayout gameProgressBox;
    public FMC_StatisticBoxHiderLayout statisticBoxHider;
    public FMC_ShowPastLayout showPastLayout;
    public Canvas coinsAndLevels;
    public Transform scrollingBackground;
    public Transform lastObject;

    private float cameraHeight;
    private float cameraWidth;
    private Vector2 cameraPosition;

    private void Awake ()
    {
        setLayout();
        SceneManager.sceneLoaded += sceneWasLoaded;
        resetAllStatisticsData();
    }

    private void OnEnable ()
    {
        if (coinsAndLevels)
            coinsAndLevels.worldCamera = Camera.main;
    }

    public void setLayout()
    {
        cameraHeight = Camera.main.orthographicSize * 2.0f;
        cameraWidth = cameraHeight * Camera.main.aspect;
        cameraPosition = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y);

        if (background)
            background.transform.localScale = new Vector3(cameraWidth, background.transform.localScale.y, 1.0f);

        if (storyModeBox)
            storyModeBox.setLayout();

        if (practiceModeBox)
            practiceModeBox.setLayout();

        if (talentsBox)
            talentsBox.setLayout();

        if (learningProgressBox)
            learningProgressBox.setLayout();

        if (gameProgressBox)
            gameProgressBox.setLayout();

        if (statisticBoxHider)
            statisticBoxHider.setLayout();

        if (showPastLayout)
            showPastLayout.setLayout();

    }

    private void sceneWasLoaded (Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Menu01")
        {
            resetAllStatisticsData();
        }
    }

    public void resetAllStatisticsData ()
    {
        if (storyModeBox)
            storyModeBox.renewStatisticsData();

        if (talentsBox)
            talentsBox.renewStatisticsData();

        if (learningProgressBox)
            learningProgressBox.renewStatisticsData();

        if (gameProgressBox)
            gameProgressBox.renewStatisticsData();

        if (showPastLayout)
            showPastLayout.renewStatisticsData();

        if (scrollingBackground && lastObject)
            scrollingBackground.localScale = new Vector3(scrollingBackground.localScale.x, (scrollingBackground.transform.position.y - lastObject.transform.position.y) + 1.5f, scrollingBackground.localScale.z);
    }

    //public void deletePlayerPrefs()
    //{
    //    PlayerPrefs.DeleteAll();
    //}

    public void setSpriteSortingLayers()
    {
        foreach(Transform t in transform.parent.gameObject.transform)
        {
            Debug.Log(t.gameObject.name);
            if (t.gameObject.name == "SelectionScreen")
            {
                setLayersRecursively("SelectionScreen", t.gameObject.transform);
            }
            if (t.gameObject.name == "Freestyle Settings")
            {
                setLayersRecursively("FreestyleSettings", t.gameObject.transform);
            }
            if (t.gameObject.name == "OneTimesOneSettings")
            {
                setLayersRecursively("OneTimesOneSettings", t.gameObject.transform);
            }
            if (t.gameObject.name == "In App Purchase")
            {
                setLayersRecursively("InAppPurchase", t.gameObject.transform);
            }
        }
    }

    private void setLayersRecursively(string layerName, Transform transform)
    {
        foreach(Transform t in transform)
        {
            if (t.gameObject.GetComponent<SpriteRenderer>())
            {
                t.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = layerName;
            }
            if (t.gameObject.GetComponent<Canvas>())
            {
                t.gameObject.GetComponent<Canvas>().sortingLayerName = layerName;
            }
            if (t.gameObject.GetComponent<MeshRenderer>())
            {
                t.gameObject.GetComponent<MeshRenderer>().sortingLayerName = layerName;
            }
            if (t.childCount > 0)
            {
                setLayersRecursively(layerName, t);
            }
        }
    }
    
}

#if UNITY_EDITOR 
[CustomEditor(typeof(FMC_SelectionScreenLayout))]
public class FMC_SelectionScreenEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        FMC_SelectionScreenLayout myScript = (FMC_SelectionScreenLayout)target;
        if (GUILayout.Button("Set layout"))
        {
            myScript.setLayout();
        }
        if (GUILayout.Button("Set Sprite Layers"))
        {
            myScript.setSpriteSortingLayers();
        }
    }
}
#endif