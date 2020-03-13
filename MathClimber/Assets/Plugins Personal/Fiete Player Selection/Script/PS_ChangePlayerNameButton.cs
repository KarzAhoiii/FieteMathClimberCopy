using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PS_ChangePlayerNameButton : MonoBehaviour 
{

	private int buttonID;
	private PS_PlayerButtonLayout layout;

	public void init(PS_PlayerButtonLayout _layout, int _buttonID)
	{
		layout = _layout;
		buttonID = _buttonID;
	}

	public void changePlayerName()
	{
		layout.changePlayerName (buttonID);
	}
}
