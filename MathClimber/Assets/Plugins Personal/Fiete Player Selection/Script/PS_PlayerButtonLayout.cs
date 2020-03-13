using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary; 
using System.IO;
using System;

public class PS_PlayerButtonLayout : MonoBehaviour 
{
	public GameObject playerButtonPrefab;
	public GameObject deleteButtonPrefab;
	public int maxNumberOfPlayers;
	[Range(0,1)] public float rectTopInPercent;
	[Range(0,1)] public float rectBottomInPercent;
	[Range(0,1)] public float buttonWidthInPercent;
	[Range(0,1)] public float buttonSpacingInPercent;
	[Range(0,1)] public float buttonHeightInPercent;
	//public float buttonTextOffsetInUnits;
	public PS_ScrollRect scrollRectScript;
	public PS_PlayerButton editButton;
	public PS_PlayerButton addPlayerButton;
	public PS_PlayButtonInfo playButtonInfo;
	public PS_InputField inputField;
	[HideInInspector] public List<GameObject> buttons;
	[HideInInspector] public List<GameObject> deleteButtons;
	[HideInInspector] public float buttonHeightInUnits;
	[HideInInspector] public float buttonSpacingInUnits;
	[HideInInspector] public float rectTopInUnits;
	[HideInInspector] public float rectBottomInUnits;

	private bool deleteButtonsShow = false;
	private bool deleteButtonsMove = false;
	private int index = 0;
	private float originalHeight;
	private float originalWidth;
	private float actualHeight;
	private float actualWidth;
	//private float checkOffset = -1;
	private float scale;
	private float checkStartX = -1;
	private float checkMoveX;
	private List<string> playerNames;
    private List<Guid> playerIDs;
    private List<PS_InputField.ageTypes> playerAges;
	private List<Button> changeNameButtons;
	private List<GameObject> checks;
	private List<PS_PlayerButton> selectedPlayers;
	private Transform scrollRect;


	void Awake () 
	{
		playerNames = new List<string> ();
        playerIDs = new List<Guid>();
        playerAges = new List<PS_InputField.ageTypes>();

		Load ();

		scrollRect = transform.GetChild (0);
		buttons = new List<GameObject> ();
		deleteButtons = new List<GameObject> ();
		checks = new List<GameObject> ();
		selectedPlayers = new List<PS_PlayerButton> ();
		changeNameButtons = new List<Button> ();

		originalHeight = 5 * 2;
		originalWidth = originalHeight * Camera.main.aspect;
		actualHeight = Camera.main.orthographicSize * 2.0f;
		actualWidth = actualHeight * Camera.main.aspect;

		buttonHeightInUnits = buttonHeightInPercent * actualHeight;
		buttonSpacingInUnits = buttonSpacingInPercent * actualHeight;
		rectTopInUnits = rectTopInPercent * Camera.main.orthographicSize * 2.0f;
		rectBottomInUnits = rectBottomInPercent * Camera.main.orthographicSize * 2.0f;
		scale = Camera.main.orthographicSize / 5;

		for (int i = 0; i < playerNames.Count; i++)
		{
			createButton (i, false);
		}

		if (checks.Count > 0)
		{
			checkStartX = checks [0].transform.position.x;
			checkMoveX = checkStartX - 1.5f * deleteButtons [0].transform.transform.localScale.x * 0.75f;
		}

		if (playerNames.Count > 0)
			inputField.init (true);
		else
			inputField.init(false);

		scrollRectScript.init (this);
	}

	public bool addPlayer(InputField field)
	{
		string name = field.text;
		if (name == "" || playerNames.Contains (name))
			return false;

		playerNames.Add (name);
        playerIDs.Add (Guid.NewGuid());
		createButton (playerNames.Count - 1, true);
		scrollRectScript.init (this);
		//Save ();
        
        if (editButton) {
		    editButton.isDisabled = false;
        }

		if (checkStartX == -1)
		{
			checkStartX = checks [0].transform.position.x;
			checkMoveX = checkStartX - 1.5f * deleteButtons [0].transform.transform.localScale.x * 0.75f;
		}

        return true;
	}

    public void addPlayerAge (PS_InputField.ageTypes newPlayerAge)
    {
        playerAges.Add(newPlayerAge);
        Save();
    }

	public void changePlayerName(int buttonID)
	{
		if (buttonID == -1)
			return;

		inputField.tweenInChangePlayerName (buttonID, playerNames[buttonID]);
	}

