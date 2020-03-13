using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPController : MonoBehaviour {

	public int[] thresholds;
	int experience;
	int level;

	GameController main;
	//TaskUI ui;
	DropManager drop;
	public XPPanel panel;

	bool spawnLvl;
	void Awake(){
		Load ();
	}
	// Use this for initialization
	void Start () {
		main = FindObjectOfType<GameController> ();
		drop = FindObjectOfType<DropManager> ();

		Load ();
		if (drop != null && !ClimberStateManager.inTraining){
			int expdiff = curThreshold - experience;
			if (expdiff <= 6){
				spawnLvl = true;
			}
		}

		Refresh();

	}
	void Update(){
		if (spawnLvl) {
			spawnLvl = false;
			int expdiff = curThreshold - experience;
			Vector3 pos = drop.GetItem (expdiff).obj.transform.position;
			drop.Spawn (PickableItem.Type.LVL, pos, expdiff, true);
		}
	}

	public void Load(){
		if (ClimberStateManager.inCampaign){
			experience = Persistence.campaignXp;
		}
		else {
			experience = Persistence.freestyleXp;
		}

		level = GetLevelFromXP(experience);

	}

	public void Refresh (){
		panel.Refresh ();
	}

	public void AddXp(){
		AddXp (1);
	}

	public void SubtractXp(){
		if (GetExperienceToLevel () < curThreshold) {
			AddXp (-1);
		}
		else {
			Debug.LogError ("Can't subtract XP lower than 0");
		}
			
	}

	public void AddXp (int i) {
		experience += i;
		Refresh ();
		if (ClimberStateManager.inCampaign) {
			if (experience == curThreshold - 6){
				drop.SpawnForced (PickableItem.Type.LVL, 7, 6);
			}

			Persistence.campaignXp = experience;
		}
		else if (ClimberStateManager.inTestMode) { // in test mode

			if (experience == curThreshold - 6){
				drop.SpawnForced (PickableItem.Type.LVL, 7, 6);
			}
			Persistence.freestyleXp = experience;
		}
		else { // non campaign live
			if (experience == curThreshold){
				LvlUp ();
			}	
			Persistence.freestyleXp = experience;
		}

	}

	public void LvlUp(){
		level++;
		//main.ResetCam ();
		Refresh();
	}

	public int GetLevel(){
		return level;
	}

	public int GetExperience(){
		return experience;
	}

		

	public int GetExperienceToLevel(){

		return GetThreshold(level) - experience;
	}

	int GetLevelFromXP(int xp){
		if (xp < thresholds [0]) {
			return 0;
		}
		else {
			for (int i = 0; i < 10000; i++) {
				if (experience >= GetThreshold(i) && experience < GetThreshold(i+1)) {
					return i + 1;
				}
			}
			Debug.LogError ("Xp is out of range of levels");
			return -1;
		}
	}


	public int curThreshold{
		get{ 

			return GetThreshold(level);
		}
	}

	int GetThreshold(int lv){
		int maxLv = thresholds.Length-1; 
		if (lv <= maxLv) {
			return thresholds[lv];
		}
		else {
			return thresholds [maxLv] + (lv - maxLv) * 5000;
		}
	}

	public float levelProgress{
		get{ 
			if (level > 0) {
				int prevThreshold = GetThreshold (level - 1);
				return ((float)experience - prevThreshold) /  (curThreshold - prevThreshold);
			}
			else {
				return (float)experience / curThreshold;
			}
		}
	}
	public void InstaLevel(){
		AddXp(GetExperienceToLevel()-2);
		if (drop != null){
			int expdiff = curThreshold - experience;
			if (expdiff <= 6){
				spawnLvl = true;
			}
		}
	}
}
