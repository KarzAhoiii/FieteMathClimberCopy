using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FMC_DimitriPlaceholder : MonoBehaviour
{

    private FMC_GameDataController gameDataController;
    private FMC_Task currentTask;

    private void Awake()
    {
        if (GameObject.FindGameObjectWithTag("GameDataController") && GameObject.FindGameObjectWithTag("GameDataController").GetComponent<FMC_GameDataController>())
        {
            gameDataController = GameObject.FindGameObjectWithTag("GameDataController").GetComponent<FMC_GameDataController>();
        }
        else
        {
            Debug.LogWarning("No Game Data Found");
        }

        FMC_TaskCreation.newTaskCreated += getTask;
        FMC_Statistics.newPowerUp += createPowerUp;

        gameDataController.createFirstTask();
    }

    public void getTask(FMC_Task task)
    {
        currentTask = task;
    }

    public void createPowerUp(string powerUpName)
    {
        //Debug.Log("Create new Power Up: " + powerUpName);
    }

    public void answerTask()
    {
        if (Random.Range(0, 11) > 4)
            currentTask.setUserAnswer(currentTask.correctAnswer, false);
        else
            currentTask.setUserAnswer(200, false);

        currentTask.setTime(Random.Range(0.5f, 10.0f));

        gameDataController.answerTask(currentTask);
    }

    public void loadMenuScene ()
    {
        SceneManager.LoadScene("Menu01");
    }
}
