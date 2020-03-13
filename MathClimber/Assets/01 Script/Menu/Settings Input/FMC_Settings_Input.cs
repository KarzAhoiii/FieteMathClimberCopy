using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FMC_Settings_Input : MonoBehaviour
{

    public enum allInformation { ron10, ron20, ron100, ron1000, ntCore, ntNeighbour01, ntNeighbour02, ntEven, ntUneven, ntMixed, opPlus, opTimes, opMinus, opDivided,
                                ttGreater, ttSame, ttSmaler, ttEquals, ttOneTimesOne, tInfinite, t5, t15, t30, oxo_1, oxo_2, oxo_3, oxo_4, oxo_5, oxo_6, oxo_7, oxo_8, oxo_9, oxo_10 };

    public List<FMC_RadioButtonController> radioButtonControllers;
    public List<FMC_CheckButtonController> checkButtonControllers;
    public FMC_IterateButton iterateButton01;
    public FMC_IterateButton iterateButton02;

    private int rangeOfNumbers = 10;
    private FMC_Settings.numberType numberTypeFront = FMC_Settings.numberType.core;
    private FMC_Settings.numberType numbeTypeBack = FMC_Settings.numberType.core;

    private bool operationPlusIsPossible = false;
    private bool operationTimesIsPossible = false;
    private bool operationMinusIsPossible = false;
    private bool operationDividedIsPossible = false;

    private bool taskTypeGreaterIsPossible = false;
    private bool taskTypeSameIsPossible = false;
    private bool taskTypeSmallerIsPossible = false;
    private bool taskTypeEqualsIsPossible = false;
    private bool taskTypeOneTimesOneIsPossible = false;

    private float timeSpecification = -1;

    public void setSettings ()
    {

        resetData();

        foreach (FMC_RadioButtonController rbc in radioButtonControllers)
        {
            checkInformation(rbc.currentlyCheckedButton.information, rbc.currentlyCheckedButton,false, false);
        }

        foreach (FMC_CheckButtonController cbc in checkButtonControllers)
        {
            foreach (FMC_CheckButton b in cbc.currentlyCheckedButtons)
            {
                checkInformation(b.information, null, false, false);
            }
        }

        checkInformation(iterateButton01.information, null, true, false);
        checkInformation(iterateButton02.information, null, false, true);

        FMC_Settings newSetting = new FMC_Settings();
        newSetting.setSettings(rangeOfNumbers, numberTypeFront, numbeTypeBack, operationPlusIsPossible, operationTimesIsPossible, operationMinusIsPossible,
                                operationDividedIsPossible, taskTypeGreaterIsPossible, taskTypeSameIsPossible, taskTypeSmallerIsPossible,
                                taskTypeEqualsIsPossible, taskTypeOneTimesOneIsPossible, timeSpecification);
        //newSetting.setSettings(rangeOfNumbers, numberType, operationPlusIsPossible, operationTimesIsPossible, operationMinusIsPossible,
        //                operationDividedIsPossible, true, true, true, true, false, timeSpecification);

        if (FMC_GameDataController.instance)
            FMC_GameDataController.instance.setSettings(newSetting);

        newSetting.logAllSettings();
    }

    private void checkInformation (allInformation i, FMC_RadioButton button, bool ntFront, bool ntBack)
    {
        switch (i)
        {
            case allInformation.ron10:
                Int32.TryParse(button.text, out rangeOfNumbers);
                break;
            case allInformation.ron20:
                Int32.TryParse(button.text, out rangeOfNumbers);
                break;
            case allInformation.ron100:
                Int32.TryParse(button.text, out rangeOfNumbers);
                break;
            case allInformation.ron1000:
                Int32.TryParse(button.text, out rangeOfNumbers);
                break;
            case allInformation.ntCore:
                if (ntFront)
                    numberTypeFront = FMC_Settings.numberType.core;
                if (ntBack)
                    numbeTypeBack = FMC_Settings.numberType.core;
                break;
            case allInformation.ntNeighbour01:
                if (ntFront)
                    numberTypeFront = FMC_Settings.numberType.neighbour01;
                if (ntBack)
                    numbeTypeBack = FMC_Settings.numberType.neighbour01;
                break;
            case allInformation.ntNeighbour02:
                if (ntFront)
                    numberTypeFront = FMC_Settings.numberType.neighbour02;
                if (ntBack)
                    numbeTypeBack = FMC_Settings.numberType.neighbour02;
                break;
            //case allInformation.ntEven:
            //    numberTypeFront = FMC_Settings.numberType.even;
            //    break;
            //case allInformation.ntUneven:
            //    numberTypeFront = FMC_Settings.numberType.uneven;
            //    break;
            case allInformation.ntMixed:
                if (ntFront)
                    numberTypeFront = FMC_Settings.numberType.mixed;
                if (ntBack)
                    numbeTypeBack = FMC_Settings.numberType.mixed;
                break;
            case allInformation.opPlus:
                operationPlusIsPossible = true;
                break;
            case allInformation.opTimes:
                operationTimesIsPossible = true;
                break;
            case allInformation.opMinus:
                operationMinusIsPossible = true;
                break;
            case allInformation.opDivided:
                operationDividedIsPossible = true;
                break;
            case allInformation.ttGreater:
                taskTypeGreaterIsPossible = true;
                break;
            case allInformation.ttSame:
                taskTypeSameIsPossible = true;
                break;
            case allInformation.ttSmaler:
                taskTypeSmallerIsPossible = true;
                break;
            case allInformation.ttEquals:
                taskTypeEqualsIsPossible = true;
                break;
            case allInformation.ttOneTimesOne:
                taskTypeOneTimesOneIsPossible = true;
                break;
            case allInformation.tInfinite:
                timeSpecification = -1;
                break;
            case allInformation.t5:
                int x = 0;
                Int32.TryParse(button.text, out x);
                timeSpecification = (float)x;
                //timeSpecification = 5;
                break;
            case allInformation.t15:
                int y = 0;
                Int32.TryParse(button.text, out y);
                timeSpecification = (float)y;
                //timeSpecification = 15;
                break;
            case allInformation.t30:
                int z = 0;
                Int32.TryParse(button.text, out z);
                timeSpecification = (float)z;
                //timeSpecification = 30;
                break;
        }
    }

    private void resetData ()
    {
        rangeOfNumbers = 10;
        numberTypeFront = FMC_Settings.numberType.core;

        operationPlusIsPossible = false;
        operationTimesIsPossible = false;
        operationMinusIsPossible = false;
        operationDividedIsPossible = false;

        taskTypeGreaterIsPossible = false;
        taskTypeSameIsPossible = false;
        taskTypeSmallerIsPossible = false;
        taskTypeEqualsIsPossible = false;
        taskTypeOneTimesOneIsPossible = false;

        timeSpecification = -1;
}
   
}
