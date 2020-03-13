using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;

public class ContentData {

	public static string lan;
	public static XmlDocument xmlData;


	public static void loadData () {
		
        lan = LanguageConverter.convertLan(Application.systemLanguage.ToString().ToLower());
		xmlData = new XmlDocument();
		TextAsset textAsset = Resources.Load("Translation/MainScreen") as TextAsset;
		xmlData.LoadXml(textAsset.text);

	}

	public static string getLabelText (string id) {
    
  
		string labelText = "";
		if (xmlData.ChildNodes[0].SelectSingleNode(lan+"/label[@id='"+id+"']") != null) {
			labelText = xmlData.ChildNodes[0].SelectSingleNode(lan+"/label[@id='"+id+"']").InnerText;
		}
        
        if (labelText == "") {
            labelText = xmlData.ChildNodes[0].SelectSingleNode("en/label[@id='"+id+"']").InnerText;
        }
		return labelText;
	}
}