	public bool setChangedPlayerName(InputField field, int buttonID) {
        if (field.text == "" || playerNames.Contains(field.text))
            return false;

        Debug.Log ("Falls Daten gespeichert sind, die mit den Namen verknüpft sind: Den auskommentierten Code benutzen und die richtigen Daten eingeben.");
		/*if (File.Exists (Application.persistentDataPath + "/levelData" + playerNames [buttonID] + ".xml"))
		{
			File.Move (Application.persistentDataPath + "/levelData" + playerNames [buttonID] + ".xml", Application.persistentDataPath + "/levelData" + field.text + ".xml");
		}*/

		playerNames[buttonID] = field.text;
		buttons [buttonID].transform.GetChild (1).GetChild (0).GetChild(0).GetComponent<Text>().text = field.text;
		//Save ();
        return true;
	}

    public void setChangedPlayerAge (PS_InputField.ageTypes newAge, int buttonID)
    {
        playerAges[buttonID] = newAge;
        Save();
    }

	public void addSelectedPlayer(PS_PlayerButton playerButton)
	{
		selectedPlayers.Add (playerButton);

		if (selectedPlayers.Count > maxNumberOfPlayers)
		{
			selectedPlayers[0].unselectButton();
			selectedPlayers.RemoveAt (0);
		}
	}

	public void removeSelectedPlayer(PS_PlayerButton playerButton)
	{
		if (selectedPlayers.Contains (playerButton))
		{
			selectedPlayers.Remove(playerButton);
		}
	}

