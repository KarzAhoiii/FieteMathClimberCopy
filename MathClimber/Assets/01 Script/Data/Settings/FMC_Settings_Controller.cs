using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMC_Settings_Controller : MonoBehaviour
{
    public enum activeSetting { storyMode, oneTimesOne, oneTimesOneBig, freestyle };

    private FMC_Settings_StoryMode settingStoryMode;
    private FMC_Settings_OneTimesOne settingOneTimesOne;
    private FMC_Settings_OneTimesOne settingOneTimesOneBig;
    private FMC_Settings_Freestyle settingFreestyle;
    private activeSetting currentSetting; // private?!
    
    public static string currentLevelName;

    public void Init (FMC_Settings_StoryMode _settingStoryMode, FMC_Settings_OneTimesOne _settingOneTimesOne, FMC_Settings_OneTimesOne _settingsOneTimesOnebig, FMC_Settings_Freestyle _settingFreestyle)
    {
        currentSetting = activeSetting.storyMode;

        settingStoryMode = _settingStoryMode;
        settingOneTimesOne = _settingOneTimesOne;
        settingOneTimesOneBig = _settingsOneTimesOnebig;
        settingFreestyle = _settingFreestyle;
        
       
    }

    public void setCurrentSettings(activeSetting setting)
    {
        currentSetting = setting;
    }

    public activeSetting getCurrentSetting()
    {
        currentLevelName = "";
        if (currentSetting == activeSetting.storyMode) {
             currentLevelName = settingStoryMode.allLevelSettings[settingStoryMode.level].levelName;
        }
 
        return currentSetting;
    }

    public int getCurrentRangeOfNumbers()
    {
        if (currentSetting == activeSetting.storyMode)
            return settingStoryMode._rangeOfNumbers;
        else if (currentSetting == activeSetting.oneTimesOne)
            return settingOneTimesOne._rangeOfNumbers;
        else if (currentSetting == activeSetting.oneTimesOneBig)
            return settingOneTimesOneBig._rangeOfNumbers;
        else
            return settingFreestyle._rangeOfNumbers;
    }

    public FMC_Settings getCurrentSettingValues()
    {
        if (currentSetting == activeSetting.storyMode)
        {
            return settingStoryMode;
        }
        else if (currentSetting == activeSetting.oneTimesOne)
        {
            return settingOneTimesOne;
        }
        else if (currentSetting == activeSetting.oneTimesOneBig)
        {
            return settingOneTimesOneBig;
        }
        else
        {
            return settingFreestyle;
        }
    }

    public FMC_Settings getCurrentStoryModeSettings ()
    {
        return settingStoryMode;
    }
}
