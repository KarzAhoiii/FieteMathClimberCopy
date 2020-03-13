using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FMC_SessionButton : MonoBehaviour,
IPointerDownHandler,
IPointerUpHandler,
IPointerExitHandler
{

    public FMC_ShowPastLayout showPastLayout;

    public SpriteRenderer selfSpriteRenderer;
    public BoxCollider2D selfBoxCollider;

    public GameObject content;
    public GameObject buttonArrow;
    public GameObject bubbleRightPrefab;
    public GameObject bubbleWrongPrefab;
    public GameObject bubbleNeutralPrefab;
    public GameObject tasksPrefab;
    public GameObject numbersPrefab;
    public SpriteRenderer buttonFace;
    public SpriteRenderer topLine;
    public SpriteRenderer bottomLine;

    public Transform unfoldButtonParent;
    public Transform pastTasksParent;
    public Transform talentsParent;
    public Transform problemsParent;
    public Transform statisticsParent;
    public Transform bubbleParent;
    public Transform tasksParent;
    public Transform tasksInformationParent;
    public Transform numbersParent;

    public Text sessionID;
    public Text answeredTasks;
    public Text talentsRecognized;
    public Text talents;
    public Text problemsRecognized;
    public Text problems;
    public Text statisticsTags;
    public Text statisticsValues;

    private bool clickPossible = false;
    private bool isUnfolded = false;
    private bool isInitialised = false;
    private bool isIphoneX = false;

    private float tasksTextHeight;
    private float talentsTextHeight;
    private float problemsTextHeight;
    private float statisticsTextHeight;

    private float fullBoxSize;
    private float distanceBetweenButtons = 0.0f;
    private float distanceBetweenBubbles = 1.0f;

    private string jumpedText;

    private Text tasks;
    private Text taskInformations;
    private Text numbers;

    private FMC_Statistics.TalentsProblems currentTalentsAndProblems;
    private List<string> currentStatisticTags = new List<string>();
    private List<string> currentStatisticValues = new List<string>();

    //private void Awake ()
    //{
    //    initialise();
    //}

    public void initialise()
    {
        moveButtonUp();

        tasksTextHeight = /*JF_Utility.calculateTextHeightInWorldSpace(tasks);*/ 0.432f;
        talentsTextHeight = JF_Utility.calculateTextHeightInWorldSpace(talents);
        problemsTextHeight = JF_Utility.calculateTextHeightInWorldSpace(problems);
        statisticsTextHeight = JF_Utility.calculateTextHeightInWorldSpace(statisticsValues);
        //-0.132

        //Debug.LogWarning(JF_Utility.calculateLineHeight(tasks) * tasks.transform.localScale.x);
        if ((float)Screen.width / (float)Screen.height < 0.5f)
            isIphoneX = true;

        jumpedText = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.statistics][25];
        if (isIphoneX && jumpedText.Length > 6)
            jumpedText = jumpedText.Substring(0, 6) + ".";

        isInitialised = true;
        //resetStatisticsData();
    }

    public bool getIsUnfolded ()
    {
        return isUnfolded;
    }

    public void setLayout(float cameraHeight, float cameraWidth, Vector3 cameraPosition)
    {
        if (topLine)
            topLine.size = new Vector2(cameraWidth * 0.9f, topLine.size.y);

        if (unfoldButtonParent)
            unfoldButtonParent.transform.position = new Vector3(topLine.transform.position.x + (topLine.size.x * 0.5f) - buttonFace.bounds.extents.x - 0.15f, unfoldButtonParent.transform.position.y, unfoldButtonParent.transform.position.z);

        if (selfSpriteRenderer)
            selfSpriteRenderer.size = new Vector2(cameraWidth * 0.9f, selfSpriteRenderer.size.y);

        if (selfBoxCollider)
            selfBoxCollider.size = new Vector2(cameraWidth * 0.9f, selfBoxCollider.size.y);

        if (sessionID)
            sessionID.transform.position = new Vector3(topLine.transform.position.x - (topLine.size.x * 0.5f) + 0.15f, sessionID.transform.position.y, sessionID.transform.position.z);

        if (numbersParent)
            numbersParent.position = new Vector3(sessionID.transform.position.x, numbersParent.position.y, numbersParent.position.z);

        if (answeredTasks)
            answeredTasks.transform.position = new Vector3(numbersParent.position.x, answeredTasks.transform.position.y, answeredTasks.transform.position.z);
            answeredTasks.text = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.statistics][12]+":";

        if (bubbleParent)
            bubbleParent.transform.position = new Vector3(numbersParent.position.x + 0.8f, bubbleParent.transform.position.y, bubbleParent.transform.position.z);

        if (tasksParent)
            tasksParent.position = new Vector3(numbersParent.position.x + 1.1f, tasksParent.position.y, tasksParent.position.z);

        if (tasksInformationParent)
            tasksInformationParent.transform.position = new Vector3(unfoldButtonParent.position.x + buttonFace.bounds.extents.x, tasksInformationParent.position.y, tasksInformationParent.position.z);

        if (talentsRecognized)
            talentsRecognized.transform.position = new Vector3(numbersParent.position.x, talentsRecognized.transform.position.y, talentsRecognized.transform.position.z);
            talentsRecognized.text = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.statistics][48]+":";

        if (talents)
            talents.transform.position = new Vector3(numbersParent.position.x, talents.transform.position.y, talents.transform.position.z);

        if (problemsRecognized)
            problemsRecognized.transform.position = new Vector3(numbersParent.position.x, problemsRecognized.transform.position.y, problemsRecognized.transform.position.z);
            problemsRecognized.text = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.statistics][49]+":";

        if (problems)
            problems.transform.position = new Vector3(numbersParent.position.x, problems.transform.position.y, problems.transform.position.z);

        if (statisticsTags)
            statisticsTags.transform.position = new Vector3(numbersParent.position.x, statisticsTags.transform.position.y, statisticsTags.transform.position.z);

        if (statisticsValues)
            statisticsValues.transform.position = new Vector3(tasksInformationParent.position.x, statisticsValues.transform.position.y, statisticsValues.transform.position.z);

        if (numbersParent)
            numbersParent.position = new Vector3(numbersParent.position.x + 0.45f, numbersParent.position.y, numbersParent.position.z);
    }

    public float getNextButtonPosition()
    {
        if (isUnfolded)
            return transform.position.y - ((selfSpriteRenderer.size.y * 0.5f) - distanceBetweenButtons) - fullBoxSize;
        else
            return transform.position.y - selfSpriteRenderer.size.y - distanceBetweenButtons;
    }

    public GameObject indicator;

    public void resetStatisticsData(FMC_Statistics.Statistics.DataPerDay sessionData)
    {

        if (!isInitialised)
            initialise();

        for (int i = 0; i < bubbleParent.childCount; i++)
            Destroy(bubbleParent.GetChild(i).gameObject);

        for (int i = 0; i < numbersParent.childCount; i++)
            Destroy(numbersParent.GetChild(i).gameObject);

        for (int i = 0; i < tasksParent.childCount; i++)
            Destroy(tasksParent.GetChild(i).gameObject);

        for (int i = 0; i < tasksInformationParent.childCount; i++)
            Destroy(tasksInformationParent.GetChild(i).gameObject);

        talents.text = "";
        problems.text = "";
        statisticsTags.text = "";
        statisticsValues.text = "";
        currentStatisticTags.Clear();
        currentStatisticValues.Clear();

        tasks = GameObject.Instantiate(tasksPrefab).GetComponent<Text>();
        tasks.transform.SetParent(tasksParent, false);
        tasks.transform.localPosition = new Vector3(0, 0, 0);
        taskInformations = GameObject.Instantiate(tasksPrefab).GetComponent<Text>();
        taskInformations.transform.SetParent(tasksInformationParent, false);
        taskInformations.transform.localPosition = new Vector3(0, 0, 0);
        taskInformations.alignment = TextAnchor.UpperRight;
        numbers = GameObject.Instantiate(numbersPrefab).GetComponent<Text>();
        numbers.transform.SetParent(numbersParent, false);
        numbers.transform.localPosition = new Vector3(0, 0, 0);

        currentTalentsAndProblems = FMC_Statistics.Statistics.getTalentsAndProblems(sessionData.dailyTalentProblemData);
        currentStatisticTags = sessionData.getStatisticsTags();
        currentStatisticValues = sessionData.getStatisticsValues();

        int tasksEntries = sessionData.allSessionTasks.Count;
        int talentsEntries = currentTalentsAndProblems.talents.Count;
        int problemsEntries = currentTalentsAndProblems.problems.Count;
        int statisticEntries = currentStatisticValues.Count;

        float totalTasksHeight = tasksTextHeight * ((tasksEntries/* * 12*/) + 2);
        float totalTasksHeight02 = tasksTextHeight * (tasksEntries);
        float totaltalentsHeight = talentsTextHeight * (talentsEntries + 2);
        float totalProblemsHeight = problemsTextHeight * (problemsEntries + 2);
        float totalStatisticsHeight = statisticsTextHeight * (statisticEntries + 2);

        talentsParent.transform.position = new Vector3(talentsParent.transform.position.x, pastTasksParent.transform.position.y - totalTasksHeight, talentsParent.transform.position.z);
        problemsParent.transform.position = new Vector3(problemsParent.transform.position.x, talentsParent.transform.position.y - totaltalentsHeight, problemsParent.transform.position.z);
        statisticsParent.transform.position = new Vector3(statisticsParent.transform.position.x, problemsParent.transform.position.y - totalProblemsHeight, statisticsParent.transform.position.z);
        fullBoxSize = (transform.position.y - (selfSpriteRenderer.size.y * 0.5f)) - (statisticsParent.transform.position.y - totalStatisticsHeight);

        //if (indicator)
        //    indicator.transform.Translate(0, -totalTasksHeight02, 0);

        float currentYPosition = 0;
        GameObject bubble;
        int counter = 0;

        for (int i = 0; i < tasksEntries; i++)
        {
            bool correct = (sessionData.allSessionTasks[i].answeredCorrectly && !sessionData.allSessionTasks[i].answeredUsingBomb);
            bool wrong = (!sessionData.allSessionTasks[i].answeredCorrectly && !sessionData.allSessionTasks[i].answeredUsingBomb);

            // Create Bubbles
            if (correct)
            {
                bubble = GameObject.Instantiate(bubbleRightPrefab);
                bubble.transform.parent = bubbleParent;
                bubble.transform.localPosition = new Vector3(0, -(currentYPosition/* + (tasksTextHeight * 0.5f)*/), 0);
            }
            else if (wrong)
            {
                bubble = GameObject.Instantiate(bubbleWrongPrefab);
                bubble.transform.parent = bubbleParent;
                bubble.transform.localPosition = new Vector3(0, -(currentYPosition/* + (tasksTextHeight * 0.5f)*/), 0);
            }
            else
            {
                bubble = GameObject.Instantiate(bubbleNeutralPrefab);
                bubble.transform.parent = bubbleParent;
                bubble.transform.localPosition = new Vector3(0, -(currentYPosition/* + (tasksTextHeight * 0.5f)*/), 0);
            }

            // Fill Tasks and Numbers  
            if (counter > 30)
            {
                tasks = GameObject.Instantiate(tasksPrefab).GetComponent<Text>();
                tasks.transform.SetParent(tasksParent, false);
                tasks.transform.localPosition = new Vector3(0, -currentYPosition, 0);

                taskInformations = GameObject.Instantiate(tasksPrefab).GetComponent<Text>();
                taskInformations.transform.SetParent(tasksInformationParent, false);
                taskInformations.transform.localPosition = new Vector3(0, -currentYPosition, 0);
                taskInformations.alignment = TextAnchor.UpperRight;

                numbers = GameObject.Instantiate(numbersPrefab).GetComponent<Text>();
                numbers.transform.SetParent(numbersParent, false);
                numbers.transform.localPosition = new Vector3(0, -currentYPosition, 0);

                counter = 0;
            }

            // Create Numbers
            numbers.text += (i + 1) + "\n";

            if (correct)
                tasks.text += "<color=#498e31>";
            else if (wrong)
                tasks.text += "<color=#e50000>";
            tasks.text += sessionData.allSessionTasks[i].task +" = " +sessionData.allSessionTasks[i].userAnswer+ "\n";
            if (correct || wrong)
                tasks.text += "</color>";
                
     

            if (correct)
                taskInformations.text += "<color=#000000>" + sessionData.allSessionTasks[i].timeNeeded.ToString("F1")+"s</color>\n";
            else if (wrong)
                taskInformations.text += "<color=#000000>" + sessionData.allSessionTasks[i].timeNeeded.ToString("F1")+"s</color>\n";
            else if (sessionData.allSessionTasks[i].answeredUsingBomb)
                taskInformations.text += "(" + jumpedText + ")\n";

            currentYPosition += tasksTextHeight;
            counter++;
        }

        for (int i = 0; i < talentsEntries; i++)
        {
            talents.text += currentTalentsAndProblems.talents[i];
        }

        for (int i = 0; i < problemsEntries; i++)
        {
            problems.text += currentTalentsAndProblems.problems[i];
        }

        for (int i = 0; i < statisticEntries; i++)
        {
            statisticsTags.text += currentStatisticTags[i] + "\n";
            statisticsValues.text += currentStatisticValues[i] + "\n";
        }

    }

    public void OnPointerDown(PointerEventData evd)
    {
        clickPossible = true;
        moveButtonDown();
    }

    public void OnPointerUp(PointerEventData evd)
    {
        if (clickPossible)
        {
            startAction();
        }
        moveButtonUp();
    }

    public void OnPointerExit(PointerEventData evd)
    {
        clickPossible = false;
        moveButtonUp();
    }

    public void startAction()
    {
        if (FMC_GameDataController.instance)
            LeanAudio.play(FMC_GameDataController.instance.buttonClickSound, FMC_GameDataController.instance.buttonClickVolume);

        if (!isUnfolded)
            unfold();
        else
            collapse();
    }

    private void unfold()
    {
        isUnfolded = true;
        buttonArrow.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -90));
        content.SetActive(true);
        showPastLayout.clickedSessionsButton();
    }

    private void collapse()
    {
        isUnfolded = false;
        buttonArrow.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));
        content.SetActive(false);
        showPastLayout.clickedSessionsButton();

    }

    private void moveButtonDown()
    {
        LeanTween.moveLocalY(buttonFace.gameObject, 0, 0.1f);
    }

    private void moveButtonUp()
    {
        LeanTween.moveLocalY(buttonFace.gameObject, 0.1f, 0.1f);
    }

}
