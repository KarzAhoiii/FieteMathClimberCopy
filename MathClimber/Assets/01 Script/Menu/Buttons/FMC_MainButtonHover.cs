using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMC_MainButtonHover : MonoBehaviour
{

    public GameObject mainButtonShadow;
    public AnimationCurve hoverCurve;

    private Vector3 startPosition;

    private void Awake ()
    {
        startPosition = gameObject.transform.position;
    }


    private void OnEnable()
    {
        startHovering();
    }

    private void OnDisable()
    {
        LeanTween.cancel(gameObject);
        LeanTween.cancel(mainButtonShadow);
    }

    private void startHovering ()
    {
        //transform.position = startPosition;
		//startPosition = gameObject.transform.position;
        LeanTween.alpha(mainButtonShadow, 1.0f, 0.0f);
        LeanTween.moveY(gameObject, startPosition.y + 0.12f, 4.0f).setEase(hoverCurve).setOnComplete(startHovering);
        LeanTween.alpha(mainButtonShadow, 0.5f, 4.0f).setEase(hoverCurve);
    }
}
