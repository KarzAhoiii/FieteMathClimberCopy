using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PS_PlayButtonInfo : MonoBehaviour 
{

	public List<int> players;
	public List<string> playerNames;
    public List<PS_InputField.ageTypes> playerAges;
	public PS_PlayerButton playButton;
	public PS_PlayerButtonLayout layout;
    [HideInInspector] public List<Guid> uniqueIDs;

    public delegate void changedPlayerName(string playerName, Guid uniqueID, PS_InputField.ageTypes age);
    public static event changedPlayerName playerNameChanged;

    private void Awake ()
	{
		players = new List<int> ();
		playerNames = new List<string> ();
        playerAges = new List<PS_InputField.ageTypes>();
        uniqueIDs = new List<Guid>();
	}

	public void switchPlayButton()
	{
		for (int i = 0; i < layout.buttons.Count; i++)
		{
			if (layout.buttons [i].GetComponent<PS_PlayerButton> ().isSelected)
			{
				playButton.enableButton ();
				return;
			}
		}

		playButton.disableButton ();
	}

	public void addSelectedPlayer (PS_PlayerButton playerButton)
	{
		layout.addSelectedPlayer (playerButton);
	}

	public void removeSelectedPlayer (PS_PlayerButton playerButton)
	{
		layout.removeSelectedPlayer (playerButton);
	}

	public void startGame()
	{
		players.Clear ();
		playerNames.Clear ();
        playerAges.Clear();

		for (int i = 0; i < layout.buttons.Count; i++)
		{
			if (layout.buttons [i].GetComponent<PS_PlayerButton> ().isSelected)
			{
				players.Add (i);
				playerNames.Add (layout.getPlayername (i));
                uniqueIDs.Add(layout.getPlayerID(i));
                playerAges.Add(layout.getPlayerAge(i));
			}
		}


       // GameObject.Find("DebugLabel").GetComponent<Text>().text = "**** "+playerNameChanged+" / "+playerNames[0];
        GameObject.Find("DebugLabel").GetComponent<Text>().text = "* "+FMC_GameDataController.instance;
        
        FMC_GameDataController.instance.setCurrentPlayer(playerNames[0], uniqueIDs[0], playerAges[0]); // setCurrentPlayer

        //if (playerNameChanged != null)
        //    playerNameChanged(playerNames[0], uniqueIDs[0], playerAges[0]);
       
        GameObject.Find("DebugLabel").GetComponent<Text>().text += "**** "+FLS_LoadingScreen.instance;
        

        if (FLS_LoadingScreen.instance)
            FLS_LoadingScreen.instance.loadScene("Menu01");
        else
        {
            Debug.LogWarning("No Loading Screen Instance");
            SceneManager.LoadScene("Menu01");
        }
	}
}
