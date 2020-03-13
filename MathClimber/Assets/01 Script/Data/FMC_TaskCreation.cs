using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Reflection;
using Random = UnityEngine.Random;

public class FMC_TaskCreation : MonoBehaviour
{
    public delegate void createdTask(FMC_Task task);
    public static event createdTask newTaskCreated;

    private FMC_Settings_Controller settingsController;

    // Parameters for each taskCreation
    private int _rangeOfNumbers;
    private int _failSaveCounter = 0;
    private bool _allreadyFellBackToRandom = false;
    private FMC_Settings.numberType _numberTypeFront;
    private FMC_Settings.numberType _numberTypeBack;
    private FMC_Task.operations _operation;
    private FMC_Task.taskTypes _taskType;
    private List<int> _coreNumbers;
    private List<int> _neighbour01Numbers;
    private List<int> _neighbour02Numbers;
    private float x;
    private float y;
    private FMC_Settings currentSetting;
    private FMC_Task lastTask = null;
    private int iterations = 0;
    private string currentMethodId = "";

    public void Init (FMC_Settings_Controller _settingsController)
    {
        settingsController = _settingsController;

        _coreNumbers = new List<int>();
        _coreNumbers.Add(0);
        _coreNumbers.Add(5);

        _neighbour01Numbers = new List<int>();
        _neighbour01Numbers.Add(1);
        _neighbour01Numbers.Add(4);
        _neighbour01Numbers.Add(6);
        _neighbour01Numbers.Add(9);

        _neighbour02Numbers = new List<int>();
        _neighbour02Numbers.Add(2);
        _neighbour02Numbers.Add(3);
        _neighbour02Numbers.Add(7);
        _neighbour02Numbers.Add(8);

        lastTask = null;
        iterations = 0;
    }

    public void createTask()
    {
        currentSetting = settingsController.getCurrentSettingValues();
        _rangeOfNumbers = currentSetting._rangeOfNumbers;
        _numberTypeFront = currentSetting._numberTypeFront;
        _numberTypeBack = currentSetting._numberTypeBack;
        _failSaveCounter = 0;
        _allreadyFellBackToRandom = false;
        //Debug.LogWarning("ntFront: " + _numberTypeFront + "; Back: " + _numberTypeBack);

        if (_numberTypeFront == FMC_Settings.numberType.even || _numberTypeFront == FMC_Settings.numberType.uneven)
            _numberTypeFront = FMC_Settings.numberType.mixed;
        if (_numberTypeBack == FMC_Settings.numberType.even || _numberTypeFront == FMC_Settings.numberType.uneven)
            _numberTypeBack = FMC_Settings.numberType.mixed;

        x = -1;
        y = -1;

        findMethod();

        if (x == -1 && y == -1)
            fallbackRandom();

        FMC_Task t = new FMC_Task();
        setTaskValues(t);

        if (lastTask == null || lastTask.task != t.task || iterations >= 20)
        {
            if (newTaskCreated != null)
                newTaskCreated(t);

            lastTask = t;
            iterations = 0;

            //t.setUserAnswer(t.correctAnswer, false);
            //FMC_GameDataController.instance.answerTask(t);
        }
        else
        {
            iterations++;
            createTask();
        }

    }

    private void findMethod()
    {
        currentMethodId = "";

        // Operation
        List<string> possibleOperations = new List<string>();
        if (currentSetting._operationPlusIsPossible)
            possibleOperations.Add("Plus");
        if (currentSetting._operationTimesIsPossible)
            possibleOperations.Add("Times");
        if (currentSetting._operationMinusIsPossible)
            possibleOperations.Add("Minus");
        if (currentSetting._operationDividedIsPossible)
            possibleOperations.Add("Divided");
        if (possibleOperations.Count == 0)
        {
            possibleOperations.Add("Plus");
            Debug.LogWarning("Operation nicht richtig eingestellt.");
        }
       
        string chosenOperation = possibleOperations[Random.Range(0, possibleOperations.Count)];
        currentMethodId += chosenOperation;

        if (chosenOperation == "Plus")
            _operation = FMC_Task.operations.plus;
        else if (chosenOperation == "Times")
            _operation = FMC_Task.operations.times;
        else if (chosenOperation == "Minus")
            _operation = FMC_Task.operations.minus;
        else
            _operation = FMC_Task.operations.divided;

        // Task Type
        List<string> possibleTaskTypes = new List<string>();

        if (currentSetting._taskTypeGreaterIsPossible)
            possibleTaskTypes.Add("FrontIsGreater");
        if (currentSetting._taskTypeSameIsPossible)
            possibleTaskTypes.Add("Same");
        if (currentSetting._taskTypeSmallerIsPossible)
            possibleTaskTypes.Add("FrontIsSmaller");
        if (currentSetting._taskTypeEqualsIsPossible)
        {
            Debug.LogWarning("Task Type == Ergibt Zahlenraum existiert nicht mehr.");
        }
        if (currentSetting._taskTypeOneTimesOneIsPossible)
        {
            oneTimesOne();
            _taskType = FMC_Task.taskTypes.oneTimesOne;
            return;
        }
        if (possibleTaskTypes.Count == 0)
        {
            possibleTaskTypes.Add("FrontIsGreater");
            Debug.LogWarning("TaskType nicht richtig eingestellt.");
        }

        int index = Random.Range(0, possibleTaskTypes.Count);
        string chosenTaskType = possibleTaskTypes[index];
        if (chosenTaskType == "Same" && possibleTaskTypes.Count > 1 && Random.Range(1, 11) < 7)
        {
            possibleTaskTypes.RemoveAt(index);
            chosenTaskType = possibleTaskTypes[Random.Range(0, possibleTaskTypes.Count)];
        }
        currentMethodId += chosenTaskType;

        if (chosenTaskType == "FrontIsGreater")
            _taskType = FMC_Task.taskTypes.greater;
        else if (chosenTaskType == "Same")
            _taskType = FMC_Task.taskTypes.same;
        else 
            _taskType = FMC_Task.taskTypes.smaller;

        findCurrentMethod();

    }

