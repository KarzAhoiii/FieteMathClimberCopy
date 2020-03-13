using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class FMC_IterateButton : FMC_ButtonParent, 
IPointerDownHandler,
IPointerUpHandler,
IPointerExitHandler
{

    public enum iterationStates { core, neighbour01, neighbour02, mixed };

    public FMC_Settings_Input.allInformation information;
    public TextMesh text;
    public BoxCollider2D boxCollider;
    public SpriteRenderer faceSpriteRenderer;
    public SpriteRenderer backgroundSpriteRenderer;
    public SpriteRenderer backgroundBackgroundSpriteRenderer;
    public Transform numberText;

    [Range(0.0f, 0.5f)] public float height;
    [Range(0.0f, 0.5f)] public float transitionTime;
    public Color downColor;
    public Color upColor;

    private bool clickPossible = false;
    private bool isClicked = false;
    private bool isLocked;
    private Vector3 checkedPosition;
    private Vector3 uncheckedPosition;
    private iterationStates currentState;

    private void Awake ()
    {
        checkedPosition = transform.localPosition;
        uncheckedPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + height, transform.localPosition.z);
        transform.localPosition = uncheckedPosition;
        LeanTween.color(gameObject, upColor, 0.0f);

        currentState = iterationStates.core;
        information = FMC_Settings_Input.allInformation.ntCore;
        text.text = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.taskInformation][4];

    }

    private void OnEnable ()
    {
        isClicked = false;
        clickPossible = false;
        checkIfEnabled();
    }

    private void checkIfEnabled()
    {
        if (!FMC_GameDataController.instance.subscriptionIsActive() && !isAvailableForFree)
        {
            setIapOverlay(backgroundBackgroundSpriteRenderer, 0.07f, transform.parent);
            isLocked = true;
        }
        else
        {
            deactivateIapOverlay();
            isLocked = false;
        }
    }

    public void OnPointerDown(PointerEventData evd)
	{
        if (!isLocked)
        {
            clickPossible = true;
            animateDown();
        }
	}

	public void OnPointerUp(PointerEventData evd) 
	{
        if (clickPossible)
        {
            if (FMC_GameDataController.instance)
                LeanAudio.play(FMC_GameDataController.instance.buttonClickSound, FMC_GameDataController.instance.buttonClickVolume);

            LeanTween.delayedCall(transitionTime, startAction);
            isClicked = true;
            animateUp();
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

	private void startAction()
	{
        iterate();
    }

    public void setState (FMC_Settings.numberType newNumberType)
    {

        if (newNumberType == FMC_Settings.numberType.core)
        {
            currentState = iterationStates.core;
            information = FMC_Settings_Input.allInformation.ntCore;
            text.text = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.taskInformation][4];
        }
        else if (newNumberType == FMC_Settings.numberType.neighbour01)
        {
            currentState = iterationStates.neighbour01;
            information = FMC_Settings_Input.allInformation.ntNeighbour01;
            text.text = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.taskInformation][5];
        }
        else if (newNumberType == FMC_Settings.numberType.neighbour02)
        {
            currentState = iterationStates.neighbour02;
            information = FMC_Settings_Input.allInformation.ntNeighbour02;
            text.text = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.taskInformation][6];
        }
        else if (newNumberType == FMC_Settings.numberType.mixed)
        {
            currentState = iterationStates.mixed;
            information = FMC_Settings_Input.allInformation.ntMixed;
            text.text = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.taskInformation][7];
        }
    }

    private void iterate ()
    {
        if (currentState == iterationStates.core)
        {
            currentState = iterationStates.neighbour01;
            information = FMC_Settings_Input.allInformation.ntNeighbour01;
            text.text = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.taskInformation][5];
        }
        else if (currentState == iterationStates.neighbour01)
        {
            currentState = iterationStates.neighbour02;
            information = FMC_Settings_Input.allInformation.ntNeighbour02;
            text.text = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.taskInformation][6];
        }
        else if (currentState == iterationStates.neighbour02)
        {
            currentState = iterationStates.mixed;
            information = FMC_Settings_Input.allInformation.ntMixed;
            text.text = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.taskInformation][7];
        }
        else if (currentState == iterationStates.mixed)
        {
            currentState = iterationStates.core;
            information = FMC_Settings_Input.allInformation.ntCore;
            text.text = FMC_GameDataController.instance.fullTranslation[FMC_Translation.translations.taskInformation][4];
        }
    }

    //private void setCurrentState (iterationStates newState)
    //{
    //    currentState = newState;
    //    setInformation();
    //}

    //private void setInformation ()
    //{
    //    if (currentState == iterationStates.core)
    //    { }
    //        information = FMC_Settings_Input.allInformation.ntCore;
    //    else if (currentState == iterationStates.neighbour01)
    //        information = FMC_Settings_Input.allInformation.ntNeighbour01;
    //    else if (currentState == iterationStates.neighbour02)
    //        information = FMC_Settings_Input.allInformation.ntNeighbour02;
    //    else if (currentState == iterationStates.mixed)
    //        information = FMC_Settings_Input.allInformation.ntMixed;
    //}

    private void animateDown ()
    {
        LeanTween.cancel(gameObject);
        LeanTween.moveLocalY(gameObject, checkedPosition.y + (Mathf.Abs((uncheckedPosition.y - checkedPosition.y) * 0.25f)), transitionTime);
        LeanTween.color(gameObject, downColor, transitionTime);
    }

    private void animateUp ()
    {
        LeanTween.cancel(gameObject);
        LeanTween.moveLocalY(gameObject, uncheckedPosition.y, transitionTime);
        LeanTween.color(gameObject, upColor, transitionTime);
    }

}
