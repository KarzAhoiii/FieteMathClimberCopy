using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FMC_ScrollRect : MonoBehaviour
{

    public GameObject background;
    [Range (0.7f, 0.95f)] public float scrollDrag; // .87f
    [Range (0.2f, 0.6f)] public float scrollDragOutside; // .3f

    private bool scroll = false;
    private float scrollDistance = 0.0f;
    private float topPosition;
    private float bottomPosition;
    private float dampingDistance = 1.75f;
    private GameObject backGroundToUse;
    private List<Vector3> lastTouchPositions = new List<Vector3>();

    private void Awake()
    {
        BoxCollider2D boxCollider = background.AddComponent<BoxCollider2D>();
        //boxCollider.size = new Vector2(background.transform.localScale.x, background.transform.localScale.y * 1.0f);
        //boxCollider.offset = new Vector2(0, -(background.transform.localScale.y * 0.5f));
        //boxCollider.isTrigger = true;

        topPosition = transform.position.y;
        bottomPosition = -Camera.main.orthographicSize;
        setCurrentBackgroundToUse();
    }

    private void OnEnable ()
    {
        setCurrentBackgroundToUse();
    }

    private void setCurrentBackgroundToUse()
    {
        backGroundToUse = background;

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

        if (transform.position.y < topPosition)
        {
            float offset = 1.0f - ((Mathf.Clamp(Mathf.Abs(topPosition - transform.position.y), 0.0f, dampingDistance)) / dampingDistance);
            transform.position = new Vector3(0.0f, transform.position.y - (moveDistance * offset), 0.0f);
        }
        else if (transform.position.y - backGroundToUse.transform.localScale.y > bottomPosition)
        {
            float offset = 1.0f - ((Mathf.Clamp(Mathf.Abs(transform.position.y - backGroundToUse.transform.localScale.y - bottomPosition), 0.0f, dampingDistance)) / dampingDistance);
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
        if (!(transform.position.y < topPosition) && !(transform.position.y - backGroundToUse.transform.localScale.y > bottomPosition))
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
        if (transform.position.y < topPosition)
        {
            LeanTween.moveY(gameObject, topPosition, 0.4f).setEase(LeanTweenType.easeOutCubic);
            return true;
        }
        else if (transform.position.y - backGroundToUse.transform.localScale.y > bottomPosition)
        {
            LeanTween.moveY(gameObject, bottomPosition + backGroundToUse.transform.localScale.y, 0.4f).setEase(LeanTweenType.easeOutCubic);
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
