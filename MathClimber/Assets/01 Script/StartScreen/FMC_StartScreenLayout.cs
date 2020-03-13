using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;


public class FMC_StartScreenLayout : MonoBehaviour
{

    public Transform infoButton;
    public GameObject swipeCards;
    public BoxCollider2D startButtonBoxCollider;

    public Text swipeCardHeader01;
    public Text swipeCardHeader02;
    public Text swipeCardHeader03;
    public Text swipeCardHeader04;
    public Text swipeCardHeader05;
    public Text swipeCardHeader06;

    public Text swipeCardText01;
    public Text swipeCardText02;
    public Text swipeCardText03;
    public Text swipeCardText04;
    public Text swipeCardText05;
    public Text swipeCardText06;

    private float cameraHeight;
    private float cameraWidth;
    private Vector2 cameraPosition;

    private void Awake ()
    {
        setLayout();

        if (File.Exists(Application.persistentDataPath + Path.DirectorySeparatorChar + "playerData.data"))
        {
            swipeCards.SetActive(false);
            startButtonBoxCollider.enabled = true;
        }
        else
        {
            swipeCards.SetActive(true);
            startButtonBoxCollider.enabled = false;
        }
    }


    private void setLayout ()
    {
        cameraHeight = Camera.main.orthographicSize * 2.0f;
        cameraWidth = cameraHeight * Camera.main.aspect;
        cameraPosition = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y);

        swipeCards.SetActive(true);
        swipeCards.SetActive(false);

        if (infoButton)
            infoButton.position = new Vector3((cameraPosition.x + (cameraWidth * 0.5f)) - (infoButton.localScale.x * 1.1f), (cameraPosition.y + (cameraHeight * 0.5f)) - (infoButton.localScale.y * 1.1f), 0.0f);

        if (FMC_GameDataController.instance)
        {
            if (swipeCardHeader01)
                swipeCardHeader01.text = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.swipeCards][0];

            if (swipeCardHeader02)
                swipeCardHeader02.text = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.swipeCards][1];

            if (swipeCardHeader03)
                swipeCardHeader03.text = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.swipeCards][2];

            if (swipeCardHeader04)
                swipeCardHeader04.text = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.swipeCards][3];

            if (swipeCardHeader05)
                swipeCardHeader05.text = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.swipeCards][4];

            if (swipeCardHeader06)
                swipeCardHeader06.text = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.swipeCards][5];

            if (swipeCardText01)
                swipeCardText01.text = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.swipeCards][9];

            if (swipeCardText02)
                swipeCardText02.text = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.swipeCards][10];

            if (swipeCardText03)
                swipeCardText03.text = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.swipeCards][11];

            if (swipeCardText04)
                swipeCardText04.text = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.swipeCards][12];

            if (swipeCardText05)
                swipeCardText05.text = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.swipeCards][13];

            if (swipeCardText06)
                swipeCardText06.text = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.swipeCards][14];
        }
    }
}
