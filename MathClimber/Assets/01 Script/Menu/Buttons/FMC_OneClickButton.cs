using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class FMC_OneClickButton : MonoBehaviour, 
IPointerDownHandler,
IPointerUpHandler,
IPointerExitHandler
{

	public UnityEvent customCallback;
    [Range(0.0f, 0.5f)] public float height;
    [Range(0.0f, 0.5f)] public float transitionTime;
    public Color downColor;
    public Color upColor;
    public bool isMultipleClick;
    public bool isStartGameFromSettingsButton;
    public bool isIapButton;

    private bool clickPossible = false;
    private bool isClicked = false;
    private Vector3 checkedPosition;
    private Vector3 uncheckedPosition;
    private Vector3 parentStartPosition;

    private void Awake ()
    {
        checkedPosition = transform.localPosition;
        uncheckedPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + height, transform.localPosition.z);
        transform.localPosition = uncheckedPosition;
        //LeanTween.color(gameObject, upColor, 0.0f);
        if (GetComponent<SpriteRenderer>())
            GetComponent<SpriteRenderer>().color = upColor;
        parentStartPosition = transform.parent.position;

        if (isIapButton && FMC_GameDataController.instance.subscriptionIsActive())
            transform.parent.gameObject.SetActive(false);

    }


    private void OnEnable ()
    {
        isClicked = false;
        clickPossible = false;

        if (isIapButton && FMC_GameDataController.instance.subscriptionIsActive())
        {
            transform.parent.gameObject.SetActive(false);
            return;
        }

        if (isStartGameFromSettingsButton && !FMC_GameDataController.instance.subscriptionIsActive()) {
        
            transform.parent.transform.position = new Vector3(transform.parent.transform.position.x, parentStartPosition.y + 1.0f, transform.parent.transform.position.z);
        } else if (isStartGameFromSettingsButton && FMC_GameDataController.instance.subscriptionIsActive()) {
        
            transform.parent.transform.position = new Vector3(transform.parent.transform.position.x, parentStartPosition.y, transform.parent.transform.position.z);
        }


    }

    public void OnPointerDown(PointerEventData evd)
	{
       
        if (!isClicked || isMultipleClick)
        {
            clickPossible = true;
            animateDown();
        }
	}

	public void OnPointerUp(PointerEventData evd) 
	{
        if (clickPossible && (!isClicked || isMultipleClick))
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
        //Debug.Log("START ACTION: " + isStartGameFromSettingsButton + "; " + SceneManager.GetActiveScene().name);

        if (!isStartGameFromSettingsButton || SceneManager.GetActiveScene().name == "Menu01") {
            customCallback.Invoke();
        }   else if (FMC_GameDataController.instance.menuController != null && SceneManager.GetActiveScene().name != "Menu01") {
            FMC_GameDataController.instance.menuController.gameObject.SetActive(false);
        }
    }

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
