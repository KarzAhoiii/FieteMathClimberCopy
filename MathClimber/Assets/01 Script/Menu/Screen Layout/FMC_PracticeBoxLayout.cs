using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class FMC_PracticeBoxLayout : MonoBehaviour
{

    public enum practiceModes { freestyle, basics, oneTimesOne, oneTimesOneBig, showPast, customExercise,
    vorschule, klasse1, klasse2, klasse3, klasse4, klasse5, addBis20, addBis100, subBis20, subBis100, mulBis50, mulBis100, divBis50, divBis100, aufgabenBis50, aufgabenBis100, verdoppeln, zehneruebergang};

    public List<FMC_PracticeButton> practiceButtons;

    public FMC_MenuController menuController;
    [Range(0.0f, 0.5f)] public float transitionTime;
    public Color downColor;
    public Color upColor;
    public Text UebungenText;
    public Text UebungenNachThema;
    public Text FreiesRechnenText;
    public Text GrundlagenText;
    public Text EinmalEinsText;
    public Text EinmalEinsTextBig;
    public Text aufgabenUebersicht;
    public Text vorschule;
    public Text klasse1;
    public Text klasse2;
    public Text klasse3;
    public Text klasse4;
    public Text klasse5;
    public Text addBis20;
    public Text addBis100;
    public Text subBis20;
    public Text subBis100;
    public Text mulBis50;
    public Text mulBis100;
    public Text divBis50;
    public Text divBis100;
    public Text aufgabenBis50;
    public Text aufgabenBis100;
    public Text verdoppeln;
    public Text zehneruebergang;
    public Text sofortUeben;

    private float cameraHeight;
    private float cameraWidth;
    private Vector2 cameraPosition;

    private float totalBoxHeight;

    public void setLayout()
    {
        cameraHeight = Camera.main.orthographicSize * 2.0f;
        cameraWidth = cameraHeight * Camera.main.aspect;
        cameraPosition = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y);

        foreach (FMC_PracticeButton button in practiceButtons)
            button.setLayout(cameraWidth, cameraPosition);

        if (FMC_GameDataController.instance)
        {
            UebungenText.text = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.statistics][21];
            UebungenNachThema.text = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.statistics][26];
            FreiesRechnenText.text = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.statistics][1];
            GrundlagenText.text = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.statistics][2];
            EinmalEinsText.text = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.statistics][3];
            EinmalEinsTextBig.text = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.statistics][24];
            aufgabenUebersicht.text = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.statistics][27];

            vorschule.text = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.statistics][33];
            klasse1.text = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.statistics][28];
            klasse2.text = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.statistics][29];
            klasse3.text = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.statistics][30];
            klasse4.text = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.statistics][31];
            klasse5.text = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.statistics][32];

            addBis20.text = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.statistics][34];
            addBis100.text = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.statistics][35];
            subBis20.text = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.statistics][36];
            subBis100.text = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.statistics][37];
            mulBis50.text = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.statistics][38];
            mulBis100.text = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.statistics][39];
            divBis50.text = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.statistics][40];
            divBis100.text = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.statistics][41];
            aufgabenBis50.text = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.statistics][42];
            aufgabenBis100.text = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.statistics][43];
            verdoppeln.text = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.statistics][44];
            zehneruebergang.text = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.statistics][45];
            sofortUeben.text = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.statistics][46];


        }
    }
}