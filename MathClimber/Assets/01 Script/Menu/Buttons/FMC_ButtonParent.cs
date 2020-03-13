using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMC_ButtonParent : MonoBehaviour
{

    public bool isAvailableForFree;

    protected SpriteRenderer iapOverlay;

    protected void setIapOverlay (SpriteRenderer biggestSpriteRenderer, float yPosition, Transform parent)
    {
        /* ABO
        if (iapOverlay)
        {
            //iapOverlay.transform.position = biggestSpriteRenderer.transform.position;
            //iapOverlay.size = biggestSpriteRenderer.size;
            iapOverlay.gameObject.SetActive(true);
        }
        else
        {
            GameObject go = new GameObject();
            go.name = "iap Overlay";
            go.transform.parent = parent;
            iapOverlay = go.AddComponent<SpriteRenderer>();
            iapOverlay.sprite = FMC_GameDataController.instance.iapOverlaySprite;
            iapOverlay.sortingLayerID = biggestSpriteRenderer.sortingLayerID;
            iapOverlay.sortingOrder = biggestSpriteRenderer.sortingOrder + 5;
            //iapOverlay.size = new Vector2(biggestSpriteRenderer.bounds.extents.x * 2, biggestSpriteRenderer.bounds.extents.y * 2);
            iapOverlay.size = biggestSpriteRenderer.size;
            iapOverlay.drawMode = SpriteDrawMode.Sliced;
            iapOverlay.color = new Color32(249, 227, 80, 200);
            iapOverlay.gameObject.SetActive(true);
            go.transform.localScale = new Vector3(1, 1.25f, 1);
            go.transform.localPosition = new Vector3(0, yPosition, 0);
        }
        */
    }

    protected void setIapOverlayForPracticeButton (Transform playButton)
    {
        /*
        if (iapOverlay)
        {
            //iapOverlay.transform.position = biggestSpriteRenderer.transform.position;
            //iapOverlay.size = biggestSpriteRenderer.size;
            iapOverlay.gameObject.SetActive(true);
        }
        else
        {
            GameObject go = new GameObject();
            go.name = "iap Overlay";
            go.transform.parent = transform;
            iapOverlay = go.AddComponent<SpriteRenderer>();
            iapOverlay.sprite = FMC_GameDataController.instance.iapOverlaySprite;
            iapOverlay.sortingLayerName = "SelectionScreen";
            iapOverlay.sortingOrder = 7;
            //iapOverlay.size = new Vector2(biggestSpriteRenderer.bounds.extents.x * 2, biggestSpriteRenderer.bounds.extents.y * 2);
            iapOverlay.size = new Vector2(1.0f, 0.8f);
            iapOverlay.drawMode = SpriteDrawMode.Sliced;
            iapOverlay.color = new Color(1, 1, 1, 0.8f);
            iapOverlay.gameObject.SetActive(true);
            go.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            go.transform.position = playButton.transform.position;
        }
        */
    }

    public void setIapOverlaySize (Vector2 size)
    {
        if (iapOverlay)
            iapOverlay.size = size;
    }

    protected void setIapOverlayPosition (Vector3 position)
    {
        iapOverlay.transform.position = position;
    }

    protected void deactivateIapOverlay ()
    {
        if (iapOverlay)
            iapOverlay.gameObject.SetActive(false);
    }
}
