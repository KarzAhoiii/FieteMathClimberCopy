using UnityEngine;
using System.Collections;
using Spine.Unity;
using Spine;

public class SpineAnimation : MonoBehaviour {

	private SkeletonAnimation skeletonAnimation;
	private bool isPlaying = false;
	private bool adjustHeight;

	float timeScale;


	void Awake(){
		skeletonAnimation = transform.GetComponent<SkeletonAnimation>();
	}

	void Start () {
		timeScale = skeletonAnimation.timeScale;


	}

//	public void OnMouseDown () {
//		
//		if (!isPlaying) {
//			isPlaying = true;
//			skeletonAnimation.state.SetAnimation(0, "touch", false);
//			float delay = skeletonAnimation.skeleton.data.FindAnimation("touch").duration;
//			Invoke("resetIdle", delay);
//
//		}
//	
//	}

	public void Jump(){
	    skeletonAnimation.state.SetAnimation(0, "jump_up", false);
	}
	public void Fall(){
	    skeletonAnimation.state.SetAnimation (0, "jump_down", false);
	}

	public void Tap() {
    
		if (!isPlaying) {
			//Debug.Log (gameObject.name+"is fallin");
			isPlaying = true;
			Spine.Animation up = skeletonAnimation.skeleton.data.FindAnimation ("tap");
			if (up != null) {
				skeletonAnimation.state.SetAnimation (0, "tap", false);
			}
			else {
				Debug.LogWarning ("There's no tap anim for this character");
				isPlaying = false;
			}
		}
		else
			Debug.LogWarning ("Tap anim already playig");
		
	}


//	public bool isFrozen{
//		get{ return skeletonAnimation.timeScale == 0;}
//	}
//	void OnTrackEnd (TrackEntry trc){
//		resetIdle ();
//	}
	public void resetIdle () {
		//Debug.Log ("reset to Idle");
		isPlaying = false;

		TrackEntry trc = skeletonAnimation.state.SetAnimation(0, "idle", true);
		trc.mixDuration = 0;
	}

	public SkeletonAnimation skeleton {
		get{ return skeletonAnimation; }
	} 
	public float savedTimeScale{
		get{ return timeScale; }
	}
}
