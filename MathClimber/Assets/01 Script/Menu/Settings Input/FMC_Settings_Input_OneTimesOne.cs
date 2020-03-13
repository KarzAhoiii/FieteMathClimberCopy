using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMC_Settings_Input_OneTimesOne : MonoBehaviour
{

    public FMC_CheckButtonController checkButtonController;

    private FMC_GameDataController gameDataController;

    private void Awake()
    {
        gameDataController = FMC_GameDataController.instance;
    }

    public void setSettings()
    {
        if (checkButtonController && gameDataController)
        {
             gameDataController.setOneTimesOneSettings(checkOneTimesOneInformation());
        }
    }

    public void setSettingsBig()
    {
        if (checkButtonController && gameDataController)
        {
            gameDataController.setOneTimesOneSettingsBig(checkOneTimesOneInformation());
        }
    }

    private List<int> checkOneTimesOneInformation()
    {
        List<int> includes = new List<int>();
        int buffer = 0;

        for (int i = 0; i < checkButtonController.currentlyCheckedButtons.Count; i++)
        {
            if (int.TryParse(checkButtonController.currentlyCheckedButtons[i].text, out buffer))
                includes.Add(buffer);
        }

        return includes;
    }
}