    private void findCurrentMethod()
    {
        Type t = typeof(FMC_TaskCreation);

        //Debug.Log("IDENTIFIER: " + id);

        if (t != null)
        {
            MethodInfo method = t.GetMethod(currentMethodId);
            if (method != null)
            {
                method.Invoke(this, null);
            }
        }
    }

    private void setTaskValues(FMC_Task t)
    {
        string task = "";
        float answer = -1;

        if (_operation == FMC_Task.operations.plus)
        {
            task = x + " + " + y;
            answer = x + y;
        }
        else if (_operation == FMC_Task.operations.times)
        {
            task = x + " × " + y;
            answer = x * y;
        }
        else if (_operation == FMC_Task.operations.minus)
        {
            task = x + " - " + y;
            answer = x - y;
        }
        else if (_operation == FMC_Task.operations.divided)
        {
            task = x + " ÷ " + y;
            answer = x / y;
        }

        t.setParameters(task, answer, _rangeOfNumbers, _numberTypeFront, _numberTypeBack, _operation, _taskType, currentSetting._timeSpecification, (int)y);

    }

    private void fallbackRandom()
    {
        Debug.LogWarning("No possible combinations found. Fell Back to random Generation.");

        _failSaveCounter = 0;
        _numberTypeFront = FMC_Settings.numberType.mixed;
        _numberTypeBack = FMC_Settings.numberType.mixed;

        if (!_allreadyFellBackToRandom)
            findCurrentMethod();
        else
        {
            Debug.LogWarning("Fell back to complete random generation. This is not great.");
            _operation = FMC_Task.operations.plus;
            x = Random.Range(1, _rangeOfNumbers);
            y = Random.Range(1, _rangeOfNumbers);
        }

        _allreadyFellBackToRandom = true;
    }

    private void switchDigits()
    {
        float buffer = y;
        y = x;
        x = buffer;
    }

    private bool isCorrectNumberType (bool front)
    {
        _failSaveCounter++;
        if (_failSaveCounter > 1500)
            fallbackRandom();

        if (front)
        {
            if (_numberTypeFront == FMC_Settings.numberType.core)
                return _coreNumbers.Contains((int)x % 10) || x == 1;
            else if (_numberTypeFront == FMC_Settings.numberType.neighbour01)
                return (_neighbour01Numbers.Contains((int)x % 10) || x == 2) && x != 1;
            else if (_numberTypeFront == FMC_Settings.numberType.neighbour02)
                return _neighbour02Numbers.Contains((int)x % 10) && x != 2;
            else if (_numberTypeFront == FMC_Settings.numberType.mixed)
                return true;
            else
            {
                Debug.LogWarning("No Matching Type to check for. Setting is uneven or even?");
                return true;
            }
        }
        else
        {
            if (_numberTypeBack == FMC_Settings.numberType.core)
                return _coreNumbers.Contains((int)y % 10) || y == 1;
            else if (_numberTypeBack == FMC_Settings.numberType.neighbour01)
                return (_neighbour01Numbers.Contains((int)y % 10) || y == 2) && y != 1;
            else if (_numberTypeBack == FMC_Settings.numberType.neighbour02)
                return _neighbour02Numbers.Contains((int)y % 10) && y != 2;
            else if (_numberTypeBack == FMC_Settings.numberType.mixed)
                return true;
            else
            {
                Debug.LogWarning("No Matching Type to check for. Setting is uneven or even?");
                return true;
            }
        }
    }

    // Plus --------------------------------
    public void PlusFrontIsGreater()
    {
        x = Random.Range(1, _rangeOfNumbers + 1);
        y = Random.Range(1, _rangeOfNumbers + 1);

        while (!isCorrectNumberType(true) || !isCorrectNumberType(false) || y > x || x == y || x + y > _rangeOfNumbers)
        {
            x = Random.Range(1, _rangeOfNumbers + 1);
            y = Random.Range(1, _rangeOfNumbers + 1);
        }
    }

