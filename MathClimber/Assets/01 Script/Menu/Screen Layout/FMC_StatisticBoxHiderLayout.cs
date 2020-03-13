using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FMC_StatisticBoxHiderLayout : MonoBehaviour
{

    public GameObject background;
    public Text buttonText;
    public SpriteRenderer faceSpriteRenderer;
    public SpriteRenderer backgroundSpriteRenderer;

    private float cameraHeight;
    private float cameraWidth;
    private Vector2 cameraPosition;

    private void Awake()
    {
        setLayout();
    }

    private void OnEnable ()
    {
        if (!FMC_GameDataController.instance.subscriptionIsActive())
            gameObject.SetActive(true);
        else
            gameObject.SetActive(false);
    }

    public void setLayout ()
    {
        cameraHeight = Camera.main.orthographicSize * 2.0f;
        cameraWidth = cameraHeight * Camera.main.aspect;
        cameraPosition = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y);

        if (background)
            background.transform.localScale = new Vector3(cameraWidth, cameraWidth, 1.0f);


        if (faceSpriteRenderer)
            faceSpriteRenderer.size = new Vector2(cameraWidth * 0.9f, faceSpriteRenderer.size.y);

        if (backgroundSpriteRenderer)
            backgroundSpriteRenderer.size = new Vector2(cameraWidth * 0.9f, faceSpriteRenderer.size.y);
    }

}
