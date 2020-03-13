using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FMC_Task
{
    public enum operations { plus, minus, times, divided };
    public enum taskTypes { greater, same, smaller, equals, oneTimesOne };

    public string task { get; private set; }
    public int correctAnswer { get; private set; }
    public int userAnswer { get; private set; }
    public bool answeredCorrectly { get; private set; }
    public float timeNeeded { get; private set; }
    public float timeSpecification { get; private set; }
    public float[] wrongAnswerSelection { get; private set; }

    public int rangeOfNumbers { get; private set; }
    public FMC_Settings.numberType numberTypeFront { get; private set; }
    public FMC_Settings.numberType numberTypeBack { get; private set; }
    public operations operation { get; private set; }
    public taskTypes taskType { get; private set; }
    public bool answeredUsingBomb { get; private set; }
    public int lastNumber { get; private set; }

    public void setParameters(string _task, float _correctAnswer, int _rangeOfNumbers, FMC_Settings.numberType _numberTypeFront, FMC_Settings.numberType _numberTypeBack , operations _operation, taskTypes _taskType, float _timeSpecification, int _lastNumber)
    {
        task = _task;
        correctAnswer = Mathf.RoundToInt(_correctAnswer);
        rangeOfNumbers = _rangeOfNumbers;
        numberTypeFront = _numberTypeFront;
        numberTypeBack = _numberTypeBack;
        operation = _operation;
        taskType = _taskType;
        timeSpecification = _timeSpecification;
        answeredUsingBomb = false;
        lastNumber = _lastNumber;

        wrongAnswerSelection = new float[] { -1, -1, -1 };
        if (rangeOfNumbers >= 10)
        {
            wrongAnswerSelection[0] = Random.Range(0, rangeOfNumbers + 1);
            wrongAnswerSelection[1] = Random.Range(0, rangeOfNumbers + 1);
            wrongAnswerSelection[2] = Random.Range(0, rangeOfNumbers + 1);

            while (wrongAnswerSelection[0] == correctAnswer)
                wrongAnswerSelection[0] = Random.Range(0, rangeOfNumbers + 1);
            while (wrongAnswerSelection[1] == correctAnswer || wrongAnswerSelection[1] == wrongAnswerSelection[0])
                wrongAnswerSelection[1] = Random.Range(0, rangeOfNumbers + 1);
            while (wrongAnswerSelection[2] == correctAnswer || wrongAnswerSelection[2] == wrongAnswerSelection[0] || wrongAnswerSelection[2] == wrongAnswerSelection[1])
                wrongAnswerSelection[2] = Random.Range(0, rangeOfNumbers + 1);
        }

    }

    public void setUserAnswer(float answer, bool usedBomb)
    {
        userAnswer = Mathf.RoundToInt(answer);
        answeredUsingBomb = usedBomb;

        if (userAnswer == correctAnswer && !usedBomb)
            answeredCorrectly = true;
        else
            answeredCorrectly = false;
    }

    public void setTime(float time)
    {
        timeNeeded = time;
    }

    public void wasAnsweredUsingBomb()
    {
        answeredUsingBomb = true;
    }

}
