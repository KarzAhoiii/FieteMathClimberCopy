using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class FMC_Settings_StoryMode : FMC_Settings
{

    public int step { get; private set; }
    public int level { get; private set; }
    public int levelCount { get; private set; }

    private int stepsNeededForLevelUp;
    public levelSettings[] allLevelSettings;

    public FMC_Settings_StoryMode ()
    {
        step = 0;
        level = 0;
    }

    public void resetData()
    {
        step = 0;
        level = 0;
        createSettingsFromLevel();
    }

    public void makeStoryModeEasier()
    {
        step = 0;
        level -= 1;
        if (level < 0)
            level = 0;

        createSettingsFromLevel();
    }

    public void makeStoryModeHarder ()
    {
        step = 0;
        level += 1;

        createSettingsFromLevel();
    }

    public void loadLevelData()
    {
        levelCollection allLevels;

        string filePath = Path.Combine(Application.streamingAssetsPath, "storyModeLevel.json");
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            allLevels = JsonUtility.FromJson<levelCollection>(jsonData);

            createLevels(allLevels);
            createSettingsFromLevel();
        }
        else
        {
            Debug.LogWarning("Fehler beim Einlesen der JSON Datei.");
        }

        levelCount = allLevelSettings.Length;

    }

    public void advanceStep()
    {
        step++;
        if (step >= stepsNeededForLevelUp)
        {
            advanceLevel();
            //Debug.Log("Story Mode Level: " + level);
        }
        //Debug.Log("Step: " + step + ", Needed Steps: " + stepsNeededForLevelUp);
    }

    public void decreaseStep()
    {
        if (step > 0)
            step--;
    }

    private void advanceLevel()  {
        level++;
        step = 0;
        createSettingsFromLevel();
    }

    public void createSettingsFromLoadedData(int _step, int _level)
    {
        step = _step;
        level = _level;
        createSettingsFromLevel();
    }

    private void createSettingsFromLevel()
    {
        int l = Mathf.Clamp(level, 0, allLevelSettings.Length - 1);
        
        stepsNeededForLevelUp = allLevelSettings[l].stepsNeeded;
        _rangeOfNumbers = allLevelSettings[l].rangeOfNumbers;

        _numberTypeFront = allLevelSettings[l].numberTypeFront;
        _numberTypeBack = allLevelSettings[l].numberTypeBack;

        _operationPlusIsPossible = allLevelSettings[l].operationPlusIsPossible;
        _operationTimesIsPossible = allLevelSettings[l].operationTimesIsPossible;
        _operationMinusIsPossible = allLevelSettings[l].operationMinusIsPossible;
        _operationDividedIsPossible = allLevelSettings[l].operationDividedIsPossible;

        _taskTypeGreaterIsPossible = allLevelSettings[l].taskTypeGreaterIsPossible;
        _taskTypeSameIsPossible = allLevelSettings[l].taskTypeSameIsPossible;
        _taskTypeSmallerIsPossible = allLevelSettings[l].taskTypeSmallerIsPossible;
        _taskTypeEqualsIsPossible = allLevelSettings[l].taskTypeEqualsIsPossible;
        _taskTypeOneTimesOneIsPossible = allLevelSettings[l].taskTypeOneTimesOneIsPossible;
        _timeSpecification = allLevelSettings[l].timeSpecification;
    }

    public void createLevels(levelCollection levelCollection)
    {
        allLevelSettings = new levelSettings[levelCollection.levels.Length];
        for (int i = 0; i < allLevelSettings.Length; i++)
        {
            allLevelSettings[i].levelName = levelCollection.levels[i].Name;
            allLevelSettings[i].rangeOfNumbers = levelCollection.levels[i].Zahlenraum;
            allLevelSettings[i].stepsNeeded = levelCollection.levels[i].Stufen;
            allLevelSettings[i].numberTypeFront = stringToNumberType(levelCollection.levels[i].ZahlenartVorne);
            allLevelSettings[i].numberTypeBack = stringToNumberType(levelCollection.levels[i].ZahlenartHinten);
            allLevelSettings[i].operationPlusIsPossible = levelCollection.levels[i].Plus;
            allLevelSettings[i].operationTimesIsPossible = levelCollection.levels[i].Mal;
            allLevelSettings[i].operationMinusIsPossible = levelCollection.levels[i].Minus;
            allLevelSettings[i].operationDividedIsPossible = levelCollection.levels[i].Geteilt;
            allLevelSettings[i].taskTypeGreaterIsPossible = levelCollection.levels[i].VorneGroesser;
            allLevelSettings[i].taskTypeSameIsPossible = levelCollection.levels[i].Gleich;
            allLevelSettings[i].taskTypeSmallerIsPossible = levelCollection.levels[i].VorneKleiner;
            allLevelSettings[i].taskTypeEqualsIsPossible = levelCollection.levels[i].ErgibtZahlenraum;
            allLevelSettings[i].taskTypeOneTimesOneIsPossible = false;
            allLevelSettings[i].timeSpecification = levelCollection.levels[i].Zeitvorgabe;
        }
    }

    [System.Serializable]
    public struct levelSettings
    {
        public string levelName;
        public int stepsNeeded;
        public int rangeOfNumbers;
        public numberType numberTypeFront;
        public numberType numberTypeBack;
        public bool operationPlusIsPossible;
        public bool operationTimesIsPossible;
        public bool operationMinusIsPossible;
        public bool operationDividedIsPossible;

        public bool taskTypeGreaterIsPossible;
        public bool taskTypeSameIsPossible;
        public bool taskTypeSmallerIsPossible;
        public bool taskTypeEqualsIsPossible;
        public bool taskTypeOneTimesOneIsPossible;

        public float timeSpecification;

    }

    [System.Serializable]
    public struct levelCollection
    {
        public level[] levels;

        [System.Serializable]
        public struct level
        {
            public string Name;
            public int Stufen;
            public int Zahlenraum;
            public string ZahlenartVorne;
            public string ZahlenartHinten;
            public bool Plus;
            public bool Mal;
            public bool Minus;
            public bool Geteilt;
            public bool VorneGroesser;
            public bool Gleich;
            public bool VorneKleiner;
            public bool ErgibtZahlenraum;
            public float Zeitvorgabe;
        }
    }

}