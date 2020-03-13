using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FMC_ShowPastLayout : MonoBehaviour
{

    public GameObject background;
    public GameObject backButton;
    public GameObject overlay;
    public List<FMC_SessionButton> allSessionButtons;
    public Text header;
    public Text headerGreeting;
    public Transform scrollRectParent;

    public List<FMC_Statistics.Statistics.DataPerDay> dailyData { get; private set; }

    private bool isIphoneX = false;
    private float cameraHeight;
    private float cameraWidth;
    private Vector2 cameraPosition;
    private int numberOfActiveButtons = 0;

    //private void Awake ()
    //{
    //    setLayout();
    //}

    private void OnEnable ()
    {
        for (int i = 0; i < allSessionButtons.Count; i++)
        {
            if (allSessionButtons[i].gameObject.activeSelf && allSessionButtons[i].getIsUnfolded())
            {
                allSessionButtons[i].startAction();
            }
        }

        if (allSessionButtons[0].gameObject.activeSelf)
            allSessionButtons[0].startAction();

        scrollRectParent.transform.localPosition = isIphoneX ? new Vector3(0, -0.33f, 0) : new Vector3(0, 0, 0);
    }

    public void setLayout ()
    {
        cameraHeight = Camera.main.orthographicSize * 2.0f;
        cameraWidth = cameraHeight * Camera.main.aspect;
        cameraPosition = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y);
        
        header.text = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.statistics][47];

        if ((float)Screen.width / (float)Screen.height < 0.5f)
            isIphoneX = true;

        if (background)
            background.transform.localScale = new Vector3(cameraWidth, cameraHeight, 1.0f);

        if (backButton)
            backButton.transform.position = new Vector3((cameraPosition.x - (cameraWidth * 0.5f)) + (backButton.transform.localScale.x * 1.1f), (cameraPosition.y + (cameraHeight * 0.5f)) - (backButton.transform.localScale.y * 1.1f), 0.0f);

        if (overlay)
            overlay.transform.localScale = new Vector3(cameraWidth, overlay.transform.localScale.y, 1.0f);

        for (int i = 0; i < allSessionButtons.Count; i++)
        {
            allSessionButtons[i].setLayout(cameraHeight, cameraWidth, cameraPosition);
        }

        clickedSessionsButton();
    }

    public void renewStatisticsData ()
    {
        dailyData = FMC_GameDataController.instance.getDailyData();

        if (headerGreeting)
            headerGreeting.text = FMC_GameDataController.instance.getCurrentPlayerName() + ", " + JF_Utility.convertAgeTypeToString(FMC_GameDataController.instance.getCurrentPlayerAge());

        for (int i = 0; i < allSessionButtons.Count; i++)
        {
            if (i < dailyData.Count)
            {
                allSessionButtons[i].gameObject.SetActive(true);
                allSessionButtons[i].sessionID.text = dailyData[i].date.ToString(@"dd\.MM\.yyyy, HH:mm");
                allSessionButtons[i].resetStatisticsData(dailyData[i]);
            }
            else
            {
                allSessionButtons[i].gameObject.SetActive(false);
            }
        }

        numberOfActiveButtons = dailyData.Count;
    }

    public void clickedSessionsButton ()
    {
        for (int i = 1; i < allSessionButtons.Count; i++)
        {
            allSessionButtons[i].transform.position = new Vector3(allSessionButtons[i].transform.position.x, allSessionButtons[i - 1].getNextButtonPosition(), allSessionButtons[i].transform.position.z);
        }
    }

    public float getBottomPosition ()
    {
        //Debug.Log(numberOfActiveButtons);
        if (allSessionButtons.Count > 0 && numberOfActiveButtons > 0)
            return allSessionButtons[numberOfActiveButtons - 1].getNextButtonPosition();
        else
            return 0;
    }

    public float getTopPosition ()
    {
        return allSessionButtons[0].transform.position.y + (allSessionButtons[0].selfSpriteRenderer.size.y * 0.5f) + 1.35f;
    }

    public float getCompleteSize ()
    {
        return Mathf.Abs(allSessionButtons[0].transform.position.y - allSessionButtons[allSessionButtons.Count - 1].getNextButtonPosition());
    }
}
