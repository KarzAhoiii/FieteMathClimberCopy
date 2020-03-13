using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class FMC_Button : MonoBehaviour, 
IPointerDownHandler,
IPointerUpHandler,
IPointerExitHandler
{

	public UnityEvent customCallback;

    private bool clickPossible = false;

    private void Awake ()
	{

	}

	public void OnPointerDown(PointerEventData evd)
	{
        clickPossible = true;
	}

	public void OnPointerUp(PointerEventData evd) 
	{
        if (clickPossible)
        {
            startAction();
        }
	}

	public void OnPointerExit (PointerEventData evd)
	{
        clickPossible = false;
	}

	public void disableButton()
	{

	}

	public void enableButton()
	{

	}

	protected virtual void startAction()
	{
		customCallback.Invoke();
	}
		
}
