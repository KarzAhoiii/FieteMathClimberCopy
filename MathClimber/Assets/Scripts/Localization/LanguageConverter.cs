using UnityEngine;
using System.Collections;

public class LanguageConverter : MonoBehaviour {

	public static string convertLan(string lanStr) {
		
		string lan = "en";

		switch (lanStr.ToLower()) {
		
			case "danish":
				lan = "da";
				break;
			case "dutch":
				lan = "nl";
				break;
			case "english":
				lan = "en";
				break;
			case "french":
				lan = "fr";
				break;
			case "german":
				lan = "de";
				break;
			case "italian":
				lan = "it";
				break;
			case "norwegian":
				lan = "nor";
				break;
			case "portuguese":
				lan = "pt";
				break;
			case "spanish":
				lan = "es";
				break;
			case "swedish":
				lan = "sv";
				break;
			case "turkish":
				lan = "tr";
				break;
			case "russian":
				lan = "ru";
				break;
			case "thai":
				//lan = "th";
                lan = "en";
				break;
			case "chinese":
				lan = "cn";
				break;
			case "japanese":
				lan = "jp";
				break;
			case "korean":
				lan = "kr";
				break;		
		}
		return lan;
	}
}
