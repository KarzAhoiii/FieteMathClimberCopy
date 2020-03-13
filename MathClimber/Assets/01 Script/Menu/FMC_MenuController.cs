using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FMC_MenuController : MonoBehaviour
{

    public GameObject selectionScreen;
    public GameObject freestyleSettings;
    public GameObject oneTimesOneSettings;
    public GameObject oneTimesOneSettingsBig;
    public GameObject showPastTasks;
    public GameObject swipeCards;
    public GameObject swipeCardsAd;
    public FMC_TalentsBoxLayout talentsBoxLayout;
    public FMC_OneTimesOneSettingsLayout oneTimesOneLayoutScript;

    public List<Text> swipeCardHeaders;
    public List<Text> swipeCardTexts;
    public Text mainSwipeCardHeader;

    private static FMC_MenuController instance = null;
    private FMC_GameDataController gameDataController = null;

    private float cameraHeight;
    private float cameraWidth;
    private Vector2 cameraPosition;
    private GameObject lastOpenedScreen;

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
            return;
        }

        if (FMC_GameDataController.instance)
            gameDataController = FMC_GameDataController.instance;

        gameDataController.setMenuController(this);
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += levelWasLoaded;

        freestyleSettings.SetActive(true);
        freestyleSettings.SetActive(false);
        oneTimesOneSettings.SetActive(true);
        oneTimesOneSettings.SetActive(false);
        oneTimesOneSettingsBig.SetActive(true);
        oneTimesOneSettingsBig.SetActive(false);
        showPastTasks.SetActive(true);
        showPastTasks.SetActive(false);
        swipeCards.SetActive(true);
        swipeCards.SetActive(false);
        swipeCardsAd.SetActive(true);
        swipeCardsAd.SetActive(false);

        lastOpenedScreen = selectionScreen;

        cameraHeight = Camera.main.orthographicSize * 2.0f;
        cameraWidth = cameraHeight * Camera.main.aspect;
        cameraPosition = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y);

        if (swipeCardHeaders.Count == 3 && swipeCardTexts.Count == 3 && FMC_GameDataController.instance)
        {
            if (swipeCardHeaders[0])
                swipeCardHeaders[0].text = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.swipeCards][6];

            if (swipeCardHeaders[1])
                swipeCardHeaders[1].text = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.swipeCards][7];

            if (swipeCardHeaders[2])
                swipeCardHeaders[2].text = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.swipeCards][8];

            if (swipeCardTexts[0])
                swipeCardTexts[0].text = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.swipeCards][15];

            if (swipeCardTexts[1])
                swipeCardTexts[1].text = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.swipeCards][16];

            if (swipeCardTexts[2])
                swipeCardTexts[2].text = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.swipeCards][17];
        }
        mainSwipeCardHeader.text = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.swipeCards][19].ToUpper();
    }

    private void disableSelectionScreen()
    {
        selectionScreen.SetActive(false);
    }

    private void disableScreensOnIapOpen ()
    {
        freestyleSettings.SetActive(false);
        oneTimesOneSettings.SetActive(false);
        oneTimesOneSettingsBig.SetActive(false);
        showPastTasks.SetActive(false);
        selectionScreen.SetActive(false);
    }

    private void disableSettingScreens()
    {
        freestyleSettings.SetActive(false);
        oneTimesOneSettings.SetActive(false);
        oneTimesOneSettingsBig.SetActive(false);
        showPastTasks.SetActive(false);

    }

    private void disableSettingScreensOnIapClose ()
    {
        if (lastOpenedScreen != freestyleSettings)
            freestyleSettings.SetActive(false);
        if (lastOpenedScreen != oneTimesOneSettings)
            oneTimesOneSettings.SetActive(false);
        if (lastOpenedScreen != oneTimesOneSettingsBig)
            oneTimesOneSettingsBig.SetActive(false);
        if (lastOpenedScreen != showPastTasks)
            showPastTasks.SetActive(false);
        if (lastOpenedScreen != selectionScreen)
            selectionScreen.SetActive(false);

    }

    public void activateLastOpenedScreen () {
         lastOpenedScreen.SetActive(true);
    }

    public void deactivateLastOpenedScreen (float delay) {
        print("**** deactivateLastOpened -----> WICHITG?????? (ist auskommentiert)");
        //LeanTween.delayedCall(delay, deactivateLastOpened);
    }

    private void deactivateLastOpened() {
        lastOpenedScreen.SetActive(false);
    }

    public void openFreestyleSettings (float speed = 0.3f) {
    
        gameDataController.setCurrentSetting(FMC_Settings_Controller.activeSetting.freestyle);

        freestyleSettings.transform.position = new Vector3(cameraPosition.x + cameraWidth, freestyleSettings.transform.position.y, freestyleSettings.transform.position.z);
        freestyleSettings.SetActive(true);
        LeanTween.moveX(freestyleSettings, cameraPosition.x, speed).setEase(LeanTweenType.easeOutCubic).setOnComplete(disableSelectionScreen);
        lastOpenedScreen = freestyleSettings;
    }

    public void openOneTimesOneSettings (float speed = 0.3f)
    {
        gameDataController.setCurrentSetting(FMC_Settings_Controller.activeSetting.oneTimesOne);

        oneTimesOneSettings.transform.position = new Vector3(cameraPosition.x + cameraWidth, oneTimesOneSettings.transform.position.y, oneTimesOneSettings.transform.position.z);
        oneTimesOneSettings.SetActive(true);
        LeanTween.moveX(oneTimesOneSettings, cameraPosition.x, speed).setEase(LeanTweenType.easeOutCubic).setOnComplete(disableSelectionScreen);
        lastOpenedScreen = oneTimesOneSettings;
    }

    public void openOneTimesOneSettingsBig(float speed = 0.3f)
    {
        gameDataController.setCurrentSetting(FMC_Settings_Controller.activeSetting.oneTimesOneBig);

        oneTimesOneSettingsBig.transform.position = new Vector3(cameraPosition.x + cameraWidth, oneTimesOneSettingsBig.transform.position.y, oneTimesOneSettingsBig.transform.position.z);
        oneTimesOneSettingsBig.SetActive(true);
        LeanTween.moveX(oneTimesOneSettingsBig, cameraPosition.x, speed).setEase(LeanTweenType.easeOutCubic).setOnComplete(disableSelectionScreen);
        lastOpenedScreen = oneTimesOneSettingsBig;
    }

    public void openPastTasks ()
    {
        
        showPastTasks.transform.position = new Vector3(cameraPosition.x + cameraWidth, showPastTasks.transform.position.y, showPastTasks.transform.position.z);
        showPastTasks.SetActive(true);
        LeanTween.moveX(showPastTasks, cameraPosition.x, 0.3f).setEase(LeanTweenType.easeOutCubic).setOnComplete(disableSelectionScreen);
        lastOpenedScreen = showPastTasks;
    }


    public void closeSettingsScreen ()
    {
        selectionScreen.SetActive(true);
        LeanTween.moveX(freestyleSettings, cameraPosition.x + cameraWidth, 0.25f).setEase(LeanTweenType.easeOutCubic);
        LeanTween.moveX(oneTimesOneSettings, cameraPosition.x + cameraWidth, 0.25f).setEase(LeanTweenType.easeOutCubic);
        LeanTween.moveX(oneTimesOneSettingsBig, cameraPosition.x + cameraWidth, 0.25f).setEase(LeanTweenType.easeOutCubic);
       
        LeanTween.moveX(showPastTasks, cameraPosition.x + cameraWidth, 0.25f).setEase(LeanTweenType.easeOutCubic).setOnComplete(() => {
            freestyleSettings.SetActive(false);
            oneTimesOneSettings.SetActive(false);
            oneTimesOneSettingsBig.SetActive(false);
            showPastTasks.SetActive(false);
        });
        
        lastOpenedScreen = selectionScreen;
    }


    public void playStoryMode ()
    {
        gameDataController.setCurrentSetting(FMC_Settings_Controller.activeSetting.storyMode);
        FLS_LoadingScreen.instance.loadScene("MathLadder");
    }
    
