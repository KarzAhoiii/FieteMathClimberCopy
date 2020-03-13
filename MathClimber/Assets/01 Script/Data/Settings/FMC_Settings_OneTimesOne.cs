using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FMC_Settings_OneTimesOne : FMC_Settings
{
    public List<int> include;

    public FMC_Settings_OneTimesOne()
    {
        _rangeOfNumbers = 20;
        _numberTypeFront = numberType.mixed;
        _numberTypeBack = _numberTypeFront;
        _operationPlusIsPossible = false;
        _operationTimesIsPossible = true;
        _operationMinusIsPossible = false;
        _operationDividedIsPossible = false;

        _taskTypeGreaterIsPossible = false;
        _taskTypeSameIsPossible = false;
        _taskTypeSmallerIsPossible = false;
        _taskTypeEqualsIsPossible = false;
        _taskTypeOneTimesOneIsPossible = true;
        _timeSpecification = -1;

        include = new List<int>();
        List<int> newInclusion = new List<int>();
        newInclusion.Add(2);
        newInclusion.Add(6);
        setInclude(newInclusion);
    }

    public void setOneTimesOneSettings(List<int> includes)
    {
        _rangeOfNumbers = 20;
        _numberTypeFront = numberType.mixed;
        _operationPlusIsPossible = false;
        _operationTimesIsPossible = true;
        _operationMinusIsPossible = false;
        _operationDividedIsPossible = false;

        _taskTypeGreaterIsPossible = false;
        _taskTypeSameIsPossible = false;
        _taskTypeSmallerIsPossible = false;
        _taskTypeEqualsIsPossible = false;
        _taskTypeOneTimesOneIsPossible = true;
        _timeSpecification = -1;

        setInclude(includes);
    }

    private void setInclude(List<int> includeInts)
    {
        include = includeInts;
    }
  
}
