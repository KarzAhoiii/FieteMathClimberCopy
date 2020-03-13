using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FMC_LearningProgressLayout : MonoBehaviour
{

    public List<SpriteRenderer> graphBarsFront;
    public List<SpriteRenderer> graphBarsBack;
    public List<Text> bottomTexts01;
    public List<Text> bottomTexts02;
    public List<Text> months;
    public SpriteRenderer lineTop;
    public RectTransform textHeader;
    public Text textHeaderText;
    public SpriteRenderer background;
    public SpriteRenderer lineBgTop;
    public SpriteRenderer lineBgBottom;
    public GameObject iterateButton;

    private float cameraHeight;
    private float cameraWidth;
    private Vector2 cameraPosition;
    private List<int> tasksAtDay;

    private enum possibleDataShown { correctlySolved, correctInPercent, timePerTask };
    private possibleDataShown currentDataShown = possibleDataShown.correctlySolved;

    public void setLayout ()
    {
        cameraHeight = Camera.main.orthographicSize * 2.0f;
        cameraWidth = cameraHeight * Camera.main.aspect;
        cameraPosition = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y);

        if (lineTop)
            lineTop.size = new Vector2(cameraWidth * 0.9f, lineTop.size.y);

        //if (textHeader)
        //    textHeader.position = new Vector3(cameraPosition.x - (cameraWidth * 0.5f) + (cameraWidth * 0.055f), textHeader.position.y, textHeader.position.z);

        if (textHeaderText && FMC_GameDataController.instance)
            textHeaderText.text = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.statistics][7];

        if (background)
            background.size = new Vector2(cameraWidth * 0.9f, background.size.y);

        if (lineBgTop)
            lineBgTop.size = new Vector2(background.size.x, lineBgTop.size.y);

        if (lineBgBottom)
            lineBgBottom.size = new Vector2(background.size.x, lineBgBottom.size.y);



        float barSpacing = (cameraWidth * 0.9f) / graphBarsFront.Count;
        float rectangleWidth = background.size.x;
        float graphbarsWidth = graphBarsFront[0].size.x;
        float x = background.transform.position.x - (rectangleWidth * 0.5f);
        float y = background.transform.position.x + (rectangleWidth * 0.5f) - graphbarsWidth;

        for (int i = 0; i < graphBarsFront.Count; i++)
        {
            float xPos = JF_Utility.convertRangeClamped(0, graphBarsFront.Count - 1, x, y, i);
            graphBarsFront[i].transform.position = new Vector3(xPos, graphBarsFront[i].transform.position.y, graphBarsFront[i].transform.position.z);
        }

        for (int i = 0; i < graphBarsBack.Count; i++)
        {
            graphBarsBack[i].transform.position = new Vector3(graphBarsFront[i].transform.position.x, graphBarsBack[i].transform.position.y, graphBarsBack[i].transform.position.z);
        }

        for (int i = 0; i < bottomTexts01.Count; i++)
        {
            bottomTexts01[i].transform.parent.transform.position = new Vector3(graphBarsFront[i].transform.position.x + (graphbarsWidth* 0.5f), bottomTexts01[i].transform.parent.transform.position.y, bottomTexts01[i].transform.parent.transform.position.z);
        }

        for (int i = 0; i < bottomTexts02.Count; i++)
        {
            bottomTexts02[i].transform.parent.transform.position = new Vector3(graphBarsFront[i].transform.position.x + (graphbarsWidth * 0.5f), bottomTexts02[i].transform.parent.transform.position.y, bottomTexts02[i].transform.parent.transform.position.z);
        }

        for (int i = 0; i < months.Count; i++)
        {
            months[i].transform.parent.transform.position = new Vector3(graphBarsFront[i].transform.position.x + (graphbarsWidth * 0.5f), months[i].transform.parent.transform.position.y, months[i].transform.parent.transform.position.z);
        }

        if (iterateButton)
            iterateButton.transform.position = new Vector3(months[months.Count-1].transform.parent.position.x, iterateButton.transform.position.y, iterateButton.transform.position.z); // 0.3441 == Button Size
    }

    public void iterateStatisticsData()
    {

        switch (currentDataShown)
        {
            case possibleDataShown.correctlySolved:
                currentDataShown = possibleDataShown.correctInPercent;
                break;
            case possibleDataShown.correctInPercent:
                currentDataShown = possibleDataShown.timePerTask;
                break;
            case possibleDataShown.timePerTask:
                currentDataShown = possibleDataShown.correctlySolved;
                break;
            default:
                Debug.LogWarning("Define this Enum Value in the switch Statement!");
                break;
        }

        renewStatisticsData();
    }

    public void renewStatisticsData()
    {
        FMC_Statistics.Statistics statistics = FMC_GameDataController.instance.getCurrentStatistics();

        for (int i = months.Count - 1, j = 0; i >= 0; i--, j++)
        {
            if (statistics.dailyData.Count > j)
            {
                months[i].text = statistics.dailyData[j].date.ToString("dd") + "\n" + statistics.dailyData[j].date.ToString("MM");
            }
            else
                months[i].text = "00\n00";
        }

        if (currentDataShown == possibleDataShown.correctlySolved)
            setBarsForCorrectlySolved();
        else if (currentDataShown == possibleDataShown.correctInPercent)
            setBarsForCorrectInPercent();
        else if (currentDataShown == possibleDataShown.timePerTask)
            setBarsForTimePerTask();

    }

    private void setBarsForCorrectlySolved()
    {
        FMC_Statistics.Statistics statistics = FMC_GameDataController.instance.getCurrentStatistics();

        if (FMC_GameDataController.instance && FMC_GameDataController.instance.subscriptionIsActive() && statistics.dailyData.Count > 0)
        {
            //float highestCount = getHighestCount();
            float highestCount = getHighestCountBack();

            if (highestCount != 0)
            {
                for (int i = graphBarsFront.Count - 1, j = 0; i >= 0; i--, j++)
                {
                    float height = 0.0f;
                    float height02 = 0.0f;
                    if (statistics.dailyData.Count > j)
                    {
                        height = (statistics.dailyData[j].correctlyCalculatedTasks * 2.52f) / highestCount; // 2.52 == Maximale Ausdehnung in  Units
                        height02 = (statistics.dailyData[j].calculatedTasksTotalWithoutBombs * 2.52f) / highestCount;
                        bottomTexts01[i].text = statistics.dailyData[j].correctlyCalculatedTasks.ToString();
                        bottomTexts02[i].text = statistics.dailyData[j].calculatedTasksTotalWithoutBombs.ToString();
                    }
                    else // Nicht genutzte Balken nicht beschriften
                    {
                        bottomTexts01[i].text = "";
                        bottomTexts02[i].text = "";
                        height = 0;
                        height02 = 0;
                    }

                    height = Mathf.Clamp(height, 0.0f, 2.52f);
                    height02 = Mathf.Clamp(height02, 0.0f, 2.52f);
                    tweenGraphBars(i, height);
                    tweenGraphBarsBack(i, height02);
                }
            }
            else
                resetStatisticBars();
        }
        else
            resetStatisticBars();

        if (FMC_GameDataController.instance)
            textHeaderText.text = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.statistics][8];
        else
            textHeaderText.text = "Lösungen/Sitzung";
    }

    private void setBarsForCorrectInPercent()
    {
        FMC_Statistics.Statistics statistics = FMC_GameDataController.instance.getCurrentStatistics();

        if (FMC_GameDataController.instance && FMC_GameDataController.instance.subscriptionIsActive() && statistics.dailyData.Count > 0)
        {
            float highestCount = getHighestCount();

            if (highestCount != 0)
            {
                for (int i = graphBarsFront.Count - 1, j = 0; i >= 0; i--, j++)
                {
                    float height = 0.0f;
                    float height02 = 0.0f;
                    if (statistics.dailyData.Count > j)
                    {
                        height = (statistics.dailyData[j].correctlyCalculatedInPercent * 0.01f) * 2.52f; // 2.52 == Maximale Ausdehnung in  Units
                        height02 = 2.52f;
                        bottomTexts01[i].text = statistics.dailyData[j].correctlyCalculatedInPercent.ToString();
                        bottomTexts02[i].text = "100";
                    }
                    else // Nicht genutzte Balken nicht beschriften
                    {
                        bottomTexts01[i].text = "";
                        bottomTexts02[i].text = "";
                        height = 0;
                        height02 = 0;
                    }

                    height = Mathf.Clamp(height, 0.0f, 2.52f);
                    height02 = Mathf.Clamp(height02, 0.0f, 2.52f);
                    tweenGraphBars(i, height);
                    tweenGraphBarsBack(i, height02);
                }

            }
            else
                resetStatisticBars();
        }
        else
            resetStatisticBars();

        if (FMC_GameDataController.instance)
            textHeaderText.text = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.statistics][9];
        else
            textHeaderText.text = "Erfolgsquote";
    }

    private void setBarsForTimePerTask()
    {
        FMC_Statistics.Statistics statistics = FMC_GameDataController.instance.getCurrentStatistics();

        if (FMC_GameDataController.instance && FMC_GameDataController.instance.subscriptionIsActive() && statistics.dailyData.Count > 0)
        {
            float highestCount = getHighestCount();

            if (highestCount != 0)
            {
                for (int i = graphBarsFront.Count - 1, j = 0; i >= 0; i--, j++)
                {
                    float height = 0.0f;
                    float height02 = 0.0f;
                    if (statistics.dailyData.Count > j)
                    {
                        height = (statistics.dailyData[j].timeNeededPerTask * 2.52f) / highestCount; // 2.52 == Maximale Ausdehnung in  Units
                        height02 = 0;
                        bottomTexts01[i].text = statistics.dailyData[j].timeNeededPerTask.ToString("F1");
                        bottomTexts02[i].text = "";
                    }
                    else
                    {
                        bottomTexts01[i].text = "";
                        bottomTexts02[i].text = "";
                        height = 0;
                        height02 = 0;
                    }

                    height = Mathf.Clamp(height, 0.0f, 2.52f);
                    height02 = Mathf.Clamp(height02, 0.0f, 2.52f);
                    tweenGraphBars(i, height);
                    tweenGraphBarsBack(i, height02);
                }
            }
            else
                resetStatisticBars();
        }
        else
            resetStatisticBars();

        if (FMC_GameDataController.instance)
            textHeaderText.text = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.statistics][10];
        else
            textHeaderText.text = "Zeit pro Aufgabe";
    }

    private float getHighestCount ()
    {
        float highestCount = -1;
        FMC_Statistics.Statistics statistics = FMC_GameDataController.instance.getCurrentStatistics();

        if (currentDataShown == possibleDataShown.correctlySolved)
        {
            for (int i = 0; i < statistics.dailyData.Count; i++)
                if (statistics.dailyData[i].correctlyCalculatedTasks > highestCount || highestCount == -1)
                    highestCount = statistics.dailyData[i].correctlyCalculatedTasks;
        }
        else if (currentDataShown == possibleDataShown.correctInPercent)
        {
            for (int i = 0; i < statistics.dailyData.Count; i++)
                if (statistics.dailyData[i].calculatedTasksTotalWithoutBombs > highestCount || highestCount == -1)
                    highestCount = statistics.dailyData[i].calculatedTasksTotalWithoutBombs;
        }
        else if (currentDataShown == possibleDataShown.timePerTask)
        {
            for (int i = 0; i < statistics.dailyData.Count; i++)
                if (statistics.dailyData[i].timeNeededPerTask > highestCount || highestCount == -1)
                    highestCount = statistics.dailyData[i].timeNeededPerTask;
        }

        return highestCount;
    }

    public float getHighestCountBack()
    {
        float highestCount = -1;
        FMC_Statistics.Statistics statistics = FMC_GameDataController.instance.getCurrentStatistics();

        if (currentDataShown == possibleDataShown.correctlySolved)
        {
            for (int i = 0; i < statistics.dailyData.Count; i++)
                if (statistics.dailyData[i].calculatedTasksTotalWithoutBombs > highestCount || highestCount == -1)
                    highestCount = statistics.dailyData[i].calculatedTasksTotalWithoutBombs;
        }
        else if (currentDataShown == possibleDataShown.correctInPercent)
        {
            //for (int i = 0; i < statistics.dailyData.Count; i++)
            //    if (statistics.dailyData[i].correctlyCalculatedInPercent > highestCount || highestCount == -1)
            //        highestCount = statistics.dailyData[i].correctlyCalculatedInPercent;
            highestCount = 100;
        }
        else if (currentDataShown == possibleDataShown.timePerTask)
        {
            //for (int i = 0; i < statistics.dailyData.Count; i++)
            //    if (statistics.dailyData[i].timeNeededPerTask > highestCount || highestCount == -1)
            //        highestCount = statistics.dailyData[i].timeNeededPerTask;
            highestCount = 0;
        }

        return highestCount;
    }

    private void resetStatisticBars()
    {
        for (int i = 0; i < graphBarsFront.Count; i++)
        {
            float height = 0.1f;
            tweenGraphBars(i, height);
            tweenGraphBarsBack(i, height);
        }

        for (int i = 0; i < bottomTexts01.Count; i++)
        {
            bottomTexts01[i].text = "0";
            bottomTexts02[i].text = "0";
        }
    }

    private void tweenGraphBars (int i, float height)
    {
        LeanTween.cancel(graphBarsFront[i].gameObject);
        if (i == 0)
        {
            LeanTween.value(graphBarsFront[i].gameObject, graphBarsFront[i].size.y, height, 0.2f).setEase(LeanTweenType.easeOutCubic).setOnUpdate((float val) =>
            {
                graphBarsFront[i].size = new Vector2(graphBarsFront[i].size.x, val);
            });
        }
        else if (i == 1)
        {
            LeanTween.value(graphBarsFront[i].gameObject, graphBarsFront[i].size.y, height, 0.2f).setEase(LeanTweenType.easeOutCubic).setOnUpdate((float val) =>
            {
                graphBarsFront[i].size = new Vector2(graphBarsFront[i].size.x, val);
            });
        }
        else if (i == 2)
        {
            LeanTween.value(graphBarsFront[i].gameObject, graphBarsFront[i].size.y, height, 0.2f).setEase(LeanTweenType.easeOutCubic).setOnUpdate((float val) =>
            {
                graphBarsFront[i].size = new Vector2(graphBarsFront[i].size.x, val);
            });
        }
        else if (i == 3)
        {
            LeanTween.value(graphBarsFront[i].gameObject, graphBarsFront[i].size.y, height, 0.2f).setEase(LeanTweenType.easeOutCubic).setOnUpdate((float val) =>
            {
                graphBarsFront[i].size = new Vector2(graphBarsFront[i].size.x, val);
            });
        }
        else if (i == 4)
        {
            LeanTween.value(graphBarsFront[i].gameObject, graphBarsFront[i].size.y, height, 0.2f).setEase(LeanTweenType.easeOutCubic).setOnUpdate((float val) =>
            {
                graphBarsFront[i].size = new Vector2(graphBarsFront[i].size.x, val);
            });
        }
        else if (i == 5)
        {
            LeanTween.value(graphBarsFront[i].gameObject, graphBarsFront[i].size.y, height, 0.2f).setEase(LeanTweenType.easeOutCubic).setOnUpdate((float val) =>
            {
                graphBarsFront[i].size = new Vector2(graphBarsFront[i].size.x, val);
            });
        }
        else if (i == 6)
        {
            LeanTween.value(graphBarsFront[i].gameObject, graphBarsFront[i].size.y, height, 0.2f).setEase(LeanTweenType.easeOutCubic).setOnUpdate((float val) =>
            {
                graphBarsFront[i].size = new Vector2(graphBarsFront[i].size.x, val);
            });
        }
        else if (i == 7)
        {
            LeanTween.value(graphBarsFront[i].gameObject, graphBarsFront[i].size.y, height, 0.2f).setEase(LeanTweenType.easeOutCubic).setOnUpdate((float val) =>
            {
                graphBarsFront[i].size = new Vector2(graphBarsFront[i].size.x, val);
            });
        }
    }

    private void tweenGraphBarsBack (int i, float height)
    {
        LeanTween.cancel(graphBarsBack[i].gameObject);
        if (i == 0)
        {
            LeanTween.value(graphBarsBack[i].gameObject, graphBarsBack[i].size.y, height, 0.2f).setEase(LeanTweenType.easeOutCubic).setOnUpdate((float val) =>
            {
                graphBarsBack[i].size = new Vector2(graphBarsBack[i].size.x, val);
            });
        }
        else if (i == 1)
        {
            LeanTween.value(graphBarsBack[i].gameObject, graphBarsBack[i].size.y, height, 0.2f).setEase(LeanTweenType.easeOutCubic).setOnUpdate((float val) =>
            {
                graphBarsBack[i].size = new Vector2(graphBarsBack[i].size.x, val);
            });
        }
        else if (i == 2)
        {
            LeanTween.value(graphBarsBack[i].gameObject, graphBarsBack[i].size.y, height, 0.2f).setEase(LeanTweenType.easeOutCubic).setOnUpdate((float val) =>
            {
                graphBarsBack[i].size = new Vector2(graphBarsBack[i].size.x, val);
            });
        }
        else if (i == 3)
        {
            LeanTween.value(graphBarsBack[i].gameObject, graphBarsBack[i].size.y, height, 0.2f).setEase(LeanTweenType.easeOutCubic).setOnUpdate((float val) =>
            {
                graphBarsBack[i].size = new Vector2(graphBarsBack[i].size.x, val);
            });
        }
        else if (i == 4)
        {
            LeanTween.value(graphBarsBack[i].gameObject, graphBarsBack[i].size.y, height, 0.2f).setEase(LeanTweenType.easeOutCubic).setOnUpdate((float val) =>
            {
                graphBarsBack[i].size = new Vector2(graphBarsBack[i].size.x, val);
            });
        }
        else if (i == 5)
        {
            LeanTween.value(graphBarsBack[i].gameObject, graphBarsBack[i].size.y, height, 0.2f).setEase(LeanTweenType.easeOutCubic).setOnUpdate((float val) =>
            {
                graphBarsBack[i].size = new Vector2(graphBarsBack[i].size.x, val);
            });
        }
        else if (i == 6)
        {
            LeanTween.value(graphBarsBack[i].gameObject, graphBarsBack[i].size.y, height, 0.2f).setEase(LeanTweenType.easeOutCubic).setOnUpdate((float val) =>
            {
                graphBarsBack[i].size = new Vector2(graphBarsBack[i].size.x, val);
            });
        }
        else if (i == 7)
        {
            LeanTween.value(graphBarsBack[i].gameObject, graphBarsBack[i].size.y, height, 0.2f).setEase(LeanTweenType.easeOutCubic).setOnUpdate((float val) =>
            {
                graphBarsBack[i].size = new Vector2(graphBarsBack[i].size.x, val);
            });
        }
    }

}