//rangeOfNumbers, numberTypeFront, numberTypeBack, PlusPossible, TimesPossible, MinusPossible, DividedPossible, GreaterPossible, SamePossible, SmallerPossible, EqualsPossible, OneTimesOnePossible, timeSpecification
    public void playFreestyle ()
    {
        gameDataController.setCurrentSetting(FMC_Settings_Controller.activeSetting.freestyle);
        FMC_Settings newSetting = new FMC_Settings();
        newSetting.setSettings(10, FMC_Settings.numberType.mixed, FMC_Settings.numberType.mixed, true, false, false, false, true, true, true, false, false, -1);
        gameDataController.setSettings(newSetting);
        openFreestyleSettings();
    }

    public void playCustomExercise (FMC_Settings_Input.allInformation problemInformation)
    {
        if (problemInformation == FMC_Settings_Input.allInformation.ttOneTimesOne || problemInformation == FMC_Settings_Input.allInformation.oxo_1
            || problemInformation == FMC_Settings_Input.allInformation.oxo_2 || problemInformation == FMC_Settings_Input.allInformation.oxo_3
            || problemInformation == FMC_Settings_Input.allInformation.oxo_4 || problemInformation == FMC_Settings_Input.allInformation.oxo_5
            || problemInformation == FMC_Settings_Input.allInformation.oxo_6 || problemInformation == FMC_Settings_Input.allInformation.oxo_7
            || problemInformation == FMC_Settings_Input.allInformation.oxo_8 || problemInformation == FMC_Settings_Input.allInformation.oxo_9
            || problemInformation == FMC_Settings_Input.allInformation.oxo_10)
        {
            gameDataController.setCurrentSetting(FMC_Settings_Controller.activeSetting.oneTimesOne);
            FMC_Settings nSetting = new FMC_Settings();
            nSetting.setSettings(10, FMC_Settings.numberType.core, FMC_Settings.numberType.core, false, true, false, false, true, true, true, false, true, -1);
            gameDataController.setSettings(nSetting);

            oneTimesOneLayoutScript.setAutoCheckButton(problemInformation);

            openOneTimesOneSettings();
            return;
        }

        gameDataController.setCurrentSetting(FMC_Settings_Controller.activeSetting.freestyle);
        FMC_Settings newSetting = createSettingsFromProblem(problemInformation);
        gameDataController.setSettings(newSetting);
        openFreestyleSettings();
    }
    