	private void createButton(int buttonNumber, bool newButton)
	{
		GameObject playerButton;
		GameObject deleteButton;
		GameObject check;
		SpriteRenderer spriteRenderer;
		BoxCollider2D boxCollider;
		PS_PlayerButton playerButtonScript;
		PS_DeleteButton deleteButtonScript;
		GameObject go;

		// Defalt Player Button initialisieren
		playerButton = GameObject.Instantiate (playerButtonPrefab);
		playerButton.name = "Player Button " + buttonNumber;
		playerButton.transform.parent = scrollRect;
        playerButtonScript = playerButton.GetComponent<PS_PlayerButton>();

		playerButton.transform.localScale = new Vector3 (scale, scale, scale);
		float yPos = (Camera.main.transform.position.y + Camera.main.orthographicSize) - rectTopInUnits - (buttonHeightInUnits * 0.5f);
		playerButton.transform.localPosition = new Vector3 (0.0f, yPos + (buttonHeightInUnits + buttonSpacingInUnits) * -buttonNumber, 0.0f);
        //spriteRenderer = playerButton.GetComponent<SpriteRenderer> ();
        //spriteRenderer.size = new Vector2 (originalWidth * buttonWidthInPercent, originalHeight * buttonHeightInPercent);
        spriteRenderer = playerButton.transform.Find("Face").gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.size = new Vector2(originalWidth * buttonWidthInPercent, originalHeight * buttonHeightInPercent);
        spriteRenderer.gameObject.transform.localPosition = new Vector3(spriteRenderer.transform.localPosition.x, spriteRenderer.transform.localPosition.y + 0.1f, spriteRenderer.transform.localPosition.z);
        playerButton.transform.Find("Background").gameObject.GetComponent<SpriteRenderer>().size = spriteRenderer.size;
        boxCollider = playerButton.GetComponent<BoxCollider2D> ();
		boxCollider.size = spriteRenderer.size;

        // Canvas des Buttons initialisieren
        go = playerButtonScript.canvas.gameObject;
		go.GetComponent<RectTransform> ().sizeDelta = spriteRenderer.size;
		go.transform.GetChild (0).GetComponent<Text> ().text = playerNames[buttonNumber];
		go.transform.GetChild (1).GetComponent<PS_ChangePlayerNameButton> ().init (this, buttonNumber);
		go.transform.GetChild (1).GetComponent<RectTransform> ().sizeDelta = new Vector2 (originalWidth * buttonWidthInPercent * 0.7f, originalHeight * buttonHeightInPercent);
		go.transform.GetChild (1).transform.position = new Vector3 (go.transform.GetChild (1).transform.position.x - (actualWidth * buttonWidthInPercent * 0.15f), go.transform.GetChild (1).transform.position.y, go.transform.GetChild (1).transform.position.z);
		go.transform.GetChild (1).gameObject.SetActive (false);
		changeNameButtons.Add (go.transform.GetChild (1).GetComponent<Button> ());

		// Den Selected Button initialisieren
		playerButton.transform.Find ("ButtonSelectedOutline").GetComponent<SpriteRenderer> ().size = spriteRenderer.size;
		playerButton.transform.Find ("ButtonSelectedOutline").GetChild(0).GetComponent<SpriteRenderer> ().size = spriteRenderer.size;
		playerButtonScript.startSize = playerButton.transform.localScale;
		playerButtonScript.buttonNumber = buttonNumber;
		playerButtonScript.playButtonInfo = playButtonInfo;
		buttons.Add (playerButton);

        // Check und Text Initialisieren
        check = playerButtonScript.check;
        check.transform.position = new Vector3(playerButton.transform.position.x + spriteRenderer.bounds.extents.x - (0.65f * scale), check.transform.position.y, playerButton.transform.position.z);
        checks.Add (check);
		playerButton.transform.GetChild (0).GetChild (0).transform.position = new Vector3 (playerButton.transform.position.x - spriteRenderer.bounds.extents.x * 0.75f * Camera.main.aspect + (0.4f * scale), playerButton.transform.position.y, playerButton.transform.position.z);
        go.transform.GetChild(0).transform.position = new Vector3(playerButton.transform.position.x - spriteRenderer.bounds.extents.x + 0.5f, check.transform.position.y, playerButton.transform.position.z);

        // Den Löschen-Button initialisieren
        deleteButton = GameObject.Instantiate (deleteButtonPrefab);
		deleteButton.name = "Delete Button " + buttonNumber;
		deleteButton.transform.parent = scrollRect;
		deleteButton.transform.localPosition = new Vector3(actualWidth * 0.52f + scale * 0.5f, playerButton.transform.localPosition.y, deleteButton.transform.position.z);
		deleteButton.transform.localScale = new Vector3 (scale, scale, scale);
		deleteButtonScript = deleteButton.GetComponent<PS_DeleteButton> ();
		deleteButtonScript.init (this, playerButtonScript, playButtonInfo);
		spriteRenderer = deleteButtonScript.buttonSpriteRenderer;
		spriteRenderer.size = new Vector2(originalHeight * buttonHeightInPercent, originalHeight * buttonHeightInPercent);
        spriteRenderer.gameObject.transform.Translate(new Vector3(0, 0.1f, 0));
        deleteButtonScript.backgroundSpriteRenderer.size = spriteRenderer.size;
		boxCollider = spriteRenderer.gameObject.GetComponent<BoxCollider2D> ();
		boxCollider.size = spriteRenderer.size;
		deleteButton.SetActive(false);
		deleteButtons.Add (deleteButton);

        if (newButton || buttonNumber == 0)
            playerButtonScript.startAction();

	}

	public void toggleDeleteButtons()
	{
		if (deleteButtonsMove)
			return;

		index = -1;
		moveDeleteButtons ();
		deleteButtonsMove = true;

		for (int i = 0; i < changeNameButtons.Count; i++)
		{
			if (!changeNameButtons [i].interactable)
			{
				changeNameButtons [i].gameObject.SetActive (true);
				changeNameButtons [i].interactable = true;
			}
			else
			{
				changeNameButtons [i].gameObject.SetActive (false);
				changeNameButtons [i].interactable = false;
			}
		}

		if (!deleteButtonsShow)
			addPlayerButton.disableButton ();
		else
			addPlayerButton.enableButton ();
	}

	private void moveDeleteButtons()
	{
		index++;
		if (index < deleteButtons.Count && !deleteButtonsShow)
		{
			buttons [index].GetComponent<PS_PlayerButton> ().isDisabled = true;
			deleteButtons [index].SetActive (true);
			LeanTween.moveX (deleteButtons [index], deleteButtons [index].transform.position.x - (1.35f * deleteButtons[index].transform.transform.localScale.x), 0.2f);
			LeanTween.moveX (checks [index], checkMoveX, 0.2f);

			buttons [index].GetComponent<PS_PlayerButton> ().scaleButtonDown ();
			LeanTween.delayedCall (0.05f, moveDeleteButtons);
		}

		if (index < deleteButtons.Count && deleteButtonsShow)
		{
			buttons [index].GetComponent<PS_PlayerButton> ().isDisabled = false;
			LeanTween.moveX (deleteButtons [index], deleteButtons [index].transform.position.x + (1.35f * deleteButtons[index].transform.localScale.x), 0.2f);
			LeanTween.moveX (checks [index], checkStartX, 0.2f);

			buttons [index].GetComponent<PS_PlayerButton> ().scaleButtonUp ();
			LeanTween.delayedCall (0.05f, moveDeleteButtons);
		}

		if (index >= deleteButtons.Count && !deleteButtonsShow)
		{
			deleteButtonsShow = true;
			deleteButtonsMove = false;
			return;
		}
		
		if (index >= deleteButtons.Count && deleteButtonsShow)
		{
			LeanTween.delayedCall (0.2f, deactivateDeleteButtons);
		}
			
	}