    public void PlusSame()
    {
        if (_numberTypeFront != _numberTypeBack)
        {
            if (Random.Range(0, 11) > 5)
                PlusFrontIsGreater();
            else
                PlusFrontIsSmaller();
            return;
        }

        x = Random.Range(1, (_rangeOfNumbers / 2) + 1);

        while (!isCorrectNumberType(true))
            x = Random.Range(1, (_rangeOfNumbers / 2) + 1);

        y = x;
    }

    public void PlusFrontIsSmaller()
    {
        x = Random.Range(1, _rangeOfNumbers + 1);
        y = Random.Range(1, _rangeOfNumbers + 1);

        while (!isCorrectNumberType(true) || !isCorrectNumberType(false) || x > y || x == y || x + y > _rangeOfNumbers)
        {
            x = Random.Range(1, _rangeOfNumbers + 1);
            y = Random.Range(1, _rangeOfNumbers + 1);
        }
    }


    // Times ------------------------------------
    public void TimesFrontIsGreater()
    {
        x = Random.Range(1, _rangeOfNumbers + 1);
        y = Random.Range(1, _rangeOfNumbers + 1);

        while (!isCorrectNumberType(true) || !isCorrectNumberType(false) || y > x || x == y || x * y > _rangeOfNumbers)
        {
            x = Random.Range(1, _rangeOfNumbers + 1);
            y = Random.Range(1, _rangeOfNumbers + 1);
        }
    }

    public void TimesSame()
    {
        if (_numberTypeFront != _numberTypeBack)
        {
            if (Random.Range(0, 11) > 5)
                TimesFrontIsGreater();
            else
                TimesFrontIsSmaller();

            return;
        }

        int lowestInt = (int)Math.Floor(Math.Sqrt(_rangeOfNumbers));
        x = (float)Random.Range(1, lowestInt + 1);

        while (!isCorrectNumberType(true))
            x = (float)Random.Range(1, lowestInt + 1);

        y = x;
    }

    public void TimesFrontIsSmaller()
    {
        x = Random.Range(1, _rangeOfNumbers + 1);
        y = Random.Range(1, _rangeOfNumbers + 1);

        while (!isCorrectNumberType(true) || !isCorrectNumberType(false) || x > y || x == y || y * x > _rangeOfNumbers)
        {
            x = Random.Range(1, _rangeOfNumbers + 1);
            y = Random.Range(1, _rangeOfNumbers + 1);
        }
    }

    // Minus ---------------------------------------
    public void MinusFrontIsGreater()
    {
        x = Random.Range(1, _rangeOfNumbers + 1);
        y = Random.Range(1, _rangeOfNumbers + 1);

        while(!isCorrectNumberType(true) || !isCorrectNumberType(false) || x < y || x == y)
        {
            x = Random.Range(1, _rangeOfNumbers + 1);
            y = Random.Range(1, _rangeOfNumbers + 1);
        }
    }

    public void MinusSame()
    {
        if (_numberTypeFront != _numberTypeBack)
        {
            MinusFrontIsGreater();
            return;
        }

        x = Random.Range(1, _rangeOfNumbers + 1);

        while (!isCorrectNumberType(true))
            x = Random.Range(1, _rangeOfNumbers + 1);

        y = x;
    }

    public void MinusFrontIsSmaller()
    {
        Debug.LogWarning("Task Type == Front is Smaller funktioniert nicht mit Operationn == Minus.");
        MinusFrontIsGreater();
    }


    // Divided --------------------------------------
    public void DividedFrontIsGreater()
    {
        x = Random.Range(1, _rangeOfNumbers + 1);
        y = Random.Range(1, _rangeOfNumbers + 1);

        // Check
        while (x % y != 0 || x < y  || x == y)
        {
            x = Random.Range(1, _rangeOfNumbers + 1);
            y = Random.Range(1, _rangeOfNumbers + 1);
        }

        if (_numberTypeFront != FMC_Settings.numberType.mixed || _numberTypeBack != FMC_Settings.numberType.mixed)
            Debug.LogWarning("Zahlenart != mixed. Division immer nur mit gemischten Zahlen möglich.");
    }

    public void DividedSame()
    {
        if (_numberTypeFront != _numberTypeBack)
        {
            DividedFrontIsGreater();
            return;
        }

        x = Random.Range(1, _rangeOfNumbers + 1);

        while (!isCorrectNumberType(true))
            x = Random.Range(1, _rangeOfNumbers + 1);

        y = x;
    }

    public void DividedFrontIsSmaller()
    {
        Debug.LogWarning("Ergibt immer float Zahlen zwischen 0 und 1");
        DividedFrontIsGreater();
    }


    // One Times One ------------------------------------------------------------------------------------------
    private void oneTimesOne ()
    {
        if (currentSetting is FMC_Settings_OneTimesOne)
        {
            FMC_Settings_OneTimesOne s = (FMC_Settings_OneTimesOne)currentSetting;

            y = s.include[Random.Range(0, s.include.Count)];

            if (FMC_GameDataController.instance.getCurrentPlayMode() == FMC_Settings_Controller.activeSetting.oneTimesOne)
                x = Random.Range(1, 11);
            else
                x = Random.Range(10, 21);

        }
        else
        {
            x = Random.Range(1, 10);
            y = x;
        }
    }

}

