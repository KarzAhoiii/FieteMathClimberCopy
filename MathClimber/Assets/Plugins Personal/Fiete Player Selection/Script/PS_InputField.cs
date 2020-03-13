using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary; 
using System.IO;

public class PS_InputField : MonoBehaviour 
{

    public enum inputFieldTypes { addPlayer, changeName, selectAge };
    public enum ageTypes { age5, age6, age7, age8, age9, age10 };

	public InputField inputField;
	public PS_PlayerButtonLayout layout;
    public GameObject ageSelection;
    public GameObject playerSelection;
    public FMC_RadioButtonController radioButtonController;

	private int playerButtonID;
    private bool buttonsExist = false;
    private inputFieldTypes currentFieldType;

	public void init(bool _buttonsExist)
	{

        inputField.keyboardType = TouchScreenKeyboardType.Default;
        buttonsExist = _buttonsExist;
        ageSelection.SetActive(false);

	}

    private void initTextEnter ()
    {
        playerSelection.SetActive(false);
    }

    private void initChooseAge()
    {
        playerSelection.SetActive(false);
        gameObject.SetActive(false);
    }

    private void Start ()
    {
        if (buttonsExist)
            gameObject.SetActive(false);
        else
            inputField.ActivateInputField();
    }

	public void tweenInAddPLayer()
	{
		transform.position = new Vector3 (transform.position.x, Camera.main.orthographicSize * 2, transform.position.z);
		gameObject.SetActive (true);
		inputField.text = "";
		inputField.ActivateInputField ();
		LeanTween.moveY (gameObject, 0, 0.5f).setEase (LeanTweenType.easeOutCubic).setOnComplete(initTextEnter);

        currentFieldType = inputFieldTypes.addPlayer;
		playerButtonID = -1;
	}

	public void tweenInChangePlayerName(int buttonID, string buttonText)
	{
		transform.position = new Vector3 (transform.position.x, Camera.main.orthographicSize * 2, transform.position.z);
		gameObject.SetActive (true);
		inputField.text = buttonText;
		inputField.ActivateInputField ();
		LeanTween.moveY (gameObject, 0, 0.5f).setEase (LeanTweenType.easeOutCubic).setOnComplete(initTextEnter);

        currentFieldType = inputFieldTypes.changeName;
		playerButtonID = buttonID;

	}

    public void tweenInChooseAge ()
    {
        ageSelection.SetActive(true);
        ageSelection.transform.position = new Vector3(ageSelection.transform.position.x, Camera.main.orthographicSize * 2, transform.position.z);
        LeanTween.moveY(ageSelection, 0, 0.5f).setEase(LeanTweenType.easeOutCubic).setOnComplete(initChooseAge);
    }

    public void setChanges ()
    {

        if (currentFieldType == inputFieldTypes.changeName)
        {
            if (layout.setChangedPlayerName(inputField, playerButtonID))
                tweenInChooseAge();
            else
                tweenOut();
        }
        else if (currentFieldType == inputFieldTypes.addPlayer)
        {
            if (layout.addPlayer(inputField))
                tweenInChooseAge();
            else
                tweenOut();
        }
    }

    public void setPlayerAge()
    {
        if (currentFieldType == inputFieldTypes.addPlayer)
            layout.addPlayerAge(radioButtonController.currentlyCheckedButton.ageType);
        else if (currentFieldType == inputFieldTypes.changeName)
            layout.setChangedPlayerAge(radioButtonController.currentlyCheckedButton.ageType, playerButtonID);
            
    }

	public void tweenOut ()
	{
        playerSelection.SetActive(true);

        if (FMC_GameDataController.instance && FMC_GameDataController.instance.buttonClickSound)
            LeanAudio.play(FMC_GameDataController.instance.buttonClickSound, FMC_GameDataController.instance.buttonClickVolume);

        LeanTween.moveY (gameObject, Camera.main.orthographicSize * 2, 0.5f).setEase (LeanTweenType.easeOutCubic).setOnComplete(disable);
        LeanTween.moveY (ageSelection, Camera.main.orthographicSize * 2, 0.5f).setEase(LeanTweenType.easeOutCubic);

    }

    public void tweenOutButton()
	{
		LeanTween.moveY (gameObject, transform.position.y + Camera.main.orthographicSize * 2, 0.5f).setEase (LeanTweenType.easeOutCubic).setOnComplete (disable);
	}

	private void disable ()
	{
		inputField.text = "";
		gameObject.SetActive (false);
        ageSelection.SetActive(false);
	}

}
