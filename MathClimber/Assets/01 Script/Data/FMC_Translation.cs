using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class FMC_Translation :MonoBehaviour
{

    public enum translations { statistics, taskInformation, inAppPurchase, playerSelection, swipeCards };

    public Dictionary<translations, List<string>> getTranslation()
    {
        Dictionary<translations, List<string>> fullTranslation = new Dictionary<translations, List<string>>();
        List<string> currentTranslation;


        currentTranslation = getTaskInformationTranslation();
        fullTranslation.Add(translations.taskInformation, currentTranslation);

        currentTranslation = getStatisticsTranslation();
        fullTranslation.Add(translations.statistics, currentTranslation);

        currentTranslation = getSwipeCardTranslation();
        fullTranslation.Add(translations.swipeCards, currentTranslation);

        return fullTranslation;
    }

    private List<string> getStatisticsTranslation()
    {
        List<string> text = new List<string>();
        text = loadXML("Statistics");

        if (text.Count < 47)
        {
            List<string> backup = new List<string>();
            backup.Add("Hello"); // 0
            backup.Add("Custom Settings"); // 1
            backup.Add("Basics"); // 2
            backup.Add("Once one"); // 3
            backup.Add("Your Statistic"); // 4
            backup.Add("Talents"); // 5
            backup.Add("Problems"); // 6
            backup.Add("Learning progress"); // 7
            backup.Add("Solved/Session"); // 8
            backup.Add("Succes rate"); // 9
            backup.Add("Time/task"); // 10
            backup.Add("Game progress"); // 11
            backup.Add("Solved tasks"); // 12
            backup.Add("Error rate"); // 13
            backup.Add("Correctly solved"); // 14
            backup.Add("Incorrectly answered"); // 15
            backup.Add("Total playing time"); // 16
            backup.Add("Time per task"); // 17
            backup.Add("Collected coins"); // 18
            backup.Add("Collected characters"); // 19
            backup.Add("SELECTION"); // 20
            backup.Add("Grade exercises"); // 21
            backup.Add("Even"); // 22
            backup.Add("Uneven"); // 23
            backup.Add("Great once"); // 24
            backup.Add("jumped"); // 25
            backup.Add("Thematic exercises"); // 26
            backup.Add("Task overview"); // 27
            backup.Add("First Grade"); // 28
            backup.Add("Second Grade"); // 29
            backup.Add("Third Grade"); // 30
            backup.Add("Fourth Grade"); // 31
            backup.Add("Fifth Grade"); // 32
            backup.Add("Preschool"); // 33
            backup.Add("Addition up to 20"); // 34
            backup.Add("Addition up to 100"); // 35
            backup.Add("Subtraction up to 20"); // 36
            backup.Add("Subtraction up to 100"); // 37
            backup.Add("Multiplication up to 50"); // 38
            backup.Add("Multiplication up to 100"); // 39
            backup.Add("Division up to 50"); // 40
            backup.Add("Division up to 100"); // 41
            backup.Add("Tasks up to 50"); // 42
            backup.Add("Tasks up to 100"); // 43
            backup.Add("Double"); // 44
            backup.Add("Tens boundary"); // 45
            backup.Add("Practice immediately"); // 46

            for (int i = text.Count; text.Count < backup.Count; i++)
                text.Add(backup[i]);
        }

        return text;
    }

    private List<string> getTaskInformationTranslation()
    {
        List<string> text = new List<string>();
        text = loadXML("TaskInformation");

        if (text.Count < 33)
        {
            List<string> backup = new List<string>();
            backup.Add("Numbers to 10"); // 0
            backup.Add("Numbers to 20"); // 1
            backup.Add("Numbers to 100"); // 2
            backup.Add("Numbers to 1000"); // 3
            backup.Add("Core Numbers"); // 4
            backup.Add("Neighbours"); // 5
            backup.Add("Second Neighbours"); // 6
            backup.Add("All Numbers"); // 7
            backup.Add("Addition"); // 8
            backup.Add("Multiplication"); // 9 
            backup.Add("Subtraction"); // 10
            backup.Add("Division"); // 11
            backup.Add("First number bigger"); // 12
            backup.Add("Equal numbers"); // 13
            backup.Add("Last number bigger"); // 14
            backup.Add("Equals range"); // 15
            backup.Add("Once one"); // 16
            backup.Add("No timelimit"); // 17
            backup.Add("Timelimit 5"); // 18
            backup.Add("Timelimit 15"); // 19
            backup.Add("Timelimit 30"); // 20
            backup.Add("None identified"); // 21 
            backup.Add("Not bought"); // 22
            backup.Add("Times row 'one'"); // 23
            backup.Add("Times row 'two'"); // 24
            backup.Add("Times row 'three'"); // 25
            backup.Add("Times row 'four'"); // 26
            backup.Add("Times row 'five'"); // 27
            backup.Add("Times row 'six'"); // 28
            backup.Add("Times row 'seven'"); // 29
            backup.Add("Times row 'eight'"); // 30
            backup.Add("Times row 'nine'"); // 31
            backup.Add("Times row 'ten'"); // 32

            for (int i = text.Count; text.Count < backup.Count; i++)
                text.Add(backup[i]);
        }

        return text;
    }

    private List<string> getSwipeCardTranslation()
    {
        List<string> text = new List<string>();
        text = loadXML("SwipeCards");

        if (text.Count < 19)
        {
            text.Add("Efficient"); // 0
            text.Add("Problem-Recognition"); // 1
            text.Add("Long-Time-Motivation"); // 2
            text.Add("Progress"); // 3
            text.Add("Secure"); // 4
            text.Add("Clarity"); // 5
            text.Add("Choose task type"); // 6
            text.Add("Choose number type"); // 7
            text.Add("Choose row"); // 8
            text.Add("Insert Efficiency text."); // 9
            text.Add("Insert problems text."); // 10
            text.Add("Insert motivation text."); // 11
            text.Add("Insert progress text."); // 12
            text.Add("Insert secure text."); // 13
            text.Add("Insert clarity text."); // 14
            text.Add("Insert task type text."); // 15
            text.Add("Insert number type text."); // 16
            text.Add("Insert row text."); // 17
            text.Add("This will only be saved on your device."); // 18
        }

        return text;
    }

    private List<string> loadXML (string fileName)
    {
        List<string> text = new List<string>();
        string userLanguage = Get2LetterISOCodeFromSystemLanguage();

        TextAsset xmlFile = (TextAsset)Resources.Load(Path.Combine("Translation/", fileName));
        XmlDocument doc = new XmlDocument();
        if (xmlFile)
            doc.LoadXml(xmlFile.text);
            
        if (doc.DocumentElement != null) {
        
            foreach (XmlNode node in doc.DocumentElement.ChildNodes) {
            
                if (node.Attributes["language"].InnerText != "" && node.Attributes["language"].InnerText == userLanguage) {
                
                    foreach (XmlNode child in node.ChildNodes) {
                        text.Add(child.InnerText);
                    }
                }
            }
        }

        return text;
    }

    public string Get2LetterISOCodeFromSystemLanguage()
    {
        SystemLanguage lang = Application.systemLanguage;
        
        string res = "EN";
        switch (lang)
        {
            case SystemLanguage.Afrikaans: res = "AF"; break;
            case SystemLanguage.Arabic: res = "AR"; break;
            case SystemLanguage.Basque: res = "EU"; break;
            case SystemLanguage.Belarusian: res = "BY"; break;
            case SystemLanguage.Bulgarian: res = "BG"; break;
            case SystemLanguage.Catalan: res = "CA"; break;
            case SystemLanguage.Chinese: res = "ZH"; break;
            case SystemLanguage.ChineseSimplified: res = "ZH"; break;
            case SystemLanguage.ChineseTraditional: res = "ZH"; break;
            case SystemLanguage.Czech: res = "CS"; break;
            case SystemLanguage.Danish: res = "DA"; break;
            case SystemLanguage.Dutch: res = "NL"; break;
            case SystemLanguage.English: res = "EN"; break;
            case SystemLanguage.Estonian: res = "ET"; break;
            case SystemLanguage.Faroese: res = "FO"; break;
            case SystemLanguage.Finnish: res = "FI"; break;
            case SystemLanguage.French: res = "FR"; break;
            case SystemLanguage.German: res = "DE"; break;
            case SystemLanguage.Greek: res = "EL"; break;
            case SystemLanguage.Hebrew: res = "IW"; break;
            case SystemLanguage.Hungarian: res = "HU"; break;
            case SystemLanguage.Icelandic: res = "IS"; break;
            case SystemLanguage.Indonesian: res = "IN"; break;
            case SystemLanguage.Italian: res = "IT"; break;
            case SystemLanguage.Japanese: res = "JA"; break;
            case SystemLanguage.Korean: res = "KO"; break;
            case SystemLanguage.Latvian: res = "LV"; break;
            case SystemLanguage.Lithuanian: res = "LT"; break;
            case SystemLanguage.Norwegian: res = "NO"; break;
            case SystemLanguage.Polish: res = "PL"; break;
            case SystemLanguage.Portuguese: res = "PT"; break;
            case SystemLanguage.Romanian: res = "RO"; break;
            case SystemLanguage.Russian: res = "RU"; break;
            case SystemLanguage.SerboCroatian: res = "SH"; break;
            case SystemLanguage.Slovak: res = "SK"; break;
            case SystemLanguage.Slovenian: res = "SL"; break;
            case SystemLanguage.Spanish: res = "ES"; break;
            case SystemLanguage.Swedish: res = "SV"; break;
            case SystemLanguage.Thai: res = "TH"; break;
            case SystemLanguage.Turkish: res = "TR"; break;
            case SystemLanguage.Ukrainian: res = "UK"; break;
            case SystemLanguage.Unknown: res = "EN"; break;
            case SystemLanguage.Vietnamese: res = "VI"; break;
        }
        //		Debug.Log ("Lang: " + res);
   
        return res.ToLower();
    }

}
