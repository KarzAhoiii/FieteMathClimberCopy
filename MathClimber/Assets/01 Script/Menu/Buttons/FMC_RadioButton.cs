using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class FMC_RadioButton : FMC_ButtonParent, IPointerDownHandler
{
    public enum edgeTypes { left, right, center };

    public FMC_Settings_Input.allInformation information;
    public edgeTypes edgeType;

    public SpriteRenderer faceSpriteRenderer;
    public SpriteRenderer backgroundSpriteRenderer;
    public SpriteRenderer edgeSpriteRenderer;
    public SpriteRenderer overlaySpriteRenderer;
    public BoxCollider2D boxCollider;
    public string text;
    public TextMesh textMesh;
    public PS_InputField.ageTypes ageType;

    public bool pressed { get; private set; }

    private bool isLocked;
    private float height = 0.2f;
    private float transitionTime = 0.0f;
    private Vector3 checkedPosition;
    private Vector3 uncheckedPosition;
    private Color checkedColor;
    private Color uncheckedColor;
    private Color disabledColor = new Color(0.8f, 0.8f, 0.8f);
    private FMC_RadioButtonController controller;

    public void initialise (FMC_RadioButtonController _controller)
    {
        controller = _controller;
        height = controller.height;
        transitionTime = controller.transitionTime;
        checkedPosition = transform.position;
        uncheckedPosition = new Vector3(transform.position.x, transform.position.y + height, transform.position.z);
        faceSpriteRenderer.transform.position = uncheckedPosition;
        checkedColor = controller.checkedColor;
        uncheckedColor = controller.uncheckedColor;
        LeanTween.color(faceSpriteRenderer.gameObject, uncheckedColor, 0.0f);
        checkIfEnabled();
    }

    public void OnEnable()
    {
        checkIfEnabled();
    }

    private void checkIfEnabled()
    {
        if (FMC_GameDataController.instance && !FMC_GameDataController.instance.subscriptionIsActive() && !isAvailableForFree)
        {
            setIapOverlay(faceSpriteRenderer, 0.07f, transform);
            isLocked = true;
        }
        else
        {
            deactivateIapOverlay();
            isLocked = false;
        }
    }

    public void createButton()
    {

        GameObject go;

        if (!faceSpriteRenderer)
        {
            go = new GameObject();
            go.name = "Face";
            go.transform.parent = transform;
            go.transform.localPosition = new Vector3(0, 0, 0);
            go.transform.localScale = new Vector3(1, 1, 1);
            faceSpriteRenderer = go.AddComponent<SpriteRenderer>();
            faceSpriteRenderer.drawMode = SpriteDrawMode.Sliced;
            faceSpriteRenderer.sprite = Resources.Load<Sprite>("Texture/Rect01");  
        }

        if (!boxCollider)
        {
            boxCollider = gameObject.AddComponent<BoxCollider2D>();
            boxCollider.isTrigger = true;
            boxCollider.size = faceSpriteRenderer.size;
        }

        if (!backgroundSpriteRenderer)
        {
            go = new GameObject();
            go.name = "Background";
            go.transform.parent = transform;
            go.transform.localPosition = new Vector3(0, 0, 0);
            go.transform.localScale = new Vector3(1, 1, 1);
            backgroundSpriteRenderer = go.AddComponent<SpriteRenderer>();
            backgroundSpriteRenderer.drawMode = SpriteDrawMode.Sliced;
            backgroundSpriteRenderer.sprite = Resources.Load<Sprite>("Texture/Rect01");
            backgroundSpriteRenderer.color = Color.black;
            backgroundSpriteRenderer.sortingOrder = faceSpriteRenderer.sortingOrder - 1;
        }

        if (!edgeSpriteRenderer)
        {
            go = new GameObject();
            go.name = "Edge";
            go.transform.parent = faceSpriteRenderer.transform;
            go.transform.localPosition = new Vector3(0, 0, 0);
            go.transform.localScale = new Vector3(1, 1, 1);
            edgeSpriteRenderer = go.AddComponent<SpriteRenderer>();
            edgeSpriteRenderer.drawMode = SpriteDrawMode.Sliced;
            edgeSpriteRenderer.sprite = Resources.Load<Sprite>("Texture/Edge01");
            edgeSpriteRenderer.color = Color.black;
            edgeSpriteRenderer.sortingOrder = faceSpriteRenderer.sortingOrder + 1;
            edgeSpriteRenderer.size = faceSpriteRenderer.size;
        }

        if (!textMesh)
        {
            go = new GameObject();
            go.name = "Text";
            go.transform.parent = faceSpriteRenderer.transform;
            go.transform.localPosition = new Vector3(0, 0, -0.05f);
            go.transform.localScale = new Vector3(1, 1, 1);
            textMesh = go.AddComponent<TextMesh>();
            textMesh.text = text;
            textMesh.anchor = TextAnchor.MiddleCenter;
            textMesh.characterSize = 0.05f;
            textMesh.fontSize = 80;
            textMesh.color = Color.black;
        }
    }

    public void destroyButton()
    {
        foreach (Transform t in transform)
            DestroyImmediate(t.gameObject);
    }

	public void OnPointerDown(PointerEventData evd)
	{
        if (!pressed && !isLocked)
        {
            checkButton(true);
        }
	}

    public void setText (string t)
    {
        text = t;
        textMesh.text = t;
    }

    public void checkButton (bool makeSound)
    {
        pressed = true;
        animateToCheckPosition(makeSound);

        if (controller)
            controller.radioButtonPressed(this);
    }

    private void animateToCheckPosition (bool makeSound)
    {
        if (FMC_GameDataController.instance && makeSound)
            LeanAudio.play(FMC_GameDataController.instance.buttonClickSound, FMC_GameDataController.instance.buttonClickVolume);

        LeanTween.cancel(faceSpriteRenderer.gameObject);
        LeanTween.moveY(faceSpriteRenderer.gameObject, checkedPosition.y + (Mathf.Abs((uncheckedPosition.y - checkedPosition.y) * 0.2f)), transitionTime);
        LeanTween.color(faceSpriteRenderer.gameObject, checkedColor, transitionTime);
    }

    public void uncheckButton (bool makeSound) {
        pressed = false;
        animateToUncheckedPosition(makeSound); 
    }
    
    public void disableButton() {
        pressed = false;
        LeanTween.cancel(faceSpriteRenderer.gameObject);
        LeanTween.moveY(faceSpriteRenderer.gameObject, uncheckedPosition.y, 0);
        LeanTween.color(faceSpriteRenderer.gameObject, uncheckedColor, 0);
    }

    private void animateToUncheckedPosition (bool makeSound) {
        if (FMC_GameDataController.instance && makeSound)
            LeanAudio.play(FMC_GameDataController.instance.buttonClickSound, FMC_GameDataController.instance.buttonClickVolume);

        LeanTween.cancel(faceSpriteRenderer.gameObject);
        LeanTween.moveY(faceSpriteRenderer.gameObject, uncheckedPosition.y, transitionTime);
        LeanTween.color(faceSpriteRenderer.gameObject, uncheckedColor, transitionTime);
    }	
}