//rangeOfNumbers, numberTypeFront, numberTypeBack, PlusPossible, TimesPossible, MinusPossible, DividedPossible, GreaterPossible, SamePossible, SmallerPossible, EqualsPossible, OneTimesOnePossible, timeSpecification
    private FMC_Settings createSettingsFromProblem (FMC_Settings_Input.allInformation info)
    {
        FMC_Settings newSetting = new FMC_Settings();
        FMC_Settings sms = FMC_GameDataController.instance.getCurrentStoryModeSettings();
        int rangeOfNumbers = Mathf.Clamp(sms._rangeOfNumbers, 10, 2000);

        if (info == FMC_Settings_Input.allInformation.ron10)
            newSetting.setSettings(10, FMC_Settings.numberType.mixed, FMC_Settings.numberType.mixed, true, false, true, false, true, true, true, true, false, -1);
        else if (info == FMC_Settings_Input.allInformation.ron20)
            newSetting.setSettings(20, FMC_Settings.numberType.mixed, FMC_Settings.numberType.mixed, true, false, true, false, true, true, true, true, false, -1);
        else if (info == FMC_Settings_Input.allInformation.ron100)
            newSetting.setSettings(100, FMC_Settings.numberType.mixed, FMC_Settings.numberType.mixed, true, false, true, false, true, true, true, true, false, -1);
        else if (info == FMC_Settings_Input.allInformation.ron1000)
            newSetting.setSettings(1000, FMC_Settings.numberType.mixed, FMC_Settings.numberType.mixed, true, false, true, false, true, true, true, true, false, -1);

        else if (info == FMC_Settings_Input.allInformation.ntCore)
            newSetting.setSettings(rangeOfNumbers, FMC_Settings.numberType.core, FMC_Settings.numberType.core, true, false, true, false, true, false, true, false, false, -1);
        else if (info == FMC_Settings_Input.allInformation.ntNeighbour01)
            newSetting.setSettings(rangeOfNumbers, FMC_Settings.numberType.neighbour01, FMC_Settings.numberType.neighbour01, true, false, true, false, true, false, true, false, false, -1);
        else if (info == FMC_Settings_Input.allInformation.ntNeighbour02)
            newSetting.setSettings(rangeOfNumbers, FMC_Settings.numberType.neighbour02, FMC_Settings.numberType.neighbour02, true, false, true, false, true, false, true, false, false, -1);
        else if (info == FMC_Settings_Input.allInformation.ntMixed)
            newSetting.setSettings(rangeOfNumbers, FMC_Settings.numberType.mixed, FMC_Settings.numberType.mixed, true, false, true, false, true, false, true, false, false, -1);
        //else if (info == FMC_Settings_Input.allInformation.ntEven)
        //    newSetting.setSettings(rangeOfNumbers, FMC_Settings.numberType.even, FMC_Settings.numberType.even, true, false, true, false, true, true, true, true, false, -1);
        //else if (info == FMC_Settings_Input.allInformation.ntUneven)
        //    newSetting.setSettings(rangeOfNumbers, FMC_Settings.numberType.uneven, FMC_Settings.numberType.uneven, true, false, true, false, true, true, true, true, false, -1);

        else if (info == FMC_Settings_Input.allInformation.opPlus)
            newSetting.setSettings(rangeOfNumbers, FMC_Settings.numberType.mixed, FMC_Settings.numberType.mixed, true, false, false, false, true, true, true, true, false, -1);
        else if (info == FMC_Settings_Input.allInformation.opTimes)
            newSetting.setSettings(rangeOfNumbers, FMC_Settings.numberType.mixed, FMC_Settings.numberType.mixed, false, true, false, false, true, true, true, true, false, -1);
        else if (info == FMC_Settings_Input.allInformation.opMinus)
            newSetting.setSettings(rangeOfNumbers, FMC_Settings.numberType.mixed, FMC_Settings.numberType.mixed, false, false, true, false, true, true, true, true, false, -1);
        else if (info == FMC_Settings_Input.allInformation.opDivided)
            newSetting.setSettings(rangeOfNumbers, FMC_Settings.numberType.mixed, FMC_Settings.numberType.mixed, false, false, false, true, true, true, true, true, false, -1);

        else if (info == FMC_Settings_Input.allInformation.ttGreater)
            newSetting.setSettings(rangeOfNumbers, FMC_Settings.numberType.mixed, FMC_Settings.numberType.mixed, true, false, true, false, true, false, false, false, false, -1);
        else if (info == FMC_Settings_Input.allInformation.ttSame)
            newSetting.setSettings(rangeOfNumbers, FMC_Settings.numberType.mixed, FMC_Settings.numberType.mixed, true, false, true, false, false, true, false, false, false, -1);
        else if (info == FMC_Settings_Input.allInformation.ttSmaler)
            newSetting.setSettings(rangeOfNumbers, FMC_Settings.numberType.mixed, FMC_Settings.numberType.mixed, true, true, false, false, false, false, true, false, false, -1);
        else if (info == FMC_Settings_Input.allInformation.ttEquals)
            newSetting.setSettings(rangeOfNumbers, FMC_Settings.numberType.mixed, FMC_Settings.numberType.mixed, true, false, true, false, false, false, false, true, false, -1);

        else if (info == FMC_Settings_Input.allInformation.tInfinite)
            newSetting.setSettings(rangeOfNumbers, FMC_Settings.numberType.mixed, FMC_Settings.numberType.mixed, true, false, true, false, true, true, true, true, false, -1);
        else if (info == FMC_Settings_Input.allInformation.t5)
            newSetting.setSettings(rangeOfNumbers, FMC_Settings.numberType.mixed, FMC_Settings.numberType.mixed, true, false, true, false, true, true, true, true, false, 5);
        else if (info == FMC_Settings_Input.allInformation.t15)
            newSetting.setSettings(rangeOfNumbers, FMC_Settings.numberType.mixed, FMC_Settings.numberType.mixed, true, false, true, false, true, true, true, true, false, 15);
        else if (info == FMC_Settings_Input.allInformation.t30)
            newSetting.setSettings(rangeOfNumbers, FMC_Settings.numberType.mixed, FMC_Settings.numberType.mixed, true, false, true, false, true, true, true, true, false, 30);


        return newSetting;
    }


