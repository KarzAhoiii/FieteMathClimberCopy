using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class FMC_Statistics : MonoBehaviour
{
    public delegate void createPowerUp(string powerup);
    public static event createPowerUp newPowerUp;

    public Statistics overallStatistics { get; private set; }

    [System.NonSerialized] private FMC_Settings_Controller settingsController;
    private FMC_Settings_StoryMode settingsStoryMode;
    private int powerUpCounter = 0;
    private int popUpCounter_long = 3;
    private int popUpCounter_short = 3;
    private int popUpCounter_wrong = 3;
    private int popUpCounter_right = 10;

    private StreamWriter outputWriter;

    public void Init(FMC_Settings_Controller _settingsController, FMC_Settings_StoryMode _settingsStoryMode)
    {
        settingsController = _settingsController;
        settingsStoryMode = _settingsStoryMode;
        overallStatistics = new Statistics();
    }

    private void startNewSession ()
    {

    }

    public void resetStatistics ()
    {
         overallStatistics = new Statistics();
    }

    public void setAnswer(FMC_Task task)
    {
        advanceCounters();
        overallStatistics.advanceParameters(task);

        if(powerUpCounter > 5)
            checkForPowerUp(overallStatistics);

        if (settingsController.getCurrentSetting() == FMC_Settings_Controller.activeSetting.storyMode && task.answeredCorrectly)
            settingsStoryMode.advanceStep();

        if (settingsController.getCurrentSetting() == FMC_Settings_Controller.activeSetting.storyMode)
            checkForPopUps(overallStatistics);

        // Write Output Info
        //outputWriter = new StreamWriter("Assets/Resources/TextFiles/outputInfo.txt", true);
        //outputWriter.WriteLine(getOutputString(task));
        //outputWriter.Close();

    }

    private void advanceCounters ()
    {
        powerUpCounter++;

        popUpCounter_long = Mathf.Clamp(popUpCounter_long - 1, 0, 20);
        popUpCounter_short = Mathf.Clamp(popUpCounter_short - 1, 0, 20);
        popUpCounter_wrong = Mathf.Clamp(popUpCounter_wrong - 1, 0, 20);
        popUpCounter_right = Mathf.Clamp(popUpCounter_right - 1, 0, 20);
    }

    private string getOutputString(FMC_Task task)
    {
        string output = "";

        output += "Current playmode: " + FMC_GameDataController.instance.getCurrentPlayMode() + ", ";
        output += "Range of numbers: " + task.rangeOfNumbers + ", ";
        output += "Number type: " + task.numberTypeFront + ", ";
        output += "Operation: " + task.operation + ", ";
        output += "Task type: " + task.taskType + ", ";
        output += "Task: " + task.task + ", ";
        output += "Correct answer: " + task.correctAnswer + ", ";
        output += "Answered: " + task.userAnswer + ", ";
        output += "Correct: " + task.answeredCorrectly + ", ";
        output += "Time ultimatum: " + task.timeSpecification + ", ";
        output += "Time needed: " + task.timeNeeded.ToString("F2") + ", ";
        output += "Used Bomb: " + task.answeredUsingBomb + ", ";

        if (settingsController.getCurrentSetting() == FMC_Settings_Controller.activeSetting.storyMode)
            output += "Story mode level: " + settingsStoryMode.level + "/" + settingsStoryMode.levelCount + ",";
        

        return output;
    }

    public void resetPowerUpCounter()
    {
        powerUpCounter = 0;
        popUpCounter_long = 5;
    }

    public void checkForPowerUp(Statistics statistic) {
    
        bool createPowerUp = false;
        string powerUpName = "";
        int rightCounter = 0;
        int wrongCounter = 0;
        
       
        if (statistic.lastTasks.Count > 5)
        {
            for (int i = 0; i < 5; i++)
            {
                if (statistic.lastTasks[i].answeredCorrectly)
                    rightCounter++;
                else if (!statistic.lastTasks[i].answeredCorrectly && !statistic.lastTasks[i].answeredUsingBomb)
                    wrongCounter++;
            }

            if (rightCounter >= 5)
            {
                createPowerUp = true;
                powerUpName = "Rocket";
            }
            else if (wrongCounter >= 2)
            {
                if (wrongCounter >= 4)
                {
                    createPowerUp = true;
                    powerUpName = "Diamond";
                }
                else
                {
                    createPowerUp = true;
                    powerUpName = "Candy";
                }
            } 
        }

        if (createPowerUp)
        {
            powerUpCounter = 0;

            if (newPowerUp != null)
                newPowerUp(powerUpName);
        }
    }

    private void checkForPopUps (Statistics statistic)  {
        
        /*
        int numberOfTasksForLong = 3;
        int numberOfTasksForShort = 3;
        int numberOfTasksForWrong = 3;
        int numberOfTasksForRight = 10;
        int longCounter = 0;
        int shortCounter = 0;
        int wrongCounter = 0;
        int rightCounter = 0;
        */
        
        int numberOfTasksForLong = 5;
        int numberOfTasksForShort = 5;
        int numberOfTasksForWrong = 5;
        int numberOfTasksForRight = 5;
        int longCounter = 0;
        int shortCounter = 0;
        int wrongCounter = 0;
        int rightCounter = 0;
        
        if (popUpCounter_long == 0 && statistic.lastTasks.Count >= numberOfTasksForLong) {
        
            for (int i = 0; i < numberOfTasksForLong; i++) {
                if (statistic.lastTasks[i].timeNeeded > 20.0f)
                    longCounter++;
            }
        }

        if (popUpCounter_wrong == 0 && statistic.lastTasks.Count >= numberOfTasksForWrong) {
            for (int i = 0; i < numberOfTasksForWrong; i++) {
                if (!statistic.lastTasks[i].answeredCorrectly)
                    wrongCounter++;
            }
        }

        if (popUpCounter_short == 0 && statistic.lastTasks.Count >= numberOfTasksForShort) {
            for (int i = 0; i < numberOfTasksForShort; i++) {
                if (statistic.lastTasks[i].timeNeeded <= 3.0f && statistic.lastTasks[i].answeredCorrectly)
                    shortCounter++;
            }
        }

        if (popUpCounter_right == 0 && statistic.lastTasks.Count >= numberOfTasksForRight)  {
            for (int i = 0; i < numberOfTasksForRight; i++) {
                if (statistic.lastTasks[i].answeredCorrectly)
                    rightCounter++;
            }
        }
        
       // Debug.Log("longCounter: "+longCounter+" >= numberOfTasksForLong: "+numberOfTasksForLong+" || wrongCounter: "+wrongCounter+" >= numberOfTasksForWrong: "+numberOfTasksForWrong);
        
       // Debug.Log("shortCounter: "+shortCounter+" >= numberOfTasksForShort: "+numberOfTasksForShort+" || rightCounter: "+rightCounter+" >= rightCounter: "+numberOfTasksForRight);

        if (longCounter >= numberOfTasksForLong || wrongCounter >= numberOfTasksForWrong) {
            popUpCounter_long = 3;
            popUpCounter_short = 3;
            popUpCounter_wrong = 6;
            popUpCounter_right = 6;

            if (newPowerUp != null)
                newPowerUp("makeEasier");
                
        } else if (shortCounter >= numberOfTasksForShort || rightCounter >= numberOfTasksForRight) {
            popUpCounter_long = 3;
            popUpCounter_short = 3;
            popUpCounter_wrong = 6;
            popUpCounter_right = 6;

            if (newPowerUp != null)
                newPowerUp("makeHarder");
        }
    }

    public void setStatisticsFromLoadedData (FMC_Statistics loadedStatistics)
    {
        overallStatistics.setParametersFromLoadedData(loadedStatistics.overallStatistics);
    }

    public class TalentsProblems
    {
        public List<string> talents;
        public List<string> problems;
        public List<FMC_Settings_Input.allInformation> talentsInfo;
        public List<FMC_Settings_Input.allInformation> problemsInfo;

        public TalentsProblems()
        {
            talents = new List<string>();
            problems = new List<string>();
            talentsInfo = new List<FMC_Settings_Input.allInformation>();
            problemsInfo = new List<FMC_Settings_Input.allInformation>();
        }
    }

    [System.Serializable]
    public class Statistics
    {
        public int lifeTimeAnswers { get; private set; }
        public int lifeTimeCorrect { get; private set; }
        public int lifeTimeNotCorrect { get; private set; }
        public TimeSpan lifeTimeTotalTime { get; private set; }
        public float lifeTimeAverageTime { get; private set; }
        public List<FMC_Task> lastTasks { get; private set; }
        public Dictionary<FMC_Settings_Input.allInformation, List<bool>> talentProblemData { get; private set; }
        public List<DataPerDay> dailyData { get; private set; }
        private bool allreadyAddedDailyDataInThisSession = false;

        [System.Serializable]
        public class DataPerDay
        {
            public DateTime date;
            public List<FMC_Task> allSessionTasks;
            public Dictionary<FMC_Settings_Input.allInformation, List<bool>> dailyTalentProblemData;
            public int calculatedTasksTotal;
            public int calculatedTasksTotalWithoutBombs;
            public int correctlyCalculatedTasks;
            public int incorrectlyCalculatedTasks;
            public float correctlyCalculatedInPercent;
            public TimeSpan timeNeededTotal;
            public float timeNeededPerTask;

            public DataPerDay ()
            {
                date = DateTime.Now;
                allSessionTasks = new List<FMC_Task>();
                calculatedTasksTotal = 0;
                calculatedTasksTotalWithoutBombs = 0;
                correctlyCalculatedTasks = 0;
                incorrectlyCalculatedTasks = 0;
                correctlyCalculatedInPercent = 0;
                timeNeededTotal = new TimeSpan(0, 0, 0, 0);
                timeNeededPerTask = 0;

                dailyTalentProblemData = new Dictionary<FMC_Settings_Input.allInformation, List<bool>>();
                for (int i = 0; i < Enum.GetNames(typeof(FMC_Settings_Input.allInformation)).Length; i++)
                {
                    dailyTalentProblemData.Add((FMC_Settings_Input.allInformation)i, new List<bool>());
                }
            }

            public void advanceParameters(FMC_Task task)
            {
                calculatedTasksTotal++;

                if (!task.answeredUsingBomb)
                {
                    calculatedTasksTotalWithoutBombs++;

                    timeNeededTotal += TimeSpan.FromSeconds(task.timeNeeded);
                    timeNeededPerTask = (float)timeNeededTotal.TotalSeconds / calculatedTasksTotal;

                    if (task.answeredCorrectly)
                        correctlyCalculatedTasks++;
                    else
                        incorrectlyCalculatedTasks++;
                }

                correctlyCalculatedInPercent = ((correctlyCalculatedTasks * 100) / calculatedTasksTotalWithoutBombs);
                advanceTalentProblemData(task, dailyTalentProblemData);

                //allSessionTasks.Insert(0, task);
                //if (allSessionTasks.Count > 1000)
                //    allSessionTasks.RemoveAt(allSessionTasks.Count - 1);
                allSessionTasks.Add(task);
                if (allSessionTasks.Count > 2000)
                    allSessionTasks.RemoveAt(0);

            }

            public List<string> getStatisticsTags ()
            {
                List<string> tags = new List<string>();

                tags.Add(FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.statistics][12]); // Alle gelösten Aufgaben
                tags.Add(FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.statistics][14]); // richtig gelöst
                tags.Add(FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.statistics][17]); // Time per Task
                tags.Add(FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.statistics][16]); // Gesamtspielzeit der Session

                return tags;
            }

            public List<string> getStatisticsValues ()
            {
                List<string> values = new List<string>();

                values.Add(calculatedTasksTotal.ToString());
                values.Add(correctlyCalculatedTasks.ToString());
                values.Add(timeNeededPerTask.ToString("F0") + "s");
                values.Add(JF_Utility.convertTimeSpanToString(timeNeededTotal));

                return values;
            }


        }

        public Statistics()
        {
            lifeTimeAnswers = lifeTimeCorrect = lifeTimeNotCorrect = 0;
            lifeTimeTotalTime = new TimeSpan(0, 0, 0, 0);
            lastTasks = new List<FMC_Task>();
            dailyData = new List<DataPerDay>();

            talentProblemData = new Dictionary<FMC_Settings_Input.allInformation, List<bool>>();
            for (int i = 0; i < Enum.GetNames(typeof(FMC_Settings_Input.allInformation)).Length; i++)
            {
                //if ((FMC_Settings_Input.allInformation)i != FMC_Settings_Input.allInformation.tInfinite) // Ausschluss
                talentProblemData.Add((FMC_Settings_Input.allInformation)i, new List<bool>());
            }

            FMC_GameDataController.newSessionStarted += startNewSession;
        }

        public void startNewSession()
        {
            Debug.LogWarning("Start New Session");
            allreadyAddedDailyDataInThisSession = false;

            if (!allreadyAddedDailyDataInThisSession)
            {
                dailyData.Insert(0, new DataPerDay());
                if (dailyData.Count > 7) dailyData.RemoveAt(6);
                allreadyAddedDailyDataInThisSession = true;
            }
        }

        public void setParametersFromLoadedData (Statistics loadedStatistics)
        {
            lifeTimeAnswers = loadedStatistics.lifeTimeAnswers;
            lifeTimeCorrect = loadedStatistics.lifeTimeCorrect;
            lifeTimeNotCorrect = loadedStatistics.lifeTimeNotCorrect;
            lifeTimeTotalTime = loadedStatistics.lifeTimeTotalTime;
            lifeTimeAverageTime = loadedStatistics.lifeTimeAverageTime;
            lastTasks = loadedStatistics.lastTasks;
            talentProblemData = loadedStatistics.talentProblemData;
            dailyData = loadedStatistics.dailyData;

            //logAllStatisticsData();
        }

        public void advanceParameters(FMC_Task task)
        {

            lastTasks.Insert(0, task);
            if (lastTasks.Count > 100)
                lastTasks.RemoveAt(lastTasks.Count - 1);

            // Daily Data for Statistic Bars - Per Session
            if (!allreadyAddedDailyDataInThisSession)
            {
                dailyData.Insert(0, new DataPerDay());
                if (dailyData.Count > 7) dailyData.RemoveAt(6);
                allreadyAddedDailyDataInThisSession = true;
            }

            dailyData[0].advanceParameters(task);

            if (task.answeredUsingBomb)
                return;

            lifeTimeAnswers++;

            if (task.answeredCorrectly)
                lifeTimeCorrect++;
            else
                lifeTimeNotCorrect++;

            lifeTimeTotalTime += TimeSpan.FromSeconds(task.timeNeeded);
            lifeTimeAverageTime = (float)lifeTimeTotalTime.TotalSeconds / lifeTimeAnswers;

            advanceTalentProblemData(task, talentProblemData);

            //Debug.Log("Erfolgsquote heute: " + dailyData[0].calculatedTasksTotal + ", " + dailyData[0].correctlyCalculatedInPercent + ", Time: " + dailyData[0].timeNeededPerTask);
            //Debug.Log("CorrectlyCalculatedToday: " + dailyData[0].correctlyCalculatedTasks);
            //Debug.Log("LifeTime Correct: " + lifeTimeCorrect + ", Not: " + lifeTimeNotCorrect + ", Average Time: " + lifeTimeAverageTime + ", Total Time: " + lifeTimeTotalTime);
        }

        public static void advanceTalentProblemData (FMC_Task task, Dictionary<FMC_Settings_Input.allInformation, List<bool>> data)
        {

            if (task.answeredUsingBomb)
                return;

            if (task.rangeOfNumbers <= 10)
                data[FMC_Settings_Input.allInformation.ron10].Insert(0, task.answeredCorrectly);
            else if (task.rangeOfNumbers <= 20)
                data[FMC_Settings_Input.allInformation.ron20].Insert(0, task.answeredCorrectly);
            else if (task.rangeOfNumbers <= 100)
                data[FMC_Settings_Input.allInformation.ron100].Insert(0, task.answeredCorrectly);
            else if (task.rangeOfNumbers <= 1000)
                data[FMC_Settings_Input.allInformation.ron1000].Insert(0, task.answeredCorrectly);

            // 1x1 Reihen
            if (task.operation == FMC_Task.operations.times && task.lastNumber == 1)
                data[FMC_Settings_Input.allInformation.oxo_1].Insert(0, task.answeredCorrectly);
            if (task.operation == FMC_Task.operations.times && task.lastNumber == 2)
                data[FMC_Settings_Input.allInformation.oxo_2].Insert(0, task.answeredCorrectly);
            if (task.operation == FMC_Task.operations.times && task.lastNumber == 3)
                data[FMC_Settings_Input.allInformation.oxo_3].Insert(0, task.answeredCorrectly);
            if (task.operation == FMC_Task.operations.times && task.lastNumber == 4)
                data[FMC_Settings_Input.allInformation.oxo_4].Insert(0, task.answeredCorrectly);
            if (task.operation == FMC_Task.operations.times && task.lastNumber == 5)
                data[FMC_Settings_Input.allInformation.oxo_5].Insert(0, task.answeredCorrectly);
            if (task.operation == FMC_Task.operations.times && task.lastNumber == 6)
                data[FMC_Settings_Input.allInformation.oxo_6].Insert(0, task.answeredCorrectly);
            if (task.operation == FMC_Task.operations.times && task.lastNumber == 7)
                data[FMC_Settings_Input.allInformation.oxo_7].Insert(0, task.answeredCorrectly);
            if (task.operation == FMC_Task.operations.times && task.lastNumber == 8)
                data[FMC_Settings_Input.allInformation.oxo_8].Insert(0, task.answeredCorrectly);
            if (task.operation == FMC_Task.operations.times && task.lastNumber == 9)
                data[FMC_Settings_Input.allInformation.oxo_9].Insert(0, task.answeredCorrectly);
            if (task.operation == FMC_Task.operations.times && task.lastNumber == 10)
                data[FMC_Settings_Input.allInformation.oxo_10].Insert(0, task.answeredCorrectly);

            if (task.numberTypeFront == FMC_Settings.numberType.core)
                data[FMC_Settings_Input.allInformation.ntCore].Insert(0, task.answeredCorrectly);
            else if (task.numberTypeFront == FMC_Settings.numberType.neighbour01)
                data[FMC_Settings_Input.allInformation.ntNeighbour01].Insert(0, task.answeredCorrectly);
            else if (task.numberTypeFront == FMC_Settings.numberType.neighbour02)
                data[FMC_Settings_Input.allInformation.ntNeighbour02].Insert(0, task.answeredCorrectly);
            else if (task.numberTypeFront == FMC_Settings.numberType.even)
                data[FMC_Settings_Input.allInformation.ntEven].Insert(0, task.answeredCorrectly);

            if (task.numberTypeBack == FMC_Settings.numberType.core && task.numberTypeFront != FMC_Settings.numberType.core)
                data[FMC_Settings_Input.allInformation.ntCore].Insert(0, task.answeredCorrectly);
            else if (task.numberTypeBack == FMC_Settings.numberType.neighbour01 && task.numberTypeFront != FMC_Settings.numberType.neighbour01)
                data[FMC_Settings_Input.allInformation.ntNeighbour01].Insert(0, task.answeredCorrectly);
            else if (task.numberTypeBack == FMC_Settings.numberType.neighbour02 && task.numberTypeFront != FMC_Settings.numberType.neighbour02)
                data[FMC_Settings_Input.allInformation.ntNeighbour02].Insert(0, task.answeredCorrectly);
            else if (task.numberTypeBack == FMC_Settings.numberType.even && task.numberTypeFront != FMC_Settings.numberType.mixed)
                data[FMC_Settings_Input.allInformation.ntEven].Insert(0, task.answeredCorrectly);

            if (task.operation == FMC_Task.operations.plus)
                data[FMC_Settings_Input.allInformation.opPlus].Insert(0, task.answeredCorrectly);
            else if (task.operation == FMC_Task.operations.times)
                data[FMC_Settings_Input.allInformation.opTimes].Insert(0, task.answeredCorrectly);
            else if (task.operation == FMC_Task.operations.minus)
                data[FMC_Settings_Input.allInformation.opMinus].Insert(0, task.answeredCorrectly);
            else if (task.operation == FMC_Task.operations.divided)
                data[FMC_Settings_Input.allInformation.opDivided].Insert(0, task.answeredCorrectly);

            if (task.taskType == FMC_Task.taskTypes.greater)
                data[FMC_Settings_Input.allInformation.ttGreater].Insert(0, task.answeredCorrectly);
            else if (task.taskType == FMC_Task.taskTypes.same)
                data[FMC_Settings_Input.allInformation.ttSame].Insert(0, task.answeredCorrectly);
            else if (task.taskType == FMC_Task.taskTypes.smaller)
                data[FMC_Settings_Input.allInformation.ttSmaler].Insert(0, task.answeredCorrectly);
            else if (task.taskType == FMC_Task.taskTypes.equals)
                data[FMC_Settings_Input.allInformation.ttEquals].Insert(0, task.answeredCorrectly);
            //else if (task.taskType == FMC_Task.taskTypes.oneTimesOne)
            //    data[FMC_Settings_Input.allInformation.ttOneTimesOne].Insert(0, task.answeredCorrectly);

            if (task.timeSpecification == -1)
                data[FMC_Settings_Input.allInformation.tInfinite].Insert(0, task.answeredCorrectly);
            else if (task.timeSpecification <= 5 && task.timeSpecification > 0)
                data[FMC_Settings_Input.allInformation.t5].Insert(0, task.answeredCorrectly);
            else if (task.timeSpecification <= 15 && task.timeSpecification > 0)
                data[FMC_Settings_Input.allInformation.t15].Insert(0, task.answeredCorrectly);
            else if (task.timeSpecification <= 30 && task.timeSpecification > 0)
                data[FMC_Settings_Input.allInformation.t30].Insert(0, task.answeredCorrectly);

            for (int i = 0; i < data.Count; i++)
            {
                if (data[(FMC_Settings_Input.allInformation)i].Count > 100)
                {
                    while (data[(FMC_Settings_Input.allInformation)i].Count > 100)
                        data[(FMC_Settings_Input.allInformation)i].RemoveAt(data[(FMC_Settings_Input.allInformation)i].Count-1);
                }
            }

        }

        public TalentsProblems getTalentsAndProblems ()
        {
            return getTalentsAndProblems(talentProblemData);
        }

        public static TalentsProblems getTalentsAndProblems (Dictionary<FMC_Settings_Input.allInformation, List<bool>> data)
        {

            // Easy Settings:
            float percentNeededForItToBeATalent = 80;
            float percentNeededForItToBeAProblem = 60;
            int totalTasksNeededToOutputAnything = 3 - 1;
            int maximalNumberOfEntries = 5;

            TalentsProblems talentsProblems = new TalentsProblems();

            //if (data == null)
            //    return talentsProblems;

            List<KeyValuePair<FMC_Settings_Input.allInformation, float>> possibleTalents = new List<KeyValuePair<FMC_Settings_Input.allInformation, float>>();
            List<KeyValuePair<FMC_Settings_Input.allInformation, float>> possibleProblems = new List<KeyValuePair<FMC_Settings_Input.allInformation, float>>();

            foreach(KeyValuePair<FMC_Settings_Input.allInformation, List<bool>> entry in data)
            {
                if (entry.Key == FMC_Settings_Input.allInformation.tInfinite) // Ausschluss
                    continue;

                int right = 0;
                int wrong = 0;
                foreach (bool b in entry.Value)
                {
                    if (b) right++;
                    else wrong++;
                }

                //if (right > 5)
                //    possibleTalents.Add(new KeyValuePair<FMC_Settings_Input.allInformation, float>(entry.Key, right));
                
                //if (wrong > 5)
                //    possibleProblems.Add(new KeyValuePair<FMC_Settings_Input.allInformation, float>(entry.Key, wrong));

                if (right + wrong > 0)
                {
                    float percentageRight = (right * 100) / (right + wrong);
                    if (percentageRight > percentNeededForItToBeATalent && (right + wrong > totalTasksNeededToOutputAnything))
                        possibleTalents.Add(new KeyValuePair<FMC_Settings_Input.allInformation, float>(entry.Key, percentageRight));
                    else if (percentageRight < percentNeededForItToBeAProblem && (right + wrong > totalTasksNeededToOutputAnything))
                        possibleProblems.Add(new KeyValuePair<FMC_Settings_Input.allInformation, float>(entry.Key, percentageRight));
                }
            }

            possibleTalents.Sort((a, b) => {
                return -1 * a.Value.CompareTo(b.Value);
            });

            possibleProblems.Sort((a, b) => {
                return -1 * a.Value.CompareTo(b.Value);
            });

            //List<string> talents = new List<string>();

            for (int i = 0; i < maximalNumberOfEntries && i < possibleTalents.Count; i++)
            {
                string s = convertTalentProblemDataToString(possibleTalents[i].Key) + "\n";
                //talents.Add(s);
                talentsProblems.talents.Add(s);
                talentsProblems.talentsInfo.Add(possibleTalents[i].Key);
            }

            for (int i = 0; i < maximalNumberOfEntries && i < possibleProblems.Count; i++)
            {
                string s = convertTalentProblemDataToString(possibleProblems[i].Key) + "\n";
                //if (talents.Contains(s))
                //    continue;

                talentsProblems.problems.Add(convertTalentProblemDataToString(possibleProblems[i].Key) + "\n");
                talentsProblems.problemsInfo.Add(possibleProblems[i].Key);
            }

            if (talentsProblems.talents.Count == 0)
                talentsProblems.talents.Add(FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.taskInformation][21]);

            if (talentsProblems.problems.Count == 0)
                talentsProblems.problems.Add(FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.taskInformation][21]);

            //Debug.Log("Talents: " + "Count: " + possibleTalents.Count + "\n" + talentsProblems[0]);
            //Debug.Log("Problems: " + "Count: " + possibleProblems.Count + "\n" + talentsProblems[1]);

            return talentsProblems;
        }

        private static string convertTalentProblemDataToString (FMC_Settings_Input.allInformation information)
        {
            string s = "";
            switch (information)
            {
                case FMC_Settings_Input.allInformation.ron10:
                    s = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.taskInformation][0];
                    break;
                case FMC_Settings_Input.allInformation.ron20:
                    s = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.taskInformation][1];
                    break;
                case FMC_Settings_Input.allInformation.ron100:
                    s = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.taskInformation][2];
                    break;
                case FMC_Settings_Input.allInformation.ron1000:
                    s = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.taskInformation][3];
                    break;
                case FMC_Settings_Input.allInformation.ntCore:
                    s = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.taskInformation][4];
                    break;
                case FMC_Settings_Input.allInformation.ntNeighbour01:
                    s = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.taskInformation][5];
                    break;
                case FMC_Settings_Input.allInformation.ntNeighbour02:
                    s = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.taskInformation][6];
                    break;
                //case FMC_Settings_Input.allInformation.ntEven:
                //    s = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.taskInformation][5];
                //    break;
                //case FMC_Settings_Input.allInformation.ntUneven:
                //    s = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.taskInformation][6];
                //    break;
                case FMC_Settings_Input.allInformation.ntMixed:
                    s = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.taskInformation][7];
                    break;
                case FMC_Settings_Input.allInformation.opPlus:
                    s = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.taskInformation][8];
                    break;
                case FMC_Settings_Input.allInformation.opTimes:
                    s = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.taskInformation][9];
                    break;
                case FMC_Settings_Input.allInformation.opMinus:
                    s = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.taskInformation][10];
                    break;
                case FMC_Settings_Input.allInformation.opDivided:
                    s = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.taskInformation][11];
                    break;
                case FMC_Settings_Input.allInformation.ttGreater:
                    s = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.taskInformation][12];
                    break;
                case FMC_Settings_Input.allInformation.ttSame:
                    s = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.taskInformation][13];
                    break;
                case FMC_Settings_Input.allInformation.ttSmaler:
                    s = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.taskInformation][14];
                    break;
                case FMC_Settings_Input.allInformation.ttEquals:
                    s = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.taskInformation][15];
                    break;
                case FMC_Settings_Input.allInformation.ttOneTimesOne:
                    s = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.taskInformation][16];
                    break;
                case FMC_Settings_Input.allInformation.tInfinite:
                    s = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.taskInformation][17];
                    break;
                case FMC_Settings_Input.allInformation.t5:
                    s = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.taskInformation][18];
                    break;
                case FMC_Settings_Input.allInformation.t15:
                    s = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.taskInformation][19];
                    break;
                case FMC_Settings_Input.allInformation.t30:
                    s = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.taskInformation][20];
                    break;
                case FMC_Settings_Input.allInformation.oxo_1:
                    s = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.taskInformation][23];
                    break;
                case FMC_Settings_Input.allInformation.oxo_2:
                    s = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.taskInformation][24];
                    break;
                case FMC_Settings_Input.allInformation.oxo_3:
                    s = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.taskInformation][25];
                    break;
                case FMC_Settings_Input.allInformation.oxo_4:
                    s = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.taskInformation][26];
                    break;
                case FMC_Settings_Input.allInformation.oxo_5:
                    s = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.taskInformation][27];
                    break;
                case FMC_Settings_Input.allInformation.oxo_6:
                    s = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.taskInformation][28];
                    break;
                case FMC_Settings_Input.allInformation.oxo_7:
                    s = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.taskInformation][29];
                    break;
                case FMC_Settings_Input.allInformation.oxo_8:
                    s = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.taskInformation][30];
                    break;
                case FMC_Settings_Input.allInformation.oxo_9:
                    s = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.taskInformation][31];
                    break;
                case FMC_Settings_Input.allInformation.oxo_10:
                    s = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.taskInformation][32];
                    break;
            }

            return s;
        }

        private void logAllStatisticsData ()
        {
            Debug.Log("Statistik Parameter: Aufgaben gerechnet: " + lifeTimeAnswers + ", Richtig: " + lifeTimeCorrect + "; Falsch: " + lifeTimeNotCorrect + ", Gesamte Zeit: " + lifeTimeTotalTime + ", Durchschnitt: " + lifeTimeAverageTime + ", Anzahl letzte Aufgaben: " + lastTasks.Count);

            string dd = "";
            foreach (DataPerDay d in dailyData)
            {
                dd += "Tasks Total: " + d.calculatedTasksTotal + ", Tasks Korrekt: " + d.correctlyCalculatedTasks + ", In Prozent: " + d.correctlyCalculatedInPercent + ", Zeit ganz: " + d.timeNeededTotal + ", Zeit pro Task: " + d.timeNeededPerTask + "\n";
            }
            Debug.Log("Daily Data: \n" + dd);

                string data = "";
            for (int i = 0; i < talentProblemData.Count; i++)
            {
                data += Enum.GetNames(typeof(FMC_Settings_Input.allInformation))[i] + ", Anzahl: " + talentProblemData[(FMC_Settings_Input.allInformation)i].Count + ", ";
                int right = 0;
                int wrong = 0;
                foreach (bool b in talentProblemData[(FMC_Settings_Input.allInformation)i])
                {
                    if (b) right++;
                    if (!b) wrong++;
                }
                data += "Richtig: " + right + ", Falsch: " + wrong + "\n";
            }

            Debug.Log("Talent Problem Data: \n" + data);
        }
    }
}


