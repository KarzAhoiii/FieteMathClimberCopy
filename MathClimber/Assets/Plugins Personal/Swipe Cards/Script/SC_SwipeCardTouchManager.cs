using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_SwipeCardTouchManager : MonoBehaviour
{

    public SC_SwipeCardController swipeCards;
    private bool touchBegan = false;
    private bool isInTouch = false;

    private void Awake()
    {

    }

    void Update()
    {
        if (!Application.isMobilePlatform)
        {
            if (Input.GetMouseButtonDown(0))
            {
                touchBegan = true;
                TouchBegan(Input.mousePosition);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (touchBegan)
                {
                    touchBegan = false;
                    TouchRelease(Input.mousePosition);
                }
            }
            if (touchBegan)
            {
                TouchMoved(Input.mousePosition);
            }
        }
        else
        {
            if (Input.touchCount > 0)
            {
                onScreenTouch();
            }
        }
    }

    private void onScreenTouch()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0]; ;
            if (touch.phase == TouchPhase.Began)
                TouchBegan(touch.position);

            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                TouchMoved(touch.position);

            else if (touch.phase == TouchPhase.Ended)
                TouchRelease(touch.position);

        }
    }

    private void TouchBegan(Vector3 inputPos)
    {
        isInTouch = true;
        inputPos = Camera.main.ScreenToWorldPoint(inputPos);
        inputPos.z = 0;
        swipeCards.inputStart(inputPos);
    }

    private void TouchMoved(Vector3 inputPos)
    {
        if (isInTouch)
        {
            inputPos = Camera.main.ScreenToWorldPoint(inputPos);
            inputPos.z = 0;
            swipeCards.inputMove(inputPos);
        }
    }

    private void TouchRelease(Vector3 inputPos)
    {
        if (isInTouch)
        {
            inputPos = Camera.main.ScreenToWorldPoint(inputPos);
            swipeCards.inputEnd(inputPos);
            isInTouch = false;
        }
    }

}
