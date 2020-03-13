using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class HackyCharSpawner : MonoBehaviour {

    public bool isForIapScreen;

    CharacterStorage charStorage;
    RecolorManager recolor;
    SkeletonAnimation skeletonAnimation;
    CharacterProfile newChar;

    void Start () {
        charStorage = FindObjectOfType<CharacterStorage>();
        recolor = RecolorManager.Get();//FindObjectOfType<RecolorManager>();
        SpawnCurrent();

    }

    void OnEnable(){
        if (transform.childCount > 0) {
            Transform child = transform.GetChild (0);
            Destroy (child.gameObject);
            SpawnCurrent ();
        }

    }
    

    public void onCharacterClick () {
    
        skeletonAnimation.state.SetAnimation(0, "tap", false);
        skeletonAnimation.state.AddAnimation(0, "idle", false, 0);
        
        if (newChar.tapVoice != null) {
            LeanAudio.play (newChar.tapVoice);
        }
    }


    public void SpawnCurrent() {
        
        newChar = charStorage.GetCharacter(Persistence.currentChar);
        GameObject go = Instantiate (newChar.prefab, transform.position, Quaternion.identity);
        go.transform.parent = transform;
        go.transform.localScale = Vector3.one;
        
        skeletonAnimation = go.GetComponent<SkeletonAnimation>();

        if (isForIapScreen)
            go.GetComponent<MeshRenderer>().sortingLayerName = "InAppPurchase";
        else
            go.GetComponent<MeshRenderer>().sortingLayerName = "SelectionScreen";

        recolor.Recolor(Persistence.currentChar);
    }
}