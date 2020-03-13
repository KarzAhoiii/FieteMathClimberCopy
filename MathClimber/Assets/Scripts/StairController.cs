using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairController : MonoBehaviour {
	[HideInInspector]
	public GameObject root; //The parent object containing all stairs
	public GameObject stairObj; //the template object that makes one step
	private List<GameObject> stairs; //the list of all stair objects
	[HideInInspector]
	public Vector3 stairOrigin; //the base of the stairs
	Vector3 step; //An offset from the center of one step to the center of another.
	private int centralStepOffset = 7; // how many steps away from the first one is a step where character stands

//	float _slideTime = 0.266f;

//	public AnimationCurve moveAnimation;
	DropManager dropMgr;
	Vector3 bounds; //dimentions of one stair object


	//float timer;
	//bool isMoving;
	//System.Action callback;
	//int direction;

	// Use this for initialization
	void Awake(){
		bounds = stairObj.GetComponent<Renderer> ().bounds.size;
		step = new Vector3 (0, bounds.y, bounds.z);
		stairOrigin = -(step * centralStepOffset);
	}

	void Start () {
		dropMgr = FindObjectOfType<DropManager> ();


		root = new GameObject ("StairRoot");
	

		stairs = new List<GameObject> ();
		int totalSteps = centralStepOffset * 2;
		for (int i = 0; i < totalSteps; i++) {
			AddStair (i);
//			if (dropMgr.enabled) {
//				if (i > centralStepOffset-1 && i < totalSteps-1) {
//					//dropMgr.SetStair (stairs [i].transform);
//					dropMgr.SpawnRandom (stairs [i].transform.position + Vector3.up * bounds.y / 2f, i - (centralStepOffset-1));
//				}
//			}
		}
		for (int i = 1; i < centralStepOffset; i++) {
			dropMgr.SpawnRandom (i);
		}
		//Spawn Lock
		if (FMC_GameDataController.instance != null && !FMC_GameDataController.instance.subscriptionIsActive()){
			dropMgr.SpawnForced (PickableItem.Type.LOCK, 6, 6);
		}

		//Setcoinorigin ();
		dropMgr.Reattach (root);
	}

	void Update(){
//		if (isMoving) {
//			timer -= Time.deltaTime;
//			float t = 1 - (timer / _slideTime);
//			root.transform.position = Vector3.Lerp (Vector3.zero, -step*direction, moveAnimation.Evaluate(t));
//			if (timer < 0) {
//				SnapBack ();
//
//			}
//		}
	}
	public void UpdateStairs (int dir, float t) {
    
		root.transform.position = Vector3.Lerp (Vector3.zero, -step*dir, t);
	}

	public void AddStair(int id){
		Vector3 pos = stairOrigin + step * id;
		GameObject go = Instantiate(stairObj, pos, Quaternion.identity);
		go.transform.parent = root.transform;
		if (id != 0){
			stairs.Add (go);
//			if (dropMgr != null && stairs.Count > 4) {
//				//dropMgr.SetStair (go.transform);
//			}
			//go.name = "Stair " + id;
		}
		else {
			stairs.Insert (0, go);
			//go.name = "Stair " + 0;
		}
		go.name = "new Stair " + id;
	}




	public void SnapBack(){


		dropMgr.Detach();
		root.transform.position = Vector3.zero;
		dropMgr.Reattach (root);
	}
		



	/// <summary>
	/// Gets the step position.
	/// </summary>
	/// <param name="id">Step number relative to player position. Can be negative.</param>
	public Vector3 GetStepPosition(int id){
		int absId = centralStepOffset -1 + id;
		if (absId > 0 && absId < stairs.Count) {
			//Debug.Log ("Position  "+absId+" is "+stairs[absId]);
			if (stairs [absId] != null) {
				return stairs [absId].transform.position + Vector3.up * (bounds.y / 2);
			}
			else {
				Debug.LogError ("ERROR the stair #" + absId + " with id "+id+" Does not exist");
				if (stairs [absId - 1] != null) {

					return stairs [absId - 1].transform.position + Vector3.up * (bounds.y / 2);

				}
				else {
					Debug.LogError ("and its neighbour "+ stairs [absId - 1] + " "+(absId - 1) + " is missing too");
					return Vector3.zero;
				}
					

			}
		}
		else {
			Debug.LogWarning ("Can't get step position. ID >"+centralStepOffset +"|| ID < -"+centralStepOffset);
			return Vector3.one * 100;
		}
	}

	public Vector3 first{
		get{ return stairs [0].transform.position;}
	}

//	public Vector3 stepBounds{
//		get{ return bounds;}
//	}s

	public Vector3 stepSize{
		get{ return step;}
	}
}