	private void deactivateDeleteButtons()
	{
		deleteButtonsMove = false;
		deleteButtonsShow = false;

		foreach (GameObject g in deleteButtons)
			g.SetActive (false);
	}

	public void deleteButton(PS_PlayerButton button)
	{
		// Den zu zerstörenden Button ausblenden/kleiner werden lassen.
		/*GameObject go = new GameObject ("Delete");
		go.transform.parent = scrollRect;
		go.transform.position = button.gameObject.transform.position;
		button.gameObject.transform.parent = go.transform;
		deleteButtons [button.buttonNumber].transform.parent = go.transform;
		LeanTween.scale (go, new Vector3(0.0f,0.0f,0.0f), 0.2f).setOnComplete(destroyButton);*/

		scrollRectScript.canMove = false;
		buttons.RemoveAt (button.buttonNumber);
		playerNames.RemoveAt (button.buttonNumber);
        playerIDs.RemoveAt(button.buttonNumber);
        playerAges.RemoveAt(button.buttonNumber);
		deleteButtons.RemoveAt (button.buttonNumber);
		checks.RemoveAt (button.buttonNumber);
		changeNameButtons.RemoveAt (button.buttonNumber);

		if (selectedPlayers.Contains (button))
		{
			selectedPlayers.Remove(button);
		}

		// Schiebt die unteren Buttons nach oben
		for (int i = button.buttonNumber; i < playerNames.Count; i++)
		{
			buttons [i].GetComponent<PS_PlayerButton> ().buttonNumber--;
			LeanTween.moveY (buttons [i], buttons [i].transform.position.y + (originalHeight * buttonHeightInPercent * scale) + buttonSpacingInUnits, 0.2f);

			if (i == playerNames.Count - 1)
				LeanTween.moveY (deleteButtons [i], deleteButtons [i].transform.position.y + buttonHeightInUnits + buttonSpacingInUnits, 0.2f).setOnComplete(reinitScrollRect);
			else
				LeanTween.moveY (deleteButtons [i], deleteButtons [i].transform.position.y + buttonHeightInUnits + buttonSpacingInUnits, 0.2f);
		}

		if (button.buttonNumber == playerNames.Count)
			reinitScrollRect ();

		if (playerNames.Count == 0)
		{
			reinitScrollRect ();
			deleteButtonsShow = false;
            if (editButton) {
			    editButton.isDisabled = true;
            }
		}

		Destroy (button.gameObject);
		Save ();
	}

	private void reinitScrollRect ()
	{
		scrollRectScript.init (this);
	}

	[System.Serializable]
	private struct saveload
	{
		public List<string> players;
        public List<Guid> playerIDs;
        public List<PS_InputField.ageTypes> playerAges;
	}

	private void Save() 
	{
		saveload sl = new saveload ();
		sl.players = playerNames;
        sl.playerIDs = playerIDs;
        sl.playerAges = playerAges;

		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/playerData.data");
		bf.Serialize (file, sl);
		file.Close ();
	}

	private void Load() 
	{
		if(File.Exists(Application.persistentDataPath + "/playerData.data")) 
		{
			saveload sl = new saveload ();
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/playerData.data", FileMode.Open);
			sl = (saveload)bf.Deserialize(file);
			file.Close();

			playerNames = sl.players;
            playerIDs = sl.playerIDs;
            playerAges = sl.playerAges;
		}
	}

	public string getPlayername (int i)
	{
		return playerNames [i];
	}

    public Guid getPlayerID (int i)
    {
        return playerIDs [i];
    }

    public PS_InputField.ageTypes getPlayerAge(int i)
    {
        return playerAges[i];
    }
}
