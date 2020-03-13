using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PS_ScrollRect : MonoBehaviour
{

	[HideInInspector] public bool canMove = true;

	//private Vector3 lastTouchPos;
    private List<Vector3> lastTouchPositions = new List<Vector3>();

    private float maxPosY = 0;
	private float minPosY = 0;
	private float bottomRectPos;
	private float touchDistance;
	private float scale;
	private float dampingDistance;

	private BoxCollider2D boxCollider;
	private Transform firstButton;
	private Transform lastButton;

	public void init (PS_PlayerButtonLayout layout) // Width noch mitgeben 
	{
		if (layout.buttons.Count <= 0)
		{
			firstButton = transform;
			lastButton = transform;
			layout.addPlayerButton.enableButton ();
			return;
		}

		scale = Camera.main.orthographicSize / 5;
		dampingDistance = scale * 3.0f;

		float topButtonPos = layout.buttons[0].transform.localPosition.y + (layout.buttonHeightInUnits * 0.5f);
		float bottomButtonPos = layout.buttons[layout.buttons.Count-1].transform.localPosition.y - (layout.buttonHeightInUnits * 0.5f);
		if (bottomButtonPos > Camera.main.transform.position.y - Camera.main.orthographicSize + layout.rectBottomInUnits)
			bottomButtonPos = Camera.main.transform.position.y - Camera.main.orthographicSize + layout.rectBottomInUnits;
		float width = Camera.main.orthographicSize * 2.0f * Camera.main.aspect * layout.buttonWidthInPercent;

		boxCollider = GetComponent<BoxCollider2D> ();

		// Größe des Box Colliders initialisieren.
		boxCollider.size = new Vector2(width, topButtonPos - bottomButtonPos);
		boxCollider.offset = new Vector2 (0, Mathf.Lerp(topButtonPos, bottomButtonPos, 0.5f));

		// Extrema des Scrollrects initialisieren.
		maxPosY = (Camera.main.transform.position.y + Camera.main.orthographicSize) - layout.rectTopInUnits - (layout.buttonHeightInUnits * 0.5f);
		minPosY = (Camera.main.transform.position.y - Camera.main.orthographicSize) + layout.rectBottomInUnits + (layout.buttonHeightInUnits * 0.5f);
		firstButton = layout.buttons [0].transform;
		lastButton = layout.buttons[layout.buttons.Count-1].transform;

		if (lastButton.position.y > minPosY)
		{
			minPosY = lastButton.position.y;
		}

		canMove = true;
	}

	public void inputStart(Vector3 pos)
	{
		if (!canMove)
			return;

        lastTouchPositions.Clear();
        lastTouchPositions.Add(pos);
		LeanTween.cancel (gameObject);
	}

	public void inputMove(Vector3 pos)
	{
		if (!canMove)
			return;

		touchDistance = lastTouchPositions[0].y - pos.y;

        if (firstButton.position.y < maxPosY)
		{
			float offset = 1.0f - ((Mathf.Clamp(Mathf.Abs (maxPosY - firstButton.position.y), 0.0f, dampingDistance)) / dampingDistance);
			transform.position = new Vector3(0.0f, transform.position.y - (touchDistance * offset), 0.0f);
		}
		else if (lastButton.position.y > minPosY) // + wenn Buttons den Screen ausfüllen
		{
			float offset = 1.0f - ((Mathf.Clamp(Mathf.Abs (lastButton.position.y - minPosY), 0.0f, dampingDistance)) / dampingDistance);
			transform.position = new Vector3(0.0f, transform.position.y - (touchDistance * offset), 0.0f);
		}
		else
			transform.position = new Vector3(0.0f, transform.position.y - touchDistance, 0.0f);

        lastTouchPositions.Insert(0, pos);
        if (lastTouchPositions.Count > 3)
            lastTouchPositions.RemoveAt(3);
    }

	public void inputEnd(Vector3 pos)
	{
		if (!canMove)
			return;

		tweenBack ();

        float moveDistance = 0;
        for (int i = 0; i < lastTouchPositions.Count - 1; i++)
            moveDistance -= lastTouchPositions[i].y - lastTouchPositions[i + 1].y;
        moveDistance /= lastTouchPositions.Count - 1;

        // Weiterscrollen ohne Finger auf dem Bildschirm
        if (Mathf.Abs(moveDistance) > 0.01f && !(firstButton.position.y < maxPosY || lastButton.position.y > minPosY))
		{
			LeanTween.value(gameObject, scroll, touchDistance, 0.0f, 1f).setEase(LeanTweenType.easeOutCubic).setOnComplete(tweenBack);
		}
	}

	private void scroll (float distance)
	{
		transform.position = new Vector3(0.0f, transform.position.y - distance, 0.0f);

		if (firstButton.position.y < maxPosY || lastButton.position.y > minPosY)
		{
			LeanTween.cancel (gameObject);
			LeanTween.value(gameObject, scrollEnd, distance, 0.0f, 0.15f).setEase(LeanTweenType.easeOutCubic).setOnComplete(tweenBack);
		}
	}

	private void scrollEnd (float distance)
	{
		transform.position = new Vector3(0.0f, transform.position.y - distance, 0.0f);
	}

	private void tweenBack ()
	{
		if (firstButton.position.y < maxPosY)
		{
			LeanTween.moveY (gameObject, 0, 0.4f).setEase (LeanTweenType.easeOutCubic);
		}

		if (lastButton.position.y > minPosY)
		{
			LeanTween.moveY (gameObject, minPosY + (transform.position.y - lastButton.position.y), 0.4f).setEase (LeanTweenType.easeOutCubic);
		}
	}

}
