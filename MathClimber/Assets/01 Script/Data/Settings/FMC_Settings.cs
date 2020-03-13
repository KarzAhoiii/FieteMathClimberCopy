using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FMC_Settings : MonoBehaviour
{

    public enum numberType { core, neighbour01, neighbour02, mixed, even, uneven };

    public bool fullyInitialised { get; private set; }
    
    public string _settingsHeadline { get; protected set; }
    public int _rangeOfNumbers { get; protected set; }
    public numberType _numberTypeFront { get; protected set; }
    public numberType _numberTypeBack { get; protected set; }

    public bool _operationPlusIsPossible { get; protected set; }
    public bool _operationTimesIsPossible { get; protected set; }
    public bool _operationMinusIsPossible { get; protected set; }
    public bool _operationDividedIsPossible { get; protected set; }

    public bool _taskTypeGreaterIsPossible { get; protected set; }
    public bool _taskTypeSameIsPossible { get; protected set; }
    public bool _taskTypeSmallerIsPossible { get; protected set; }
    public bool _taskTypeEqualsIsPossible { get; protected set; }
    public bool _taskTypeOneTimesOneIsPossible { get; protected set; }

    public float _timeSpecification { get; protected set; }

    public void setSettings(int rangeOfNumbers, numberType numberTypeFront, numberType numberTypeBack, bool opPlusIsPossible, bool opTimesIsPossible, bool opMinusisPossible,
                            bool opDividedIsPossible, bool ttGreaterIsPossible, bool ttSameIsPossible, bool ttSmallerIsPossible, bool ttEqualsIsPossible,
                            bool ttOneTimesOneIsPossible, float timeSpecification)
    {
        _rangeOfNumbers = rangeOfNumbers;
        _numberTypeFront = numberTypeFront;
        _numberTypeBack = numberTypeBack;
        _operationPlusIsPossible = opPlusIsPossible;
        _operationTimesIsPossible = opTimesIsPossible;
        _operationMinusIsPossible = opMinusisPossible;
        _operationDividedIsPossible = opDividedIsPossible;
        _taskTypeGreaterIsPossible = ttGreaterIsPossible;
        _taskTypeSameIsPossible = ttSameIsPossible;
        _taskTypeSmallerIsPossible = ttSmallerIsPossible;
        //_taskTypeEqualsIsPossible = ttEqualsIsPossible;
        _taskTypeEqualsIsPossible = false; // Always disabled
        _taskTypeOneTimesOneIsPossible = ttOneTimesOneIsPossible;
        _timeSpecification = timeSpecification;

        fullyInitialised = true;
        //logAllSettings();

    }

    public void setSettings(FMC_Settings newSetting)
    {
        _rangeOfNumbers = newSetting._rangeOfNumbers;
        _numberTypeFront = newSetting._numberTypeFront;
        _numberTypeBack = newSetting._numberTypeBack;
        _operationPlusIsPossible = newSetting._operationPlusIsPossible;
        _operationTimesIsPossible = newSetting._operationTimesIsPossible;
        _operationMinusIsPossible = newSetting._operationMinusIsPossible;
        _operationDividedIsPossible = newSetting._operationDividedIsPossible;
        _taskTypeGreaterIsPossible = newSetting._taskTypeGreaterIsPossible;
        _taskTypeSameIsPossible = newSetting._taskTypeSameIsPossible;
        _taskTypeSmallerIsPossible = newSetting._taskTypeSmallerIsPossible;
        //_taskTypeEqualsIsPossible = newSetting._taskTypeEqualsIsPossible;
        _taskTypeEqualsIsPossible = false; // Always Disabled
        _taskTypeOneTimesOneIsPossible = newSetting._taskTypeOneTimesOneIsPossible;
        _timeSpecification = newSetting._timeSpecification;

        fullyInitialised = true;
        //logAllSettings();
    }

    public void logAllSettings ()
    {
        Debug.Log(_rangeOfNumbers + ", " + _numberTypeFront + ", " + _operationPlusIsPossible + ", " + _operationTimesIsPossible + ", " + _operationMinusIsPossible + ", " + _operationDividedIsPossible + ", " + _taskTypeGreaterIsPossible + ", " + _taskTypeSameIsPossible + ", " + _taskTypeEqualsIsPossible + ", " + _taskTypeSmallerIsPossible + ", " + _taskTypeOneTimesOneIsPossible);
    }

    public static string numberTypeToString (numberType numberType)
    {
        if (numberType == numberType.core)
            return "core";
        else if (numberType == numberType.neighbour01)
            return "neighbour01";
        else if (numberType == numberType.neighbour02)
            return "neighbour02";
        else if (numberType == numberType.mixed)
            return "mixed";
        else if (numberType == numberType.even)
            return "even";
        else if (numberType == numberType.uneven)
            return "uneven";
        else
        {
            Debug.LogWarning("No Matching numbertype found.");
            return "mixed";
        }
    }

    public static numberType stringToNumberType (string numberType)
    {
        if (numberType == "core")
            return FMC_Settings.numberType.core;
        else if (numberType == "neighbour01")
            return FMC_Settings.numberType.neighbour01;
        else if (numberType == "neighbour02")
            return FMC_Settings.numberType.neighbour02;
        else if (numberType == "mixed")
            return FMC_Settings.numberType.mixed;
        else if (numberType == "even")
            return FMC_Settings.numberType.even;
        else if (numberType == "uneven")
            return FMC_Settings.numberType.uneven;
        else
        {
            Debug.LogWarning("No Matching numbertype found");
            return FMC_Settings.numberType.mixed;
        }
    }
}
