using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class FMC_StartButton : MonoBehaviour, 
IPointerDownHandler,
IPointerUpHandler,
IPointerExitHandler
{

	public UnityEvent customCallback;
    public AnimationCurve buttonScaleCurve;

    private bool clickPossible = false;
    private Vector3 startScale;

    private int tweenID01;
    private int tweenID02;

    private void Awake ()
	{
        startScale = gameObject.transform.localScale;
        tweenButton();
	}

    private void tweenButton()
    {
        LeanTween.cancel(tweenID01);
        tweenID01 = LeanTween.scale(gameObject, startScale * 1.05f, 3.0f).setEase(buttonScaleCurve).setOnComplete(tweenButton).id;
    }

	public void OnPointerDown(PointerEventData evd)
	{
        LeanTween.cancel(tweenID01);
        clickPossible = true;
        animateDown();
    }

    public void OnPointerUp(PointerEventData evd)
    {
        if (clickPossible)
            animateUpDontStartAgain();
    }

    public void OnPointerExit (PointerEventData evd)
	{
        if (clickPossible)
        {
            clickPossible = false;
            animateUp();
        }
	}

    private void animateDown()
    {
        LeanTween.cancel(tweenID01);
        tweenID01 = LeanTween.scale(gameObject, startScale * 0.9f, 0.05f).id;
    }

    private void animateUp()
    {
        LeanTween.cancel(tweenID01);
        tweenID01 = LeanTween.scale(gameObject, startScale, 0.05f).setOnComplete(tweenButton).id;
    }

    private void animateUpDontStartAgain()
    {
        if (FMC_GameDataController.instance)
            LeanAudio.play(FMC_GameDataController.instance.buttonClickSound, FMC_GameDataController.instance.buttonClickVolume);

        LeanTween.cancel(tweenID01);
        tweenID02 = LeanTween.scale(gameObject, startScale, 0.05f).setOnComplete(startAction).id;
        //Debug.Log(tweenID01 + ", " + tweenID02);
    }

    protected virtual void startAction()
    {
        customCallback.Invoke();
    }

}
