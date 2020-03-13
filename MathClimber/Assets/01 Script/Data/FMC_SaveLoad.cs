using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class FMC_SaveLoad : MonoBehaviour
{

    private DateTime expirationDate = DateTime.MinValue;
    private bool userBoughtOneTime = false;
    private bool userDidBuySubscriptionOnce = false;

    private FMC_Settings_StoryMode storyModeSettings;
    private FMC_Settings_OneTimesOne oneTimesOneSettings;
    private FMC_Settings_Freestyle freestyleSettings;
    private FMC_Statistics statistics;

    private string currentPlayerName = "Dave";
    private Guid currentPLayerID;
    private PS_InputField.ageTypes currentPlayerAge;
    
    
    public void Init (FMC_Settings_StoryMode _storyModeSettings, FMC_Settings_OneTimesOne _oneTimesOneSettings, FMC_Settings_Freestyle _freestyleSettings, FMC_Statistics _statistics)
    {
        expirationDate = DateTime.MinValue;

        storyModeSettings = _storyModeSettings;
        oneTimesOneSettings = _oneTimesOneSettings;
        freestyleSettings = _freestyleSettings;
        statistics = _statistics;
        
        
        //loadPurchaseInformation();
        
        //PlayerPrefs.DeleteAll();s
        if (PlayerPrefs.HasKey ("MoneyDave")) {
              PlayerPrefs.SetInt("MoneyDave", 100000);
              Debug.Log("Mony Dave: "+PlayerPrefs.GetInt("MoneyDave"));
        }
    }
    
	/*
    public FMC_SaveLoad (FMC_Settings_StoryMode _storyModeSettings, FMC_Settings_OneTimesOne _oneTimesOneSettings, FMC_Settings_Freestyle _freestyleSettings, FMC_Statistics _statistics)
    {
        expirationDate = DateTime.MinValue;

        storyModeSettings = _storyModeSettings;
        oneTimesOneSettings = _oneTimesOneSettings;
        freestyleSettings = _freestyleSettings;
        statistics = _statistics;
        
        
        loadPurchaseInformation();
        
        //PlayerPrefs.DeleteAll();s
        if (PlayerPrefs.HasKey ("MoneyDave")) {
              PlayerPrefs.SetInt("MoneyDave", 100000);
              Debug.Log("Mony Dave: "+PlayerPrefs.GetInt("MoneyDave"));
        }
    }
    */

    public void setCurrentPlayer(string name, Guid uniqeID, PS_InputField.ageTypes age)
    {

        currentPlayerName = name;
        currentPLayerID = uniqeID;
        currentPlayerAge = age;
    }

    public string getCurrentPlayerName()
    {
        return currentPlayerName;
    }

    public PS_InputField.ageTypes getCurrentPlayerAge ()
    {
        return currentPlayerAge;
    }

    public Guid getCurrentPlayerID()
    {
        if (currentPLayerID != Guid.Empty)
            return currentPLayerID;
        else
        {
            Debug.LogWarning("GUID is empty:" + currentPLayerID);
            return currentPLayerID;
        }
    }

    [System.Serializable]
    private struct saveload
    {
        public FMC_Settings_StoryMode storyModeSettings;
        public FMC_Settings_OneTimesOne oneTimesOneSettings;
        public FMC_Settings_Freestyle freestyleSettings;
        public FMC_Statistics statistics;
    }

    public void Save()
    {
        saveload sl = new saveload();

        sl.storyModeSettings = storyModeSettings;
        sl.oneTimesOneSettings = oneTimesOneSettings;
        sl.freestyleSettings = freestyleSettings;
        sl.statistics = statistics;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + Path.DirectorySeparatorChar + currentPlayerName + ".fmc");
        bf.Serialize(file, sl);
        file.Close();
    }

    public void Load()
    {

        if (File.Exists(Application.persistentDataPath + Path.DirectorySeparatorChar + currentPlayerName + ".fmc")) 
        {
            saveload sl = new saveload();
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + Path.DirectorySeparatorChar + currentPlayerName + ".fmc", FileMode.Open);
            sl = (saveload)bf.Deserialize(file);
            file.Close();

            // Anwendung der geladenen Daten
            storyModeSettings.createSettingsFromLoadedData(sl.storyModeSettings.step, sl.storyModeSettings.level);
            oneTimesOneSettings.setSettings(sl.oneTimesOneSettings);
            freestyleSettings.setSettings(sl.freestyleSettings);

            statistics.setStatisticsFromLoadedData(sl.statistics);

            //Debug.Log("One times One Settings");
            //oneTimesOneSettings.logAllSettings();
            //Debug.Log("FreestyleSettings");
            //freestyleSettings.logAllSettings();
        }
        else
        {
            if (FMC_GameDataController.instance)
               FMC_GameDataController.instance.resetPlayerData();
        }	
    }

    [System.Serializable]
    public struct purchaseInformation
    {
        public DateTime expirationDate;
        public bool userBoughtOneTime;
        public bool userDidBuySubscriptionOnce;
    }

    public void saveUserBoughtOneTime ()
    {
        userBoughtOneTime = true;
        //savePurchaseInformation();
    }

    public void saveSubscriptionInformation (DateTime _expirationDate)
    {
        // Expiration Date kommt direkt aus dem Receipt und sollte in Utc gespeichert sein.
        expirationDate = _expirationDate;
        userDidBuySubscriptionOnce = true;
       // savePurchaseInformation();
    }

	/*
    private void savePurchaseInformation()
    {
        purchaseInformation pi = new purchaseInformation();

        pi.expirationDate = expirationDate;
        pi.userBoughtOneTime = userBoughtOneTime;
        pi.userDidBuySubscriptionOnce = userDidBuySubscriptionOnce;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + Path.DirectorySeparatorChar + "PurchaseInformation.fmc");
        bf.Serialize(file, pi);
        file.Close();
    }

    public void loadPurchaseInformation()
    {
        if (File.Exists(Application.persistentDataPath + Path.DirectorySeparatorChar + "PurchaseInformation.fmc"))
        {
            purchaseInformation pi = new purchaseInformation();
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + Path.DirectorySeparatorChar + "PurchaseInformation.fmc", FileMode.Open);
            pi = (purchaseInformation)bf.Deserialize(file);
            file.Close();

            expirationDate = pi.expirationDate;
            userBoughtOneTime = pi.userBoughtOneTime;
            userDidBuySubscriptionOnce = pi.userDidBuySubscriptionOnce;
        }
    }
    */

    public bool subscriptionIsActiveFromSavedData ()
    {
        /* ABO
        // Nächste Zeile zurückgeben, wenn der IAP nicht drin sein soll:
        //return true;

        // Nächste Zeile zurückgeben, wenn der Zeittest von der gespeicherten Zeit genutzt werdn soll
        if (userBoughtOneTime || !JF_Utility.isExpired(expirationDate))
            return true;
        else
            return false;

        // Nächste Zeilen zurückgeben, wenn jedesmal das Receipt befragt werden soll, ob die Sub active ist, falls der user nicht einmalig bezahlt hat
        if (userBoughtOneTime)
            return true;
        else
            return false;
        */
        return true;
    }

    public bool userDidBuySubOnce ()
    {
        return userDidBuySubscriptionOnce;
    }


    // Log In Data
    [System.Serializable]
    public struct logInData
    {
        public DateTime lastLogInDate;
    }

    public void saveNewLoginDate (DateTime newDate)
    {
        logInData li = new logInData();

        li.lastLogInDate = newDate;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + Path.DirectorySeparatorChar + "LoginData.fmc");
        bf.Serialize(file, li);
        file.Close();
    }

    public bool checkForReturnReward ()
    {
        if (File.Exists(Application.persistentDataPath + Path.DirectorySeparatorChar + "LoginData.fmc"))
        {
            logInData li = new logInData();
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + Path.DirectorySeparatorChar + "LoginData.fmc", FileMode.Open);
            li = (logInData)bf.Deserialize(file);
            file.Close();

            TimeSpan timespan = DateTime.UtcNow - li.lastLogInDate;
            
			if (timespan.TotalDays > 1)
                return true;
            else
                return false;
        }
        else
        {
            saveNewLoginDate(DateTime.UtcNow);
            return false;
        }
    }

    // Translation Saving
    [System.Serializable]
    public struct translationInformation
    {
        public string twoLetterISOCode;
        public Dictionary<FMC_Translation.translations, List<string>> translation;
    }

    public void saveTranslation(string ISOCode, Dictionary<FMC_Translation.translations, List<string>> translation)
    {
        translationInformation ti = new translationInformation();

        ti.twoLetterISOCode = ISOCode;
        ti.translation = translation;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + Path.DirectorySeparatorChar + "Translation.fmc");
        bf.Serialize(file, ti);
        file.Close();
    }

    public Dictionary<FMC_Translation.translations, List<string>> loadTranslation(FMC_Translation translationScript)
    {
        Dictionary<FMC_Translation.translations, List<string>> translation = null;

        if (File.Exists(Application.persistentDataPath + Path.DirectorySeparatorChar + "Translation.fmc"))
        {
            translationInformation ti = new translationInformation();
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + Path.DirectorySeparatorChar + "Translation.fmc", FileMode.Open);
            ti = (translationInformation)bf.Deserialize(file);
            file.Close();

            if (translationScript.Get2LetterISOCodeFromSystemLanguage() == ti.twoLetterISOCode)
                translation = ti.translation;
        }

        return translation;
    }
}
