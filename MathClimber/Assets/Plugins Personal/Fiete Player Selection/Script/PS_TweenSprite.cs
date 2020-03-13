using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PS_TweenSprite : MonoBehaviour 
{

	public SpriteRenderer outlineSpriteRenderer;
	public SpriteRenderer innerPartSpriteRenderer;
	public SpriteRenderer checkSpriteRenderer;

	private bool selected = false;
	private Color startColorOutline;
	private Color startColorInner;

	void Awake()
	{
		//outlineSpriteRenderer.material = new Material(Shader.Find("Sprites/Default"));
		//innerPartSpriteRenderer.material = new Material(Shader.Find("Sprites/Default"));

		startColorOutline = outlineSpriteRenderer.color;
		startColorInner = innerPartSpriteRenderer.color;
	}

	public void changeSprite()
	{
		//LeanTween.cancel (gameObject);

		if (selected)
		{
			LeanTween.value (gameObject, tweenValue, outlineSpriteRenderer.color.a, 0.0f, 0.1f);
			LeanTween.value (gameObject, tweenValue, innerPartSpriteRenderer.color.a, 0.0f, 0.1f);
			LeanTween.value (gameObject, tweenCheck, checkSpriteRenderer.color.a, 0.0f, 0.1f);
			selected = false;
			//return;
		}
		else
		{
			LeanTween.value (gameObject, tweenValue, outlineSpriteRenderer.color.a, 1.0f, 0.1f);
			LeanTween.value (gameObject, tweenValue, innerPartSpriteRenderer.color.a, 1.0f, 0.1f);
			LeanTween.value (gameObject, tweenCheck, checkSpriteRenderer.color.a, 1.0f, 0.1f);
			selected = true;
		}
	}

	private void tweenValue (float value)
	{
		outlineSpriteRenderer.color = new Color (startColorOutline.r, startColorOutline.g, startColorOutline.b, value);
		innerPartSpriteRenderer.color = new Color (startColorInner.r, startColorInner.g, startColorInner.b, value);
	}

	private void tweenCheck (float value)
	{
		checkSpriteRenderer.color = new Color (checkSpriteRenderer.color.r, checkSpriteRenderer.color.g, checkSpriteRenderer.color.b, value);
	}
}
