using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PS_DeleteButton : MonoBehaviour 
{

	public SpriteRenderer buttonSpriteRenderer;
	public SpriteRenderer deleteButtonSpriteRenderer;
    public SpriteRenderer backgroundSpriteRenderer;

	private PS_PlayerButtonLayout layout;
	private PS_PlayerButton playerButton;
	private PS_PlayButtonInfo playButtonInfo;

	// Use this for initialization
	public void init (PS_PlayerButtonLayout _layout, PS_PlayerButton _playerButton, PS_PlayButtonInfo _playButtonInfo) 
	{
		layout = _layout;
		playerButton = _playerButton;
		playButtonInfo = _playButtonInfo;
	}

	public void deleteButton()
	{
		layout.deleteButton (playerButton);
		Destroy (this.gameObject);
		playButtonInfo.switchPlayButton();
	}

}
