using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PS_PlayerButton : MonoBehaviour, 
IPointerDownHandler,
IPointerUpHandler,
IPointerEnterHandler,
IPointerExitHandler,
IBeginDragHandler,
IDragHandler
{

	// Visuelle Button-Einstellungen
	public bool isDisabled;
	public bool disableAfterClick;
    public bool isDeleteButton;
    public bool isAddPlayerButton;
	[Range(0,1)] 
	public float scaleToPercentOnDown;
	[Range(0,1)] 
	public float scaleToPercentOnUp;
	[Range(0,0.5f)] 
	public float scaleTime;
	public float colorFadeTime;
	public Color hoverColorAdd;
	public Color downColorAdd;
	[Range(0,1f)] 
	public float disabledOpacity;

	public UnityEvent customCallback;
	public int buttonNumber = -1;
	public SpriteRenderer selectedOutlineRenderer;
	public SpriteRenderer selectedInnerRenderer;
	public bool isSelected = false;
	public Vector3 startSize;
    public Canvas canvas;
    public GameObject faceGameObject;
    public GameObject check;
    //public SpriteRenderer faceSpriteRenderer;
    public Color downColor;
    public Color upColor;
	[HideInInspector] public PS_PlayButtonInfo playButtonInfo;

	private bool isClick;
	private bool isHovering;
	private Vector3 startPos;
	private Color startCol;
	private List <SpriteRenderer> allRenderers = new List<SpriteRenderer>();
	private List <Color> allStartColors;

	void Awake ()
	{
		//allRenderers = new List<SpriteRenderer> ();
		allStartColors = new List<Color> ();
		allRenderers.Add(GetComponent<SpriteRenderer>());
		startCol = allRenderers [0].color;
		allStartColors.Add (startCol);

		startSize = transform.localScale;

		/*if (transform.childCount > 0)
		{
			foreach (Transform t in transform)
			{
				if (t.GetComponent<SpriteRenderer> ())
				{
					allRenderers.Add(t.GetComponent<SpriteRenderer>());
					allStartColors.Add(allRenderers[allRenderers.Count-1].color);
				}
			}
		}*/

		if (isDisabled)
			disableButton ();
			
		isClick = false;
		isHovering = false;

	}

    //public void initialise (float scale, float rectTopInUnits, float buttonHeightInUnits, float originalWidth, float buttonWidthInPercent, float originalHeight, float buttonHeightInPercent, float buttonSpacingInUnits, int _buttonNumber, PS_PlayButtonInfo _playButtonInfo, float actualWidth)
    //{
    //    transform.localScale = new Vector3(scale, scale, scale);
    //    float yPos = (Camera.main.transform.position.y + Camera.main.orthographicSize) - rectTopInUnits - (buttonHeightInUnits * 0.5f);
    //    transform.localPosition = new Vector3(0.0f, yPos + (buttonHeightInUnits + buttonSpacingInUnits) * -buttonNumber, 0.0f);
    //    faceSpriteRenderer.size = new Vector2(originalWidth * buttonWidthInPercent, originalHeight * buttonHeightInPercent);
    //    boxCollider.size = faceSpriteRenderer.size;

    //    // Canvas des Buttons initialisieren
    //    go = transform.Find("Canvas").gameObject;
    //    go.GetComponent<RectTransform>().sizeDelta = faceSpriteRenderer.size;
    //    go.transform.GetChild(0).GetComponent<Text>().text = playerNames[buttonNumber];
    //    go.transform.GetChild(1).GetComponent<PS_ChangePlayerNameButton>().init(this, buttonNumber);
    //    go.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(originalWidth * buttonWidthInPercent * 0.7f, originalHeight * buttonHeightInPercent);
    //    go.transform.GetChild(1).transform.position = new Vector3(go.transform.GetChild(1).transform.position.x - (actualWidth * buttonWidthInPercent * 0.15f), go.transform.GetChild(1).transform.position.y, go.transform.GetChild(1).transform.position.z);
    //    go.transform.GetChild(1).gameObject.SetActive(false);
    //    changeNameButtons.Add(go.transform.GetChild(1).GetComponent<Button>());

    //    // Den Selected Button initialisieren
    //    transform.Find("ButtonSelectedOutline").GetComponent<SpriteRenderer>().size = faceSpriteRenderer.size;
    //    transform.Find("ButtonSelectedOutline").GetChild(0).GetComponent<SpriteRenderer>().size = faceSpriteRenderer.size;
    //    startSize = transform.localScale;
    //    buttonNumber = _buttonNumber;
    //    playButtonInfo = _playButtonInfo;
    //    //buttons.Add(playerButton);

    //    // Check und Text Initialisieren
    //    check.transform.position = new Vector3(transform.position.x + faceSpriteRenderer.bounds.extents.x - (0.65f * scale), transform.position.y, transform.position.z);
    //    checks.Add(check);
    //    //transform.GetChild(0).GetChild(0).transform.position = new Vector3(transform.position.x - spriteRenderer.bounds.extents.x * 0.75f * Camera.main.aspect + (0.4f * scale), transform.position.y, transform.position.z);
    //}

	public void OnPointerDown(PointerEventData evd)
	{
		if (isDisabled)
			return;

		startSize = transform.localScale;
		startPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
        //LeanTween.scale (gameObject, new Vector3 (startSize.x * scaleToPercentOnDown, startSize.y * scaleToPercentOnDown, startSize.z * scaleToPercentOnDown), scaleTime).setDelay(0.01f);
        //LeanTween.moveLocalY(faceGameObject, 0, 0.1f);
        //LeanTween.color(faceGameObject, downColor, 0.1f);

        if (isDeleteButton)
        {
            LeanTween.moveLocalY(faceGameObject, 0.0f, 0.1f);
            LeanTween.color(faceGameObject, downColor, 0.1f);
        }

        mixColors (downColorAdd);

		isClick = true;
	}

	public void OnPointerUp(PointerEventData evd) 
	{
		if (isDisabled)
			return;

        //LeanTween.scale (gameObject, new Vector3 (startSize.x * scaleToPercentOnUp, startSize.y * scaleToPercentOnUp, startSize.z * scaleToPercentOnUp), scaleTime);

        if (Vector3.Distance (Camera.main.ScreenToWorldPoint (Input.mousePosition), startPos) > 0.1f)
			isClick = false;

		if (isClick && !LeanTween.isTweening (Camera.main.gameObject))
		{
			startAction();
			if (disableAfterClick)
			{
				disableButton ();
				return;
			}
			isClick = false;
		}

        if (isHovering)
			mixColors(hoverColorAdd);
		else
			mixColors();
	}

	public void OnPointerEnter (PointerEventData evd)
	{
		if (isDisabled)
			return;

		isHovering = true;

		if (!isClick)
			mixColors(hoverColorAdd);
	}

	public void OnPointerExit (PointerEventData evd)
	{
		if (isDisabled)
			return;

		isHovering = false;

		if (!isClick)
			mixColors();
	}

	public void OnBeginDrag(PointerEventData evd)
	{
		LeanTween.cancel (gameObject);

        if (isDeleteButton)
        {
            LeanTween.moveLocalY(faceGameObject, 0.1f, 0.1f);
            LeanTween.color(faceGameObject, upColor, 0.1f);
        }


        //startSize = transform.localScale;

        //if(!isDisabled)
        //LeanTween.scale (gameObject, new Vector3 (startSize.x * scaleToPercentOnUp, startSize.y * scaleToPercentOnUp, startSize.z * scaleToPercentOnUp), scaleTime);
        //LeanTween.moveLocalY(faceGameObject, 0.1f, 0.1f);

    }

    public void OnDrag(PointerEventData evd)
	{
	}

	private void mixColors (Color mixColor)
	{
		for (int i = 0; i < allRenderers.Count; i++)
		{
			Color colorMix = (allStartColors [i] * (1 - mixColor.a)) + (mixColor * mixColor.a);
			colorMix.a = 1;
			//LeanTween.color (allRenderers [i].gameObject, colorMix, colorFadeTime);
		}
	}

	private void mixColors ()
	{
		//for (int i = 0; i < allRenderers.Count; i++)
			//LeanTween.color (allRenderers [i].gameObject, allStartColors[i], colorFadeTime);
	}

	public void disableButton()
	{
        if (isAddPlayerButton || isDeleteButton)
        {
            LeanTween.color(faceGameObject, new Color(0.85f, 0.85f, 0.85f, 1.0f), 0.1f);
            isDisabled = true;
            return;
        }

		for (int i = 0; i < allRenderers.Count; i++)
			LeanTween.color (allRenderers[i].gameObject, new Color (allStartColors[i].r, allStartColors[i].g, allStartColors[i].b, disabledOpacity), colorFadeTime);

		isDisabled = true;
	}

	public void enableButton()
	{
        if (isAddPlayerButton)
        {
            LeanTween.color(faceGameObject, upColor, 0.1f).setDelay(0.1f);
            isDisabled = false;
            return;
        }

        for (int i = 0; i < allRenderers.Count; i++)
			LeanTween.color (allRenderers[i].gameObject, new Color (allStartColors[i].r, allStartColors[i].g, allStartColors[i].b, allStartColors[i].a), colorFadeTime);

		isDisabled = false;
	}

	public void startAction()
	{

        if (FMC_GameDataController.instance && FMC_GameDataController.instance.buttonClickSound)
            LeanAudio.play(FMC_GameDataController.instance.buttonClickSound, FMC_GameDataController.instance.buttonClickVolume);

		customCallback.Invoke();

		if (isSelected)
		{
			isSelected = false;

			if (playButtonInfo && buttonNumber != -1)
				playButtonInfo.removeSelectedPlayer (this);

            if (faceGameObject)
            {
                LeanTween.moveLocalY(faceGameObject, 0.1f, 0.1f);
                LeanTween.color(faceGameObject, Color.white, 0.1f);
            }
        }
		else
		{
			isSelected = true;

			if (playButtonInfo && buttonNumber != -1)
				playButtonInfo.addSelectedPlayer (this);

            if (faceGameObject)
            {
                LeanTween.moveLocalY(faceGameObject, 0.0f, 0.1f);
                LeanTween.color(faceGameObject, downColor, 0.1f);
            }
        }

        if (isAddPlayerButton)
        {
            LeanTween.moveLocalY(faceGameObject, 0.1f, 0.1f).setDelay(0.1f);
            LeanTween.color(faceGameObject, upColor, 0.1f).setDelay(0.1f);
        }

		//if (playButtonInfo && buttonNumber != -1)
		//	playButtonInfo.addRemovePlayer (buttonNumber);
		if (playButtonInfo && buttonNumber != -1)
		{
			playButtonInfo.switchPlayButton ();
		}
	}

	public void unselectButton()
	{
        LeanTween.moveLocalY(faceGameObject, 0.1f, 0.1f);
        LeanTween.color(faceGameObject, Color.white, 0.1f);

        customCallback.Invoke ();
		isSelected = false;
		playButtonInfo.switchPlayButton ();
	}

	public void scaleButtonDown()
	{
		//float offset = (Camera.main.orthographicSize * 1.15f) / 5.0f;
		float offset = 1.15f;
		LeanTween.value(gameObject, tweenSize, allRenderers[0].size.x, allRenderers[0].size.x - offset, 0.2f);
		LeanTween.value (selectedOutlineRenderer.gameObject, tweenSelectedOutlineSize, selectedOutlineRenderer.size.x, selectedOutlineRenderer.size.x - offset, 0.2f);
		LeanTween.value (selectedInnerRenderer.gameObject, tweenSelectedInnerSize, selectedInnerRenderer.size.x, selectedInnerRenderer.size.x - offset, 0.2f);
		LeanTween.moveX (gameObject, transform.position.x - offset * transform.localScale.x * 0.5f, 0.2f);
		LeanTween.moveX (canvas.GetComponent<RectTransform> (), canvas.GetComponent<RectTransform> ().anchoredPosition.x + offset * 0.5f, 0.2f);
	}

	public void scaleButtonUp()
	{
		//float offset = (Camera.main.orthographicSize * 1.15f) / 5.0f;
		float offset = 1.15f;
		LeanTween.value(gameObject, tweenSize, allRenderers[0].size.x, allRenderers[0].size.x + offset, 0.2f);
		LeanTween.value (selectedOutlineRenderer.gameObject, tweenSelectedOutlineSize, selectedOutlineRenderer.size.x, selectedOutlineRenderer.size.x + offset, 0.2f);
		LeanTween.value (selectedInnerRenderer.gameObject, tweenSelectedInnerSize, selectedInnerRenderer.size.x, selectedInnerRenderer.size.x + offset, 0.2f);
		LeanTween.moveX (gameObject, transform.position.x + offset * transform.localScale.x * 0.5f, 0.2f);
		LeanTween.moveX (canvas.GetComponent<RectTransform>(), canvas.GetComponent<RectTransform>().anchoredPosition.x - offset * 0.5f, 0.2f);
	}

	public void tweenSize(float value)
	{
		allRenderers [0].size = new Vector2 (value, allRenderers [0].size.y);
	}

	public void tweenSelectedOutlineSize(float value)
	{
		selectedOutlineRenderer.size = new Vector2 (value, selectedOutlineRenderer.size.y);
	}

	public void tweenSelectedInnerSize(float value)
	{
		selectedInnerRenderer.size = new Vector2 (value, selectedInnerRenderer.size.y);
	}
		
}
