using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR 
using UnityEditor;
#endif

public class SC_SwipeCardController : MonoBehaviour
{

    public List<SC_SwipeCard> allSwipeCards;
    public SC_SwipeCard startCard;
    public SC_SwipeCard endCard;

    public Transform swipeCardsParent;
    public Transform whiteIndicator;
    public List<GameObject> allIndicatorCircles;
    public GameObject backButton;

    public UnityEvent onReachedLastSwipeCard;

    private float cameraHeight;
    private float cameraWidth;
    private Vector2 cameraPosition;

    private float lastTouchPosition;
    private float minLocalX;
    private float maxLocalX;
    private float dampingDistance = 1.75f;
    private float delayedPosition;
    private float circleDistance = 0.3f;
    private SC_SwipeCard currentSwipeCard;
    private List<GameObject> indicatorCircles;

    private void Awake ()
    {
        setLayout();
    }

    public void setLayout ()
    {
        cameraHeight = Camera.main.orthographicSize * 2.0f;
        cameraWidth = cameraHeight * Camera.main.aspect;
        cameraPosition = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y);

        for (int i = 0; i < allSwipeCards.Count; i++)
        {
            allSwipeCards[i].background.size = new Vector2(cameraWidth, cameraHeight);
            allSwipeCards[i].background.color = allSwipeCards[i].backgroundColor;
            allSwipeCards[i].transform.localPosition = new Vector3(i * cameraWidth, 0, 0);
            allSwipeCards[i].index = i;
        }

        float extent = ((allIndicatorCircles.Count - 1) * circleDistance) / 2;
        for (int i = 0; i < allIndicatorCircles.Count; i++)
            allIndicatorCircles[i].transform.localPosition = new Vector3((i * circleDistance) - extent, allIndicatorCircles[i].transform.localPosition.y, allIndicatorCircles[i].transform.localPosition.z);

        setToSwipeCard(startCard);

        minLocalX = -endCard.transform.localPosition.x;
        maxLocalX = -startCard.transform.localPosition.x;

        if (backButton)
            backButton.transform.position = new Vector3((cameraPosition.x - (cameraWidth * 0.5f)) + (backButton.transform.localScale.x * 1.1f), (cameraPosition.y + (cameraHeight * 0.5f)) - (backButton.transform.localScale.y * 1.1f), 0.0f);
    }

    public void inputStart (Vector3 position)
    {
        lastTouchPosition = position.x;
        delayedPosition = lastTouchPosition;
    }

    public void inputMove (Vector3 position)
    {
        float moveDistance = lastTouchPosition - position.x;

        if (swipeCardsParent.localPosition.x > maxLocalX)
        {
            float offset = 1.0f - ((Mathf.Clamp(Mathf.Abs(maxLocalX - swipeCardsParent.position.x), 0.0f, dampingDistance)) / dampingDistance);
            swipeCardsParent.position = new Vector3(swipeCardsParent.position.x - (moveDistance * offset), swipeCardsParent.position.y, swipeCardsParent.position.z);
        }
        else if (swipeCardsParent.localPosition.x < minLocalX)
        {
            float offset = 1.0f - ((Mathf.Clamp(Mathf.Abs(minLocalX - swipeCardsParent.position.x), 0.0f, dampingDistance)) / dampingDistance);
            swipeCardsParent.position = new Vector3(swipeCardsParent.position.x - (moveDistance * offset), swipeCardsParent.position.y, swipeCardsParent.position.z);
        }
        else
            swipeCardsParent.position = new Vector3(swipeCardsParent.position.x - moveDistance, 0.0f, 0.0f);

        lastTouchPosition = position.x;
        StartCoroutine(setValueDelayed(lastTouchPosition));
    }

    public void inputEnd (Vector3 position)
    {
        lastTouchPosition = position.x;

        if (!checkForSwipe())
            tweenToNearest();

    }

    private bool checkForSwipe ()
    {
        if (Mathf.Abs(lastTouchPosition - delayedPosition) > 0.15f)
        {
            if (lastTouchPosition - delayedPosition < 0 && currentSwipeCard.index + 1 <= endCard.index)
            {
                tweenToSwipeCard(allSwipeCards[currentSwipeCard.index + 1], 0.35f);
                return true;
            }
            else if ((lastTouchPosition - delayedPosition > 0 && currentSwipeCard.index - 1 >= startCard.index))
            {
                tweenToSwipeCard(allSwipeCards[currentSwipeCard.index - 1], 0.35f);
                return true;
            }
        }
        return false;
    }

    private void tweenToNearest ()
    {
        int index = 0;
        float nearestPosition = Mathf.Abs(transform.InverseTransformPoint(allSwipeCards[0].transform.position).x); // Lokale X Position relativ zum Parent Parent

        for (int i = 1; i < allSwipeCards.Count; i++)
        {
            if (Mathf.Abs(transform.InverseTransformPoint(allSwipeCards[i].transform.position).x) < nearestPosition)
            {
                index = i;
                nearestPosition = Mathf.Abs(transform.InverseTransformPoint(allSwipeCards[i].transform.position).x);
            }
        }

        tweenToSwipeCard(allSwipeCards[index], 0.2f);

    }

    private void tweenToSwipeCard (SC_SwipeCard swipeCard, float time)
    {
        LeanTween.moveX(swipeCardsParent.gameObject, swipeCardsParent.position.x - transform.InverseTransformPoint(swipeCard.transform.position).x, time).setEase(LeanTweenType.easeOutCubic).setOnComplete(onTweenEnd);
        currentSwipeCard = swipeCard;
    }

    private void setToSwipeCard (SC_SwipeCard swipeCard)
    {
        swipeCardsParent.position = new Vector3(swipeCardsParent.position.x - transform.InverseTransformPoint(swipeCard.transform.position).x, swipeCardsParent.position.y, swipeCardsParent.position.z);
        currentSwipeCard = swipeCard;

        if (currentSwipeCard.associatedIndicatorCircle)
            whiteIndicator.position = currentSwipeCard.associatedIndicatorCircle.position;
    }

    private void onTweenEnd ()
    {
        if (currentSwipeCard.associatedIndicatorCircle)
            whiteIndicator.position = currentSwipeCard.associatedIndicatorCircle.position;
        else
            Debug.LogWarning("Indicator Circle not correctly set up.");

        checkForEndCard();
    }

    private void activateFirstSwipeCard()
    {
        allSwipeCards[0].gameObject.SetActive(true);
    }

    private void checkForEndCard()
    {
        if (currentSwipeCard == endCard)
        {
            onReachedLastSwipeCard.Invoke();
        }
    }

    private IEnumerator setValueDelayed(float delayedValue)
    {
        yield return new WaitForSeconds(0.1f);
        delayedPosition = delayedValue;
    }

    public void enableSwipeCards ()
    {
        setToSwipeCard(startCard);
        gameObject.SetActive(true);
    }

    public void tweenInSwipeCards ()
    {
        allSwipeCards[0].gameObject.SetActive(false);
        setToSwipeCard(allSwipeCards[0]);
        tweenToSwipeCard(allSwipeCards[1], 0.3f);
        gameObject.SetActive(true);
        LeanTween.delayedCall(0.3f, activateFirstSwipeCard);
    }

    public void disableSwipeCards ()
    {
        gameObject.SetActive(false);
    }
}

#if UNITY_EDITOR 
[CustomEditor(typeof(SC_SwipeCardController))]
public class SC_SwipeCardControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SC_SwipeCardController myScript = (SC_SwipeCardController)target;
        if (GUILayout.Button("Set layout"))
        {
            myScript.setLayout();
        }
    }
}
#endif