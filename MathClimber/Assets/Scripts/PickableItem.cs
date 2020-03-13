using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableItem : MonoBehaviour {
	public enum Type
	{
		COIN, BOMB, PINATA, ROCKET, BOX, CRYSTAL, TIMER, LVL, LOCK

	}

	public AudioClip sound;
	public GameObject pickEffect;
	public Type type;
	public int coinCount;
	//[HideInInspector]
	public int place;

	float period = 1;
	float amplitude = 1;
	// Use this for initialization
	void Start () {
		Transform child = transform.GetChild (0);
		Vector3 to = child.localPosition + child.up * amplitude;
		LeanTween.moveLocal (child.gameObject, to, period).setLoopPingPong().setRepeat (-1).setEase(LeanTweenType.easeInOutSine);
	}
	


	public void SetPlace(int plc){
		place = plc;

	}

	public bool isSpecial{
		get{ return type == Type.BOMB || type == Type.PINATA || type == Type.ROCKET || type == Type.CRYSTAL || type == Type.LVL; }
	}
}
