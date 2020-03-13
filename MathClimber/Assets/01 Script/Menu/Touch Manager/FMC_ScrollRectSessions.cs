using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FMC_ScrollRectSessions : MonoBehaviour
{

    [Range (0.7f, 0.95f)] public float scrollDrag; // .87f
    [Range (0.2f, 0.6f)] public float scrollDragOutside; // .3f
    public FMC_ShowPastLayout showPastLayout;

    private bool scroll = false;
    private bool isIphoneX = false;
    private float scrollDistance = 0.0f;
    private float maxPosition;
    private float minPosition;
    private float dampingDistance = 1.75f;
    private GameObject backGroundToUse;
    private List<Vector3> lastTouchPositions = new List<Vector3>();

    private void Awake()
    {
        if ((float)Screen.width / (float)Screen.height < 0.5f)
            isIphoneX = true;

        maxPosition = isIphoneX ? Camera.main.orthographicSize - 1.33f : Camera.main.orthographicSize - 1;
        minPosition = -(Camera.main.orthographicSize) + 0.5f;


    }

    public void inputStart(Vector3 position)
    {
        LeanTween.cancel(gameObject);
        lastTouchPositions.Clear();
        scroll = false;
        lastTouchPositions.Add(position);
    }

    public void inputMove(Vector3 position)
    {
        LeanTween.cancel(gameObject);
        float moveDistance = lastTouchPositions[0].y - position.y;
        float topPos = showPastLayout.getTopPosition();
        float bottomPos = showPastLayout.getBottomPosition();


        if (topPos < maxPosition)
        {
            float offset = 1.0f - ((Mathf.Clamp(Mathf.Abs(maxPosition - topPos), 0.0f, dampingDistance)) / dampingDistance);
            transform.position = new Vector3(0.0f, transform.position.y - (moveDistance * offset), 0.0f);
        }
        else if (bottomPos > minPosition)
        {
            float offset = 1.0f - ((Mathf.Clamp(Mathf.Abs(bottomPos - minPosition), 0.0f, dampingDistance)) / dampingDistance);
            transform.position = new Vector3(0.0f, transform.position.y - (moveDistance * offset), 0.0f);
        }
        else
            transform.position = new Vector3(0.0f, transform.position.y - moveDistance, 0.0f);

        lastTouchPositions.Insert(0, position);
        if (lastTouchPositions.Count > 3)
            lastTouchPositions.RemoveAt(3);
    }

    public void inputEnd(Vector3 position)
    {
        if (!moveBack())
        {
            //float moveDistance = 0;
            //for (int i = 0; i < lastTouchPositions.Count - 1; i++)
            //    moveDistance -= lastTouchPositions[i].y - lastTouchPositions[i + 1].y;
            //moveDistance /= lastTouchPositions.Count - 1;

            float moveDistance = 0;
            if (lastTouchPositions.Count > 1)
                moveDistance = lastTouchPositions[1].y - lastTouchPositions[0].y;

            if (Mathf.Abs(moveDistance) < 0.002 && lastTouchPositions.Count > 2)
                moveDistance = lastTouchPositions[2].y - lastTouchPositions[1].y;

            if (Mathf.Abs(moveDistance) > 0.05f)
            {
                scrollDistance = moveDistance;
                scroll = true;
            }
        }
    }

    private void scrollBox ()
    {
        if (!(showPastLayout.getTopPosition() < maxPosition) && !(showPastLayout.getBottomPosition() > minPosition))
        {
            transform.position = new Vector3(0.0f, transform.position.y - scrollDistance, 0.0f);
            scrollDistance *= scrollDrag;
        }
        else
        {
            transform.position = new Vector3(0.0f, transform.position.y - scrollDistance, 0.0f);
            scrollDistance *= scrollDragOutside;
        }

        if (Mathf.Abs(scrollDistance) < 0.01f)
        {
            scroll = false;
            moveBack();
        }
    }

    private bool moveBack ()
    {
        float topPos = showPastLayout.getTopPosition();
        float bottomPos = showPastLayout.getBottomPosition();

        if (showPastLayout.getCompleteSize() < Camera.main.orthographicSize * 2)
        {
            if (!isIphoneX)
                LeanTween.moveLocalY(gameObject, 0, 0.4f).setEase(LeanTweenType.easeOutCubic);
            else
                LeanTween.moveLocalY(gameObject, -0.33f, 0.4f).setEase(LeanTweenType.easeOutCubic);
            return true;
        }

        if (topPos < maxPosition)
        {
            LeanTween.moveLocalY(gameObject, transform.localPosition.y + Mathf.Abs(maxPosition - showPastLayout.getTopPosition()), 0.4f).setEase(LeanTweenType.easeOutCubic);
            return true;
        }
        else if (bottomPos > minPosition)
        {
            LeanTween.moveY(gameObject, transform.localPosition.y - Mathf.Abs(showPastLayout.getBottomPosition() - minPosition), 0.4f).setEase(LeanTweenType.easeOutCubic);
            return true;
        }

        return false;
    }

    private void Update ()
    {
        if (scroll)
            scrollBox();
        
    }

}
