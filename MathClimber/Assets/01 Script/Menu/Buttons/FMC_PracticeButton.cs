using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FMC_PracticeButton : FMC_ButtonParent, 
IPointerDownHandler,
IPointerUpHandler,
IPointerExitHandler,
IBeginDragHandler,
IDragHandler
{

    public FMC_PracticeBoxLayout practiceBoxLayout;
    public FMC_PracticeBoxLayout.practiceModes practiceMode;
    public FMC_MenuController menuController;
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer line01;
    public SpriteRenderer line02;
    public BoxCollider2D boxCollider;
    //public TextMesh textMesh;
    public Text text;
    public GameObject playButtonSmall;
    public GameObject playButtonFace;

    private bool clickPossible = false;
    private bool isClicked = false;
    private bool isLocked;
    private float initialButtonHeight = -1;
    private FMC_Settings_Input.allInformation problemInfo;

    private void Awake ()
    {
        if (practiceBoxLayout)
            LeanTween.color(gameObject, practiceBoxLayout.upColor, 0.0f);

        //if (practiceBoxLayout && playButtonSmall)
        //    LeanTween.color(playButtonSmall.transform.GetChild(0).gameObject, practiceBoxLayout.upColor, 0.0f);
    }

    private void checkIfEnabled()
    {
        if (!FMC_GameDataController.instance.subscriptionIsActive() && !isAvailableForFree)
        {
            setIapOverlayForPracticeButton(playButtonSmall.transform);
            isLocked = true;
        }
        else
        {
            deactivateIapOverlay();
            isLocked = false;
        }
    }

    public void initialiseCustomExercise (FMC_PracticeBoxLayout _practiceBoxLayout, FMC_MenuController _menuController, FMC_Settings_Input.allInformation _problemInfo)
    {
        isClicked = false;
        clickPossible = false;
        practiceBoxLayout = _practiceBoxLayout;
        menuController = _menuController;
        problemInfo = _problemInfo;
    }

    public void setLayout(float cameraWidth, Vector2 cameraPosition)
    {
        if (spriteRenderer)
            spriteRenderer.size = new Vector2(cameraWidth * 0.9f, spriteRenderer.size.y);

        if (line01)
        {
            line01.size = new Vector2(cameraWidth * 0.975f, line01.size.y);
            line01.transform.position = new Vector3(cameraPosition.x - (cameraWidth * 0.5f) + (cameraWidth * 0.055f), line01.transform.position.y, line01.transform.position.z);
        }

        if (line02)
        {
            line02.size = new Vector2(cameraWidth * 0.975f, line02.size.y);
            line02.transform.position = new Vector3(cameraPosition.x - (cameraWidth * 0.5f) + (cameraWidth * 0.055f), line02.transform.position.y, line02.transform.position.z);
        }

        if (boxCollider)
            boxCollider.size = new Vector2(cameraWidth * 0.9f, boxCollider.size.y);

        if (text)
            text.transform.position = new Vector3(cameraPosition.x - (cameraWidth * 0.5f) + (cameraWidth * 0.055f), text.transform.position.y, text.transform.position.z);

        if (playButtonSmall)
            playButtonSmall.transform.position = new Vector3(cameraPosition.x + (cameraWidth * 0.4f), playButtonSmall.transform.position.y, playButtonSmall.transform.position.z);

        if (playButtonFace)
            initialButtonHeight = playButtonFace.transform.localPosition.y;

        if (iapOverlay)
            setIapOverlayPosition(playButtonSmall.transform.position);
    }

    private void OnEnable ()
    {
        isClicked = false;
        clickPossible = false;
        if (practiceBoxLayout)
            LeanTween.color(gameObject, practiceBoxLayout.upColor, 0.0f);
        if (playButtonFace && initialButtonHeight != -1)
            playButtonFace.transform.localPosition = new Vector3(playButtonFace.transform.localPosition.x, initialButtonHeight, playButtonFace.transform.localPosition.y);

        checkIfEnabled();
    }

	public void OnPointerDown(PointerEventData evd)
	{
        if (!isClicked && !isLocked)
        {
            clickPossible = true;
            animateDown();
        }
	}

	public void OnPointerUp(PointerEventData evd) 
	{
        if (clickPossible && !isClicked)
        {
            LeanTween.delayedCall(practiceBoxLayout.transitionTime, startAction);
            isClicked = true;
            //animateUp();
        }
	}

	public void OnPointerExit (PointerEventData evd)
	{
        if (clickPossible)
        {
            clickPossible = false;
            animateUp();
        }
	}

    public void OnBeginDrag (PointerEventData evd)
    {
        if (clickPossible)
        {
            clickPossible = false;
            animateUp();
        }
    }

    public void OnDrag (PointerEventData evd)
    {
    }

    private void startAction()
	{
        if (practiceMode == FMC_PracticeBoxLayout.practiceModes.showPast)
        {
            //Debug.Log("Insert Logic for Showing past Tasks herer.");
            if (FMC_GameDataController.instance)
                LeanAudio.play(FMC_GameDataController.instance.buttonClickSound, FMC_GameDataController.instance.buttonClickVolume);

            menuController.openPastTasks();
            return;
        }

        else if (practiceMode == FMC_PracticeBoxLayout.practiceModes.customExercise && menuController)
        {
            if (FMC_GameDataController.instance)
                LeanAudio.play(FMC_GameDataController.instance.buttonClickSound, FMC_GameDataController.instance.buttonClickVolume);

            menuController.playCustomExercise(problemInfo);
        }

		else if (menuController)
        {
            if (FMC_GameDataController.instance)
                LeanAudio.play(FMC_GameDataController.instance.buttonClickSound, FMC_GameDataController.instance.buttonClickVolume);

           menuController.playPractice(practiceMode);
        }
	}

    private void animateDown ()
    {
        LeanTween.cancel(gameObject);
        if (initialButtonHeight != -1)
            LeanTween.moveLocalY(playButtonFace, 0.0f, 0.1f);
        //LeanTween.color(gameObject, practiceBoxLayout.downColor, practiceBoxLayout.transitionTime);
    }

    private void animateUp ()
    {
        LeanTween.cancel(gameObject);
        if (initialButtonHeight != -1)
            LeanTween.moveLocalY(playButtonFace, initialButtonHeight, 0.1f);
        //LeanTween.color(gameObject, practiceBoxLayout.upColor, practiceBoxLayout.transitionTime);
    }

}
