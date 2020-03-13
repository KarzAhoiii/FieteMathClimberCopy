using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropManager : MonoBehaviour {
	//public Vector2 bankPos;
	//public GameObject bankPlank;
//	public Canvas uiRoot;
	public GameObject[] drops;
	public float[] weights;
	//public Vector3 spawnPos;

	PickableItem.Type[] types;
	//How far are items spawned from the player
	int offset = 6;

	float[] chances;
	List <DropItem> spawned;
	//Transform parentStair;
	Camera stairCam;

	public BankController bank;
	BombController bomb;
	CharacterScript character;
	CharacterStorage charStore;
    
    public GameObject coinObject;


	public Sprite coin;
	public GameObject pickupEff;


	GameController main;
	StairController stairs;
	XPController exp;
	HelpController help;
	SmallHelpButton smHelp;
	//bool reward;
	// Use this for initialization
	void Awake () {
		spawned = new List<DropItem> ();
		InitRandom ();
	}

	void Start () {
    
		stairCam = GameObject.Find ("StairCamera").GetComponent<Camera> ();
		bomb = FindObjectOfType<BombController> ();
		character = FindObjectOfType<CharacterScript> ();
		charStore = FindObjectOfType<CharacterStorage> ();
		help = FindObjectOfType<HelpController>();
		smHelp = FindObjectOfType<SmallHelpButton> ();

		stairs = FindObjectOfType< StairController>();

		main = FindObjectOfType<GameController>();
		exp = FindObjectOfType<XPController> ();

		FMC_Statistics.newPowerUp += RewardPerformance;

		types = new PickableItem.Type[drops.Length];
		for (int i = 0; i < types.Length; i++) {
			types [i] = drops [i].GetComponent<PickableItem> ().type;
		}
	}

	void OnDestroy(){
		FMC_Statistics.newPowerUp -= RewardPerformance;
	}
    
    
    private Vector3 GetAnchor(Vector2 ndcSpace) {
    
        Vector3 worldPosition;
    
        Vector4 viewSpace = new Vector4(ndcSpace.x, ndcSpace.y, 1.0f, 1.0f);
    
        // Transform to projection coordinate.
        Vector4 projectionToWorld = (Camera.main.projectionMatrix.inverse * viewSpace);
    
        // Perspective divide.
        projectionToWorld /= projectionToWorld.w;
    
        // Z-component is backwards in Unity.
        projectionToWorld.z = -projectionToWorld.z;
    
        // Transform from camera space to world space.
        worldPosition = Camera.main.transform.position + Camera.main.transform.TransformVector(projectionToWorld);
    
        return worldPosition;
    }
    
	//Picks up a specific item
	public void Pickup(DropItem itm){
		Sprite spr = itm.obj.GetComponentInChildren<SpriteRenderer> ().sprite;
		Vector2 pos = stairCam.WorldToScreenPoint (itm.obj.transform.position);
		//TMPro.TextMeshPro[] texts = itm.obj.GetComponentsInChildren<TMPro.TextMeshPro> ();

		if (itm.item.pickEffect != null){
			GameObject eff = Instantiate (itm.item.pickEffect, itm.obj.transform.position + Vector3.up, itm.item.pickEffect.transform.rotation);
			eff.transform.parent = itm.obj.transform.parent;

		}

		//Debug.Log ("snd "+itm.item.sound.name);
		CharacterProfile curChar = charStore.GetCharacter (Persistence.currentChar);
		if (itm.item.isSpecial && curChar.specialVoice != null && character.canTalk){
			LeanAudio.play (curChar.specialVoice);
		}

		Destroy (itm.obj);
		spawned.Remove (itm);


		GameObject inst = Overlay.instance.CreateOverlayObj (spr, pos);

		AudioSource src = inst.AddComponent<AudioSource> ();
		src.clip = itm.item.sound;
		src.Play ();

		GameObject poof = Instantiate (pickupEff, inst.transform.position, inst.transform.rotation);
		poof.layer = LayerMask.NameToLayer ("Overlay");
		Destroy(poof, 2);

		float timer = 0.4333f;
        
        

		if (itm.type == PickableItem.Type.COIN) {
        
			if (Overlay.instance.enabled) {
            
               //Overlay.instance.MoveTo (inst, bank.coinTargetPos, timer, OnCoinEnd);
               moveCoin(inst, timer);
			}
			else {
				OnCoinEnd ();
			} 
		}
		else if (itm.type == PickableItem.Type.BOMB) {
			Overlay.instance.MoveTo (inst, bomb.position, 0.6f, LeanTweenType.easeInOutSine, true, true, OnBombEnd);
            
		} else if (itm.type == PickableItem.Type.PINATA) {
			for (int i = 0; i < itm.item.coinCount; i++) {
				LeanTween.delayedCall (0.2f * i, () => {
                
                    Vector3 pek = pos + Random.insideUnitCircle.normalized * 15;
					
					GameObject o = Overlay.instance.CreateOverlayObj (coin, pek);
					o.name += i;
                    moveCoin(o, timer);
                    
					//Overlay.instance.MoveTo (o, bank.bankPos, timer, OnCoinEnd);
				});
			}
		}
		else if (itm.type == PickableItem.Type.BOX) {
			LeanTween.delayedCall (character.gameObject, 0.3f, character.Switch);
			
		}
		else if (itm.type == PickableItem.Type.CRYSTAL) {
			if (FMC_GameDataController.instance != null){
				FMC_GameDataController.instance.makeStoryModeEasier ();
				Debug.LogWarning ("STORY MODE MADE EASIER");
				GameObject go = GameObject.Find ("diamonds");
				ParticleSystem part = go.GetComponent<ParticleSystem> ();
				part.Play ();
			}
		}
		else if (itm.type == PickableItem.Type.LVL) {
			inst.SetActive (false);
			exp.LvlUp ();
			FindObjectOfType<LevelUpScreen> ().Init (inst.transform.position);

		}
		else if (itm.type == PickableItem.Type.LOCK) {
			
			inst.SetActive (false);
			main.MainMenu ();
			bank.Wipe ();


		}
		else if (itm.type == PickableItem.Type.ROCKET) {
			inst.SetActive (false);
			main.Fly (Random.Range(3, 10));
		}
		else {
			
		}

	}
    
    private void moveCoin (GameObject inst, float timer) {
    
        FMC_Settings currentSetting = FMC_GameDataController.instance.getCurrentSettings();
                        
        bank.setCoinTargetPosition();
        Vector3 dest = bank.bankCamera.ScreenToWorldPoint(new Vector3(bank.coinTargetPos.x, bank.coinTargetPos.y, 10));
        dest.y = Mathf.Abs(dest.y);
        dest.x = 0;
        
        if (currentSetting is FMC_Settings_StoryMode) {
            dest.x = - 1.2f;
        }
        
        LeanTween.scale (inst, Vector2.one * 0.8f, timer * 0.1f).setEase (LeanTweenType.easeOutCubic).setOnComplete (() => {
            LeanTween.scale (inst, Vector2.one * 0.25f, timer * 0.9f).setEase (LeanTweenType.easeInCubic);
        });
        LeanTween.move(inst, dest, timer).setOnComplete(() => {
            OnCoinEnd();
        });
    }

	void RewardPerformance(string name) {
    
		Debug.LogWarning ("Rewarding with "+name);
		if (name == "Rocket" && CountItems (PickableItem.Type.ROCKET) == 0 && !ClimberStateManager.isFlying) {
			if (CheckAvailable (7) || GetItem (7).type != PickableItem.Type.LVL){
				SpawnForced (PickableItem.Type.ROCKET, 7, 7);
			}
			else{
				Debug.LogError ("Can't Spawn rocket on Level Token");
			}
		}
		else if (name == "Diamond" && main.count > 0) {
			SpawnForced(PickableItem.Type.CRYSTAL, -1, -1);
		}
		else if (name == "makeEasier") {
			if (help.fold) {
				smHelp.Show (false);
			}
			else{
				help.Show (false);
			}
		}
		else if (name == "makeHarder"){
			if (help.fold) {
				smHelp.Show (true);
			}
			else {
				help.Show (true);
			}
		}
	}

	void OnCoinEnd(){
		//Debug.Log ("Bouncy bops");
		int reward = 1 + CharacterStorage.curChar.rewardBonus;
		if (FMC_GameDataController.instance != null){
			if (ClimberStateManager.inCampaign){
				//reward += Mathf.FloorToInt(exp.GetLevel()/2);
			}
			else if (ClimberStateManager.inTraining){
				//reward += FMC_GameDataController.instance.getCurrentRangeOfNumbers ().ToString().Length;//Mathf.FloorToInt(exp.GetLevel()/2);
			}
		}
		
		bank.CheckIn (reward);
	}
	void OnBombEnd(){
		bomb.Charge ();

	}


	//=============================================
	//========= All the spawning methods ==========
	//=============================================
	public void SpawnForced(PickableItem.Type t, int pos, int at){
		Spawn (ItemTypeToId (t), stairs.GetStepPosition (pos), at, true);
	}

	public void SpawnRandom(){
		SpawnRandom (offset + 1, offset);
	}
	public void SpawnRandom (int pos, int at){
		SpawnRandom (stairs.GetStepPosition (pos), at);
	}

	public void SpawnRandom(int at){
		SpawnRandom (at, at);
	}

	public void SpawnRandom(Vector3 position, int at){
		int randum = ChooseRandom ();
		if (randum == ItemTypeToId(PickableItem.Type.BOMB) && CountItems (PickableItem.Type.BOMB) >= bomb.capacity - bomb.charges) {
			Spawn (PickableItem.Type.COIN, position, at, false);
		}
		else {

			Spawn (randum, position, at, false);
		}
	}
	public void Spawn(PickableItem.Type t){
		Spawn (ItemTypeToId (t));
	}
	public void Spawn(int id){
		Spawn (id, stairs.GetStepPosition(offset), offset, false);
	}
	public void Spawn (PickableItem.Type t, Vector3 position, int at, bool force) {
		Spawn (ItemTypeToId (t), position, at, force);
	}
	//The base spawn function should be used only by other Spawn overloads
	public void Spawn (int id, Vector3 position, int at, bool force) {
		bool isAvalable = CheckAvailable (at);
		if (isAvalable || force) {
			if (stairs == null) {
				stairs = FindObjectOfType<StairController> ();
			}
			GameObject go = Instantiate (drops [id], position, drops [id].transform.rotation, stairs.root.transform);
			if (!isAvalable) {
				
				Despawn(at);
			}

			DropItem itm = new DropItem (go, at);
			spawned.Add (itm);
			itm.item.SetPlace (itm.at);

			//go.transform.parent = parentStair;

		}
	}

	public void Despawn(int at){
		for (int i = 0; i < spawned.Count; i++) {
			if (spawned [i].at == at) {
				Destroy(spawned[i].obj);
				spawned.RemoveAt (i);
			}
		}
	}

	public DropItem GetItem(int at){
		for (int i = 0; i < spawned.Count; i++) {
			if (spawned [i].at == at) {
				return spawned [i];	
			}
		}

		return null;

	}
	public int CountItems(PickableItem.Type t){
		int result = 0;
		for (int i = 0; i < spawned.Count; i++) {
			if(spawned[i].type == t){
				result++;
			}
		}
		return result;
	}
	//happens when stairs move
	public void ShiftIndex(int dir){
		PickableItem.Type t = PickableItem.Type.COIN;
		for (int i = 0; i < spawned.Count; i++) {
			//Check world position
			if (spawned [i].obj.transform.position != stairs.GetStepPosition (spawned [i].at)) {
				spawned [i].obj.transform.position = stairs.GetStepPosition (spawned [i].at);
			}

			spawned [i].at += dir;
			spawned [i].item.SetPlace(spawned[i].at);
			//Debug.Log (i + ". " + spawned [i].obj.name + " at " + spawned [i].pos);

		}
		for (int i = 0; i < spawned.Count; i++) {
			if (spawned [i].at == 0 || spawned[i].type != PickableItem.Type.CRYSTAL && spawned[i].at < 0) {
				spawned [i].Echo ();
				t = spawned [i].type;
				Pickup (spawned [i]);

			}
			else if (spawned[i].at < -15){
				Despawn (spawned[i].at);
			}


		}

		if (dir < 0) {
			if (exp.GetExperienceToLevel () > 1 || t == PickableItem.Type.LVL) {
				exp.AddXp ();
			}
			else {
				if (ClimberStateManager.inTraining) {
					exp.AddXp ();
				}
				else {
					Debug.Log ("Cant give xp");
				}
			}

			SpawnRandom ();
			//Debug.Log ("Shift spawns at "+offset);

		}
		else {
			exp.SubtractXp ();
			//Debug.Log ("Shift spawns at 1");
			SpawnRandom(0,1);

		}
	}

	public void Detach(){
		for (int i = 0; i < spawned.Count; i++) {
			spawned [i].obj.transform.parent = null;
		}
	}
	public void Reattach(GameObject root){
		for (int i = 0; i < spawned.Count; i++) {
			spawned [i].obj.transform.parent = root.transform;
		}
	}

	public int ItemTypeToId(PickableItem.Type t){
		for (int i = 0; i < types.Length; i++) {
			if (types[i] == t){
				return i;
			}
		}
		return -1;
	}

//	public void SetStair(Transform t){
//		parentStair = t;
//	}


	void InitRandom(){
		chances = new float[drops.Length];
		float total = 0;
		for (int i = 0; i < weights.Length; i++) {
			total += weights [i];
		}
		for (int i = 0; i < chances.Length; i++) {
			chances [i] = weights [i] / total;
		}
	}

	int ChooseRandom(){
		float roll = Random.Range (0, 1f);
		float sum = 0;
		for (int i = 0; i < drops.Length; i++) {
			//Debug.Log (chances [i]);
			sum += chances [i];
			if (roll < sum){
//				Debug.Log ("Sum is " + sum +" roll is "+roll);
				return i;
			}
		}
		return -1;
	}
	/// Check if the slot is available

	bool CheckAvailable(int slot){
		//bool result = true;
		for (int i = 0; i < spawned.Count; i++) {
			if (spawned [i].at == slot) {
				return false;
			}
		}
		return true;
	}

	public Transform topItemTransform{
		get{ return spawned [spawned.Count - 1].obj.transform; }
	}

	public class DropItem{
		PickableItem _item;
		GameObject _obj;
		int _pos;
		public DropItem(GameObject obj, int pos){
			_obj = obj;
			_pos = pos;
			_item = obj.GetComponent<PickableItem>();
		}

		/// Shows which step item will be picked at.

		public int at{
			get{ return _pos;  }
			set{ _pos = value; }
		}

		public GameObject obj {
			get { return _obj; }
		}
		public PickableItem item{
			get{ return _item; }
		}
		public PickableItem.Type type{
			get { return _item.type; }
		}
		public void Echo(){
			//Debug.Log ("My name is"+obj.name+" place is "+_pos + " or "+_item.place);
		}

	}
}

