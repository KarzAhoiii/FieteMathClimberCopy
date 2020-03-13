using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PS_TouchManager : MonoBehaviour
{

    public PS_ScrollRect scrollRect;
    public GameObject colliderHolder;
    private bool touchBegan = false;
    private bool isInTouch = false;

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

        Ray ray = Camera.main.ScreenPointToRay(inputPos);
        RaycastHit2D[] hits = Physics2D.GetRayIntersectionAll(ray, Mathf.Infinity);

        for (int i = 0; i < hits.Length; i++)
        {
            Collider2D coll = hits[i].collider;
            if (coll.gameObject == colliderHolder)
            {
                isInTouch = true;
                inputPos = Camera.main.ScreenToWorldPoint(inputPos);
                inputPos.z = 0;
                scrollRect.inputStart(inputPos);
            }
        }
    }

    private void TouchMoved(Vector3 inputPos)
    {
        if (isInTouch)
        {
            inputPos = Camera.main.ScreenToWorldPoint(inputPos);
            inputPos.z = 0;
            scrollRect.inputMove(inputPos);
        }
    }

    private void TouchRelease(Vector3 inputPos)
    {
        if (isInTouch)
        {
            inputPos = Camera.main.ScreenToWorldPoint(inputPos);
            scrollRect.inputEnd(inputPos);
            isInTouch = false;
        }
    }

}