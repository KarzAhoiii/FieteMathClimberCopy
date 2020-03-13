using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using UnityEngine.UI;

public class FMC_GameDataController : MonoBehaviour
{

    public AudioClip buttonClickSound;
    [Range (0.0f, 1.0f)] public float buttonClickVolume;

    public delegate void focusEvent ();
    public static event focusEvent newSessionStarted;
    public delegate void returnRewardEvent();
    public static event returnRewardEvent grantReturnReward;

    [HideInInspector] public static FMC_GameDataController instance = null;
    [HideInInspector] public Dictionary<FMC_Translation.translations, List<string>> fullTranslation { get; private set; }
    [HideInInspector] public FMC_MenuController menuController { get; private set; }

    public FMC_TaskCreation taskCreator;
    public FMC_Settings_Controller settingsController;
    public FMC_Settings_StoryMode storyModeSettings;
    public FMC_Settings_OneTimesOne oneTimesOneSettings;
    public FMC_Settings_OneTimesOne oneTimesOneBigSettings;
    public FMC_Settings_Freestyle freestyleSettings;
    public FMC_Statistics statistics;
    public FMC_SaveLoad saveLoad;
    public FMC_Translation loadTranslation;
    
    private DateTime timeWhenApplicationStopped;
    private DateTime currentInternetTime;
    private bool subIsActive = false;
    private bool giveReturnReward = false;
    private string receiptLog = "";

    private void Start()
    {

       DontDestroyOnLoad(gameObject);
       instance = this;
        //if (instance == null)
        //{
            //instance = this;
            
        //}
        //else if (instance != this)
        //{
        //    Destroy(this.gameObject);
        //    return;
        //}

        Application.targetFrameRate = 60;
        
        ContentData.loadData();
        

        //storyModeSettings = new FMC_Settings_StoryMode();
       	//storyModeSettings.loadLevelData();
        //oneTimesOneSettings = new FMC_Settings_OneTimesOne();
        //oneTimesOneBigSettings = new FMC_Settings_OneTimesOne();
        //freestyleSettings = new FMC_Settings_Freestyle();
        //settingsController = new FMC_Settings_Controller(storyModeSettings, oneTimesOneSettings, oneTimesOneBigSettings, freestyleSettings);
        //taskCreator = new FMC_TaskCreation(settingsController);
        //statistics = new FMC_Statistics(settingsController, storyModeSettings);
        //saveLoad = new FMC_SaveLoad(storyModeSettings, oneTimesOneSettings, freestyleSettings, statistics);
        
        //loadTranslation = new FMC_Translation();
        //fullTranslation = new Dictionary<FMC_Translation.translations, List<string>>();
        //fullTranslation = loadTranslation.getTranslation();
        
        
        storyModeSettings.loadLevelData();
        settingsController.Init (storyModeSettings, oneTimesOneSettings, oneTimesOneBigSettings, freestyleSettings);
        taskCreator.Init(settingsController);
        statistics.Init(settingsController, storyModeSettings);

        saveLoad.Init(storyModeSettings, oneTimesOneSettings, freestyleSettings, statistics);
        subIsActive = saveLoad.subscriptionIsActiveFromSavedData();

        fullTranslation = new Dictionary<FMC_Translation.translations, List<string>>();
        fullTranslation = loadTranslation.getTranslation();


        menuController = null;
        
        

        PS_PlayButtonInfo.playerNameChanged += setCurrentPlayer;
        if (SceneManager.GetActiveScene().name == "Menu01")
            loadData();

        timeWhenApplicationStopped = DateTime.Now;
        
        if (newSessionStarted != null)
            newSessionStarted();
    }
    
    private void setLevelName () {
    
        FMC_Settings_Controller.activeSetting tempSetting = getCurrentPlayMode();
       
        InfoPanel infoPanel = FindObjectOfType<InfoPanel> ();
        
        if (tempSetting == FMC_Settings_Controller.activeSetting.storyMode) {
            if (infoPanel) {
                infoPanel.activate();
                infoPanel.setLabel(FMC_Settings_Controller.currentLevelName);
            }
        } else {
            if (infoPanel) {
                infoPanel.deactivate();
            }
        }
    }


    public void createFirstTask ()  {
        
        if (taskCreator != null) {
            taskCreator.createTask();
            setLevelName();
        }
    }

	public void answerTask(FMC_Task task/*, bool stat*/) {
    
        statistics.setAnswer(task);
        taskCreator.createTask();
        saveData();
        setLevelName();
    }

    public void setCurrentSetting (FMC_Settings_Controller.activeSetting newSetting) {
        settingsController.setCurrentSettings(newSetting);
        statistics.resetPowerUpCounter();
    }

    public void setSettings (FMC_Settings newSettings) {
        settingsController.getCurrentSettingValues().setSettings(newSettings);
    }

