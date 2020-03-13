using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PS_WholePlayerSelectionLayout : MonoBehaviour 
{
	// Backgrounds
	public Transform background;
	public Transform playerSelectionBackground;
	public Transform inputFieldBackground;
    public Transform ageSelectionBackground;

	// Overlays
	public Transform overlayTop;
	public Transform overlayBottom;
	public Transform lineTop;
	public Transform lineBottom;

	// Buttons
	public Transform homeButton;
	public Transform editButton;
	public Transform addPlayerButton;
	public Transform playButton;
    public Transform ageButtonTransform;
	public Transform IF_HomeButton;
    public SpriteRenderer chooseAgeBg;
    public SpriteRenderer chooseAgeFace;


    // Input Field
    public Transform IF_Line;
	public RectTransform IF_Canvas;

    // Text for Translation
    public Text choosePlayer;
    public Text addPlayer;
    public Text play;
    public Text nurAufDemGeraet;
    public Text placeholder;
    public Text ageHeader;
    public Text ageButton;

	// Layout Settings in Prozent der Höhe der Kamera
	[Range (0, 1)] public float OverlayTopHeightInPercent;
	[Range (0, 1)] public float OverlayBottomHeightInPercent;
	[Range (0, 1)] public float OverlayLineHeightInPercent;
	[Range (0, 1)] public float circleButtonSizeInPercent;
	[Range (0, 1)] public float rectButtonWidthInPercent;
	[Range (0, 1)] public float rectButtonHeightInPercent;

	private void Awake () {
		
        if (ContentData.xmlData == null) {
            ContentData.loadData();
        }
        setLayout ();
	}

	public void setLayout () {
    
    
		float height = Camera.main.orthographicSize * 2.0f;
		float width = height * Camera.main.aspect;
		float originalHeight = 5 * 2.0f;
		float originalWidth = originalHeight * Camera.main.aspect;
		float scale = Camera.main.orthographicSize / 5;
		Vector3 cameraPosition = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0.0f);

		if (background)
		{
			background.position = cameraPosition;
			background.localScale = new Vector3 (width, height, transform.localScale.z);
		}

		if (playerSelectionBackground)
		{
			playerSelectionBackground.position = cameraPosition;
			playerSelectionBackground.localScale = new Vector3 (width, height, transform.localScale.z);
		}

		if (inputFieldBackground)
		{
			inputFieldBackground.position = cameraPosition;
			inputFieldBackground.localScale = new Vector3 (width, height, transform.localScale.z);
		}

        if (ageSelectionBackground)
        {
            ageSelectionBackground.position = cameraPosition;
            ageSelectionBackground.localScale = new Vector3(width, height, transform.localScale.z);
        }

		if (overlayTop)
		{
			overlayTop.localScale = new Vector3 (width, height * OverlayTopHeightInPercent, 1.0f);
			overlayTop.position = new Vector3 (cameraPosition.x, cameraPosition.y + (height * 0.5f) - (height * OverlayTopHeightInPercent * 0.5f) );
		}

		if (overlayBottom)
		{
			overlayBottom.localScale = new Vector3 (width, height * OverlayBottomHeightInPercent, 1.0f);
			overlayBottom.position = new Vector3 (cameraPosition.x, cameraPosition.y - (height * 0.5f) + (height * OverlayBottomHeightInPercent * 0.5f) );
		}

		if (lineTop)
		{
			lineTop.localScale = new Vector3 (width, height * OverlayLineHeightInPercent, 1.0f);
			lineTop.position = new Vector3 (cameraPosition.x, cameraPosition.y + (height * 0.5f) - (height * OverlayTopHeightInPercent));
		}

		if (lineBottom)
		{
			lineBottom.localScale = new Vector3 (width, height * OverlayLineHeightInPercent, 1.0f);
			lineBottom.position = new Vector3 (cameraPosition.x, cameraPosition.y - (height * 0.5f) + (height * OverlayBottomHeightInPercent));
		}

		// Abstand der Buttons vom Oberen Rand des Bildschirms in Prozent
		float buttonDistance = height * 0.02f;

		if (homeButton)
		{
			//homeButton.localScale = new Vector3 (height * circleButtonSizeInPercent, height * circleButtonSizeInPercent, 1.0f);
			homeButton.position = new Vector3((cameraPosition.x - (width * 0.5f)) + (IF_HomeButton.transform.localScale.x * 1.1f), (cameraPosition.y + (height * 0.5f)) - (IF_HomeButton.transform.localScale.y * 1.1f), 0.0f); // new Vector3 (cameraPosition.x - ((width * 0.5f) * 0.9f) + (homeButton.localScale.x * 0.5f), cameraPosition.y + height * 0.5f - ((homeButton.localScale.x * 0.5f) + buttonDistance), homeButton.position.z);
        }

		if (editButton)
		{
			//editButton.localScale = new Vector3 (height * circleButtonSizeInPercent, height * circleButtonSizeInPercent, 1.0f);
			editButton.position = new Vector3((cameraPosition.x + (width * 0.5f)) - (IF_HomeButton.transform.localScale.x * 1.1f), (cameraPosition.y + (height * 0.5f)) - (IF_HomeButton.transform.localScale.y * 1.1f), 0.0f); //new Vector3 (cameraPosition.x + ((width * 0.5f) * 0.9f) - (editButton.localScale.x * 0.5f), cameraPosition.y + height * 0.5f - ((editButton.localScale.x * 0.5f) + buttonDistance), homeButton.position.z);
        }

		if (IF_HomeButton)
		{
			//IF_HomeButton.localScale = new Vector3 (height * circleButtonSizeInPercent, height * circleButtonSizeInPercent, 1.0f);
			IF_HomeButton.position = new Vector3((cameraPosition.x - (width * 0.5f)) + (IF_HomeButton.transform.localScale.x * 1.1f), (cameraPosition.y + (height * 0.5f)) - (IF_HomeButton.transform.localScale.y * 1.1f), 0.0f);// new Vector3 (cameraPosition.x - ((width * 0.5f) * 1.0f) + (IF_HomeButton.localScale.x * 0.5f), cameraPosition.y + height * 0.5f - ((IF_HomeButton.localScale.x * 0.5f) + buttonDistance), homeButton.position.z);
        }

        if (nurAufDemGeraet)
            nurAufDemGeraet.text = ContentData.getLabelText("dataSecurity");

		// Abstand der rechteckigen Buttons voneinander
		float buttonDistanceFromLine = height * 0.03f;
		float buttonDistanceRect = height * 0.02f;

		if (addPlayerButton)
		{
			addPlayerButton.position = new Vector3 (cameraPosition.x, lineBottom.position.y - ((rectButtonHeightInPercent * height) * 0.5f) - buttonDistanceFromLine, 1.0f);
            PS_PlayerButton playerButton = addPlayerButton.GetComponent<PS_PlayerButton>();
            playerButton.selectedOutlineRenderer.size = new Vector2 (rectButtonWidthInPercent * originalWidth, rectButtonHeightInPercent * originalHeight);
            playerButton.selectedInnerRenderer.size = playerButton.selectedOutlineRenderer.size;
            playerButton.faceGameObject.transform.localPosition = new Vector3(0,0,0);
            playerButton.faceGameObject.transform.Translate(new Vector3(0, 0.1f, 0));
            addPlayerButton.localScale = new Vector3 (scale, scale, scale);

		}

        float cameraHeight = Camera.main.orthographicSize * 2.0f;
        float cameraWidth = cameraHeight * Camera.main.aspect;

        if (chooseAgeFace)
            chooseAgeFace.size = new Vector2(Mathf.Clamp(cameraWidth * 1.3f, 3.0f, 7.3125f), 1.5f);

        if (chooseAgeBg)
            chooseAgeBg.size = chooseAgeFace.size;

        if (playButton)
		{
            playButton.position = new Vector3(cameraPosition.x, addPlayerButton.position.y - (rectButtonHeightInPercent * height) - buttonDistanceRect, 1.0f);
            PS_PlayerButton playerButton = playButton.GetComponent<PS_PlayerButton>();
            playerButton.selectedOutlineRenderer.size = new Vector2(rectButtonWidthInPercent * originalWidth, rectButtonHeightInPercent * originalHeight);
            playerButton.selectedInnerRenderer.size = playerButton.selectedOutlineRenderer.size;
            playerButton.faceGameObject.transform.localPosition = new Vector3(0, 0, 0);
            playerButton.faceGameObject.transform.Translate(new Vector3(0, 0.1f, 0));
            addPlayerButton.localScale = new Vector3(scale, scale, scale);

		}
        
        if (ageButtonTransform)
        {
         
            PS_PlayerButton playerButton = ageButtonTransform.GetComponent<PS_PlayerButton>();
            playerButton.selectedOutlineRenderer.size = new Vector2(rectButtonWidthInPercent * originalWidth, rectButtonHeightInPercent * originalHeight);
            playerButton.selectedInnerRenderer.size = playerButton.selectedOutlineRenderer.size;
            playerButton.faceGameObject.transform.localPosition = new Vector3(0, 0, 0);
            playerButton.faceGameObject.transform.Translate(new Vector3(0, 0.1f, 0));
            addPlayerButton.localScale = new Vector3(scale, scale, scale);

        }

		if (IF_Line)
		{
			IF_Line.position = new Vector3 (cameraPosition.x, cameraPosition.y, 0.0f);
			IF_Line.localScale = new Vector3 (width * 0.6f, height * OverlayLineHeightInPercent * 1.5f, 1.0f);
		}

		if (IF_Canvas)
		{
			float scaleAtSizeFive = 0.003662109f;
			float scale2 = Camera.main.orthographicSize * scaleAtSizeFive / 5.0f;
			IF_Canvas.localScale = new Vector3 (scale2, scale2, scale2);
			IF_Canvas.position = new Vector3 (cameraPosition.x, cameraPosition.y + height * 0.05f, 0.0f);
		}

        if (choosePlayer)
            choosePlayer.text = ContentData.getLabelText("PlayerHeadline").ToUpper();

        if (addPlayer)
            addPlayer.text = ContentData.getLabelText("AddPlayer");

        if (play)
            play.text = ContentData.getLabelText("Player");
        
        if (placeholder)
            placeholder.text = ContentData.getLabelText("Placeholder");
        
        if (ageHeader)
            ageHeader.text = ContentData.getLabelText("AgeHeader").ToUpper();
        
        if (ageButton)
            ageButton.text = ContentData.getLabelText("AgeButton").ToUpper();

    }
}
