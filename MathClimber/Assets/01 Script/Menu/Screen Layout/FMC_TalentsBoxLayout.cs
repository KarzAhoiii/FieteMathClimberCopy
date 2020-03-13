using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FMC_TalentsBoxLayout : MonoBehaviour
{

    public SpriteRenderer lineTop;
    public RectTransform talentsHeader;
    public RectTransform problemsHeader;
    public RectTransform talentsBox;
    public RectTransform problemsBox;
    public Text statisticsHeader;
    public Text TalenteText;
    public Text ProblemeText;
    public Transform dynamicallyMoving;
    public Transform customExercises;
    public GameObject customButtonPrefab;
    public List<Transform> exercicesParents;
    public FMC_PracticeBoxLayout practiceBoxLayout;
    public FMC_MenuController menuController;
    public float originialCustomExercisePosY;
    public float originaldynamicMovingPosY;
    public Transform iapButton;
    public Transform iapButtonPositionX;

    private FMC_Statistics.TalentsProblems talentsProblems;

    private float cameraHeight;
    private float cameraWidth;
    private Vector2 cameraPosition;
    private List<GameObject> allCustomExercises = new List<GameObject>();

    public void setLayout()
    {
        cameraHeight = Camera.main.orthographicSize * 2.0f;
        cameraWidth = cameraHeight * Camera.main.aspect;
        cameraPosition = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y);

        if (lineTop)
            lineTop.size = new Vector2(cameraWidth * 0.9f, lineTop.size.y);

        if (talentsHeader)
            talentsHeader.position = new Vector3(cameraPosition.x - (cameraWidth * 0.5f) + (cameraWidth * 0.055f), talentsHeader.position.y, talentsBox.position.z);

        if (problemsHeader)
            problemsHeader.position = new Vector3(Mathf.Abs(talentsHeader.position.x), problemsHeader.position.y, problemsHeader.position.z);

        if (talentsBox)
            talentsBox.position = new Vector3(cameraPosition.x - (cameraWidth * 0.5f) + (cameraWidth * 0.055f), talentsBox.position.y, talentsBox.position.z);

        if (problemsBox)
            problemsBox.position = new Vector3(Mathf.Abs(talentsBox.position.x), problemsBox.position.y, problemsBox.position.z);

        if (FMC_GameDataController.instance)
        {
            statisticsHeader.text = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.statistics][4];
            TalenteText.text = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.statistics][5];
            ProblemeText.text = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.statistics][6];
        }
    }

    public void renewStatisticsData()
    {
        Text text = null;

        FMC_Statistics.Statistics statistics = FMC_GameDataController.instance.getCurrentStatistics();
        talentsProblems = FMC_Statistics.Statistics.getTalentsAndProblems(statistics.dailyData[0].dailyTalentProblemData);

        //if (FMC_GameDataController.instance)
        //    talentsProblems = FMC_GameDataController.instance.getTalentProblemData();

        if (talentsBox)
        {
            text = talentsBox.GetComponent<Text>();
            text.text = "";
            if (text)
                foreach (string s in talentsProblems.talents)
                    text.text += s;
        }

        text = null;
        if (problemsBox)
        {
            problemsBox.position = new Vector3(Mathf.Abs(talentsBox.position.x), problemsBox.position.y, problemsBox.position.z);
            text = problemsBox.GetComponent<Text>();
            text.text = "";
            if (text)
                foreach (string s in talentsProblems.problems)
                    text.text += s;
        }

        // Create Custom Exercises etc.
        foreach (GameObject g in allCustomExercises)
            Destroy(g);

        allCustomExercises.Clear();

        int counterText = talentsProblems.talentsInfo.Count > talentsProblems.problemsInfo.Count ? talentsProblems.talentsInfo.Count : talentsProblems.problemsInfo.Count;

        if (talentsProblems.problemsInfo.Count == 0)
            customExercises.gameObject.SetActive(false);
        else
            customExercises.gameObject.SetActive(true);

        if (counterText < 1)
            counterText = 1;

        float textSizeY = 0.45f;
        float textPadding = (counterText - 1) * textSizeY;

        if (talentsProblems.problemsInfo.Count == 0)
            dynamicallyMoving.transform.localPosition = new Vector3(dynamicallyMoving.transform.localPosition.x, originaldynamicMovingPosY - textPadding, dynamicallyMoving.transform.localPosition.z);
        else
            customExercises.transform.localPosition = new Vector3(customExercises.transform.localPosition.x, originialCustomExercisePosY - textPadding, customExercises.localPosition.z);

        int counterProblems = talentsProblems.problemsInfo.Count;

        GameObject go;
        FMC_PracticeButton practiceButton;
        for (int i = 0; i < counterProblems; i++)
        {
            go = GameObject.Instantiate(customButtonPrefab);
            go.transform.parent = exercicesParents[i];
            go.transform.localPosition = new Vector3(0, 0, 0);
            go.name = "Custom Exercise " + i;
            allCustomExercises.Add(go);
            practiceButton = go.GetComponent<FMC_PracticeButton>();
            practiceButton.text.text = talentsProblems.problems[i];
            practiceButton.initialiseCustomExercise(practiceBoxLayout, menuController, talentsProblems.problemsInfo[i]);
            practiceButton.setLayout(cameraWidth, cameraPosition);
        }

        if (allCustomExercises.Count > 0)
        {
            dynamicallyMoving.transform.position = new Vector3(dynamicallyMoving.transform.position.x, allCustomExercises[allCustomExercises.Count - 1].transform.position.y - 1.0f, dynamicallyMoving.transform.position.z);

            if (iapButton && !FMC_GameDataController.instance.subscriptionIsActive())
            {
                iapButton.gameObject.SetActive(true);
                iapButton.transform.position = new Vector3(iapButtonPositionX.position.x, Mathf.Lerp(allCustomExercises[0].transform.position.y, allCustomExercises[allCustomExercises.Count - 1].transform.position.y, 0.5f), iapButton.position.z);
            }
        }
        else if (iapButton)
        {
            iapButton.gameObject.SetActive(false);
        }
    }

}