//rangeOfNumbers, numberTypeFront, numberTypeBack, PlusPossible, TimesPossible, MinusPossible, DividedPossible, GreaterPossible, SamePossible, SmallerPossible, EqualsPossible, OneTimesOnePossible, timeSpecification
    private FMC_Settings createSettingsFromPracticeMode (FMC_PracticeBoxLayout.practiceModes practiceMode)
    {
        FMC_Settings newSetting = new FMC_Settings();

        switch(practiceMode)
        {
            case FMC_PracticeBoxLayout.practiceModes.vorschule:
                newSetting.setSettings(10, FMC_Settings.numberType.core, FMC_Settings.numberType.mixed, true, false, false, false, true, true, true, false, false, -1);
                break;
            case FMC_PracticeBoxLayout.practiceModes.klasse1:
                newSetting.setSettings(20, FMC_Settings.numberType.mixed, FMC_Settings.numberType.neighbour01, true, false, true, false, true, true, true, false, false, -1);
                break;
            case FMC_PracticeBoxLayout.practiceModes.klasse2:
                newSetting.setSettings(20, FMC_Settings.numberType.mixed, FMC_Settings.numberType.neighbour02, true, false, true, false, true, true, true, false, false, -1);
                break;
            case FMC_PracticeBoxLayout.practiceModes.klasse3:
                newSetting.setSettings(100, FMC_Settings.numberType.mixed, FMC_Settings.numberType.mixed, true, true, true, true, true, true, true, false, false, -1);
                break;
            case FMC_PracticeBoxLayout.practiceModes.klasse4:
                newSetting.setSettings(200, FMC_Settings.numberType.mixed, FMC_Settings.numberType.mixed, true, true, true, true, true, true, true, false, false, -1);
                break;
            case FMC_PracticeBoxLayout.practiceModes.klasse5:
                newSetting.setSettings(1000, FMC_Settings.numberType.mixed, FMC_Settings.numberType.mixed, true, true, true, true, true, true, true, false, false, -1);
                break;
            case FMC_PracticeBoxLayout.practiceModes.addBis20:
                newSetting.setSettings(20, FMC_Settings.numberType.mixed, FMC_Settings.numberType.mixed, true, false, false, false, true, true, true, false, false, -1);
                break;
            case FMC_PracticeBoxLayout.practiceModes.addBis100:
                newSetting.setSettings(100, FMC_Settings.numberType.mixed, FMC_Settings.numberType.mixed, true, false, false, false, true, true, true, false, false, -1);
                break;
            case FMC_PracticeBoxLayout.practiceModes.subBis20:
                newSetting.setSettings(20, FMC_Settings.numberType.mixed, FMC_Settings.numberType.mixed, false, false, true, false, true, true, true, false, false, -1);
                break;
            case FMC_PracticeBoxLayout.practiceModes.subBis100:
                newSetting.setSettings(100, FMC_Settings.numberType.mixed, FMC_Settings.numberType.mixed, false, false, true, false, true, true, true, false, false, -1);
                break;
            case FMC_PracticeBoxLayout.practiceModes.mulBis50:
                newSetting.setSettings(50, FMC_Settings.numberType.mixed, FMC_Settings.numberType.mixed, false, true, false, false, true, true, true, false, false, -1);
                break;
            case FMC_PracticeBoxLayout.practiceModes.mulBis100:
                newSetting.setSettings(100, FMC_Settings.numberType.mixed, FMC_Settings.numberType.mixed, false, true, false, false, true, true, true, false, false, -1);
                break;
            case FMC_PracticeBoxLayout.practiceModes.divBis50:
                newSetting.setSettings(50, FMC_Settings.numberType.mixed, FMC_Settings.numberType.mixed, false, false, false, true, true, true, true, false, false, -1);
                break;
            case FMC_PracticeBoxLayout.practiceModes.divBis100:
                newSetting.setSettings(100, FMC_Settings.numberType.mixed, FMC_Settings.numberType.mixed, false, false, false, true, true, true, true, false, false, -1);
                break;
            case FMC_PracticeBoxLayout.practiceModes.aufgabenBis50:
                newSetting.setSettings(50, FMC_Settings.numberType.mixed, FMC_Settings.numberType.mixed, true, true, true, true, true, true, true, false, false, -1);
                break;
            case FMC_PracticeBoxLayout.practiceModes.aufgabenBis100:
                newSetting.setSettings(100, FMC_Settings.numberType.mixed, FMC_Settings.numberType.mixed, true, true, true, true, true, true, true, false, false, -1);
                break;
            case FMC_PracticeBoxLayout.practiceModes.verdoppeln:
                newSetting.setSettings(50, FMC_Settings.numberType.mixed, FMC_Settings.numberType.mixed, true, false, false, false, false, true, false, false, false, -1);
                break;
            case FMC_PracticeBoxLayout.practiceModes.zehneruebergang:
                newSetting.setSettings(12, FMC_Settings.numberType.mixed, FMC_Settings.numberType.mixed, true, false, true, false, true, true, true, false, false, -1);
                break;
        }

        return newSetting;

    }

//rangeOfNumbers, numberTypeFront, numberTypeBack, PlusPossible, TimesPossible, MinusPossible, DividedPossible, GreaterPossible, SamePossible, SmallerPossible, EqualsPossible, OneTimesOnePossible, timeSpecification
    public void playBasics ()
    {
        gameDataController.setCurrentSetting(FMC_Settings_Controller.activeSetting.freestyle);
        FMC_Settings newSetting = new FMC_Settings();
        newSetting.setSettings(15, FMC_Settings.numberType.mixed, FMC_Settings.numberType.mixed, true, false, true, false, true, true, true, true, false, -1);
        gameDataController.setSettings(newSetting);
        openFreestyleSettings();
        //SceneManager.LoadScene("MathLadder");
        //FLS_LoadingScreen.instance.loadScene("MathLadder");
    }

    public void playPractice (FMC_PracticeBoxLayout.practiceModes practiceMode)
    {
        if (practiceMode == FMC_PracticeBoxLayout.practiceModes.freestyle)
            playFreestyle();

        else if (practiceMode == FMC_PracticeBoxLayout.practiceModes.oneTimesOne)
            openOneTimesOneSettings();

        else if (practiceMode == FMC_PracticeBoxLayout.practiceModes.oneTimesOneBig)
            openOneTimesOneSettingsBig();

        else if (practiceMode == FMC_PracticeBoxLayout.practiceModes.basics)
            playBasics();

        else
        {
            gameDataController.setCurrentSetting(FMC_Settings_Controller.activeSetting.freestyle);
            FMC_Settings newSetting = createSettingsFromPracticeMode(practiceMode);
            gameDataController.setSettings(newSetting);
            openFreestyleSettings();
        }
    }

    private void levelWasLoaded (Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Menu01")
        {
            selectionScreen.SetActive(true);
            freestyleSettings.SetActive(false);
            oneTimesOneSettings.SetActive(false);
            oneTimesOneSettingsBig.SetActive(false);
            showPastTasks.SetActive(false);

            if (FMC_GameDataController.instance) {
                FMC_GameDataController.instance.menuSceneWasLoaded();
            }
        }
        else
        {
            selectionScreen.SetActive(false);
            freestyleSettings.SetActive(false);
            oneTimesOneSettings.SetActive(false);
            oneTimesOneSettingsBig.SetActive(false);
            showPastTasks.SetActive(false);
        }
    }
}
