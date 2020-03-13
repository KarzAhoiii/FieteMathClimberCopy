using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FMC_Settings_Freestyle : FMC_Settings
{
    public FMC_Settings_Freestyle()
    {

        _rangeOfNumbers = 100;
        _numberTypeFront = numberType.mixed;
        _numberTypeBack = _numberTypeFront;

        _operationPlusIsPossible = true;
        _operationMinusIsPossible = true;
        _operationTimesIsPossible = false;
        _operationDividedIsPossible = false;

        _taskTypeGreaterIsPossible = true;
        _taskTypeSameIsPossible = true;
        _taskTypeSmallerIsPossible = true;
        _taskTypeEqualsIsPossible = true;
        _taskTypeOneTimesOneIsPossible = false;
        _timeSpecification = -1;

    }

}