    public FMC_Settings getCurrentSettings () {
        return settingsController.getCurrentSettingValues();
    }

    public FMC_Settings getCurrentStoryModeSettings ()
    {
        return settingsController.getCurrentStoryModeSettings();
    }

    public FMC_Settings_Controller.activeSetting getCurrentPlayMode ()
    {
        return settingsController.getCurrentSetting();
    }
    
    public int getCurrentRangeOfNumbers ()
    {
        return settingsController.getCurrentRangeOfNumbers();
    }

    public List<FMC_Statistics.Statistics.DataPerDay> getDailyData ()
    {
        return statistics.overallStatistics.dailyData;
    }

    public void setOneTimesOneSettings(List<int> includes)
    {
        oneTimesOneSettings.setOneTimesOneSettings(includes);
    }

    public void setOneTimesOneSettingsBig(List<int> includes)
    {
        oneTimesOneBigSettings.setOneTimesOneSettings(includes);
    }

    public FMC_Statistics.Statistics getCurrentStatistics ()
    {
        return statistics.overallStatistics;
    }

    public FMC_Statistics.TalentsProblems getTalentProblemData ()
    {
        return statistics.overallStatistics.getTalentsAndProblems();
    }

    public void setCurrentPlayer (string newName, Guid uniqueID, PS_InputField.ageTypes age)
    {

        saveLoad.setCurrentPlayer(newName, uniqueID, age);
        loadData();
        if (newSessionStarted != null)
            newSessionStarted();
      
    }

    public string getCurrentPlayerName ()
    {
        return saveLoad.getCurrentPlayerName();
    }

    public PS_InputField.ageTypes getCurrentPlayerAge ()
    {
        return saveLoad.getCurrentPlayerAge();
    }

    public Guid getCurrentPlayerID ()
    {
        return saveLoad.getCurrentPlayerID();
    }

    public void resetPlayerData()
    {
       print("ERROR: in  -> storyModeSettings.resetData()");
       storyModeSettings.resetData();
       statistics.resetStatistics();
    }

    public void saveData()
    {
        saveLoad.Save();
    }

    public void loadData()
    {
        saveLoad.Load();
    }

    /* ABO
    public FMC_InAppPurchasing getInAppPurchasing ()
    {
        return inAppPurchasing;
    }
    */

    public void saveNewExpirationDate (DateTime expirationDate)
    {
        saveLoad.saveSubscriptionInformation(expirationDate);
    }

    public void saveOneTimePayment ()
    {
        saveLoad.saveUserBoughtOneTime();
    }

    public bool subscriptionIsActive () {
        return subIsActive;
    }

    /* ABO
    public void checkIfSubscriptionIsActiveFromReceipt ()
    {
        receiptController.checkIfSubscriptionIsActive();
    }

    public void saveLongestSubscriptionFromReceipt()
    {
        receiptController.saveLongestLastingExpirationDate();
    }
    */

    public bool userDidBuySubscriptionOnce ()
    {
        return saveLoad.userDidBuySubOnce();
    }

    public void setSubscriptionToActive ()
    {
        subIsActive = true;
    }

    public void setMenuController(FMC_MenuController controller)
    {
        menuController = controller;
    }

    public void makeStoryModeEasier()
    {
        storyModeSettings.makeStoryModeEasier();
    }

    public void makeStoryModeHarder ()
    {
        storyModeSettings.makeStoryModeHarder();
    }

    public void resetStoryModeData() {
        storyModeSettings.resetData();
    }


    public DateTime getCurrentInternetTime() {
        return currentInternetTime;
    }

    private void checkIfSubscriptionIsActive () {
    
        subIsActive = true;
        /* ABO
        subIsActive = false;
        currentInternetTime = JF_Utility.GetFastestNISTDate();
        currentInternetTime = currentInternetTime.ToUniversalTime();
        subIsActive = saveLoad.subscriptionIsActiveFromSavedData();
        if (!subIsActive && inAppPurchasing.IsInitialized())
            checkIfSubscriptionIsActiveFromReceipt();
        */
    }

    private void checkForReturnReward () {
        if (saveLoad.checkForReturnReward()) {
            giveReturnReward = true;
        }
    }

    private void OnApplicationFocus (bool hasFocus) {
        if (!hasFocus)
            timeWhenApplicationStopped = DateTime.Now;
        else if (hasFocus && (DateTime.Now - timeWhenApplicationStopped).TotalHours > 2 && newSessionStarted != null) {
            newSessionStarted();
            checkForReturnReward();
            checkIfSubscriptionIsActive();
        } else {
            checkForReturnReward();
        }
    }

    public void menuSceneWasLoaded ()  {
    
        if (giveReturnReward) {
			if (grantReturnReward != null){
				
                grantReturnReward();
				giveReturnReward = false;
			}

            saveLoad.saveNewLoginDate(DateTime.UtcNow);
        }
    }
}
