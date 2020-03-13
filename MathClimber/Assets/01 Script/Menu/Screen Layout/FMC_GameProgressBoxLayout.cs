using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FMC_GameProgressBoxLayout : MonoBehaviour
{

    public SpriteRenderer lineTop;
    //public RectTransform textHeader;
    public Text textHeaderText;
    public Text tags;
    public Text data;
    public Transform iapButton;
    public Transform iapButton02;
    public Transform iapButton02Position;

    private float cameraHeight;
    private float cameraWidth;
    private Vector2 cameraPosition;

    public void setLayout()
    {
        cameraHeight = Camera.main.orthographicSize * 2.0f;
        cameraWidth = cameraHeight * Camera.main.aspect;
        cameraPosition = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y);

        if (lineTop)
            lineTop.size = new Vector2(cameraWidth * 0.9f, lineTop.size.y);

        //if (textHeader)
        //    textHeader.position = new Vector3(cameraPosition.x - (cameraWidth * 0.5f) + (cameraWidth * 0.055f), textHeader.position.y, textHeader.position.z);

        if (textHeaderText && FMC_GameDataController.instance)
            textHeaderText.text = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.statistics][11];

        if (tags)
        {
            tags.transform.position = new Vector3(cameraPosition.x - (cameraWidth * 0.5f) + (cameraWidth * 0.055f), tags.transform.position.y, tags.transform.position.z);
            if (FMC_GameDataController.instance)
            {
                tags.text = "";
                tags.text += FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.statistics][12] + "\n";
                tags.text += FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.statistics][13] + "\n";
                tags.text += FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.statistics][14] + "\n";
                tags.text += FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.statistics][15] + "\n";
                tags.text += FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.statistics][16] + "\n";
                tags.text += FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.statistics][17] + "\n";
                tags.text += FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.statistics][18] + "\n";
                tags.text += FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.statistics][19];
            }
        }

        if (data)
            data.transform.position = new Vector3(Mathf.Abs(tags.transform.position.x), data.transform.position.y, data.transform.position.z);

        if (iapButton)
            iapButton.transform.position = new Vector3(data.transform.position.x - 0.25f, iapButton.transform.position.y, iapButton.transform.position.z);

        if (iapButton02)
            iapButton02.position = new Vector3(iapButton02Position.position.x, iapButton02.position.y, iapButton02.position.z);
    }

    public void renewStatisticsData()
    {
        FMC_Statistics.Statistics statistics = FMC_GameDataController.instance.getCurrentStatistics();

        if (FMC_GameDataController.instance.subscriptionIsActive())
        {

            string dataString = "";
            dataString += statistics.lifeTimeAnswers + "\n";
            if (statistics.lifeTimeAnswers > 0)
                dataString += ((statistics.lifeTimeNotCorrect * 100) / statistics.lifeTimeAnswers).ToString("F0") + "%\n";
            else
                dataString += "\n";
            dataString += statistics.lifeTimeCorrect + "\n";
            dataString += statistics.lifeTimeNotCorrect + "\n";
            dataString += convertTimeSpanToString(statistics) + "\n";
            dataString += Mathf.RoundToInt(statistics.lifeTimeAverageTime) + "s" + "\n";
            dataString += Persistence.moneyLifetime + "\n";
            dataString += Persistence.GetPurchaseNumber() + " / 39" + "\n";

            data.text = dataString;
        }
        else
        {
            string dataString = "";
            dataString += statistics.lifeTimeAnswers + "\n";
            dataString += "<color=#D4D4D4FF>33%\n";
            dataString += "2585\n";
            dataString += "833\n";
            dataString += "2h 5m\n";
            dataString += "12s\n";
            dataString += "1200\n";
            dataString += "12/30</color>\n";

            data.text = dataString;
        }
    }

    private string convertTimeSpanToString (FMC_Statistics.Statistics statistics)
    {
        string output = "";

        if (statistics.lifeTimeTotalTime.Days != 0)
            output += statistics.lifeTimeTotalTime.Days + "d ";
        if (statistics.lifeTimeTotalTime.Hours != 0)
            output += statistics.lifeTimeTotalTime.Hours + "h ";
        if (statistics.lifeTimeTotalTime.Minutes != 0)
            output += statistics.lifeTimeTotalTime.Minutes + "m ";
        if (statistics.lifeTimeTotalTime.Seconds != 0 && statistics.lifeTimeTotalTime.Hours == 0 && statistics.lifeTimeTotalTime.Days == 0)
            output += statistics.lifeTimeTotalTime.Seconds + "s ";

        if (output == "")
            output = "0" + "s";

        return output;
    }

}