using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAnimation: MonoBehaviour, IPauseHandler {

	ParticleSystem[] particles;

	float timer;
	public bool destroy;
	// Use this for initialization
	void Start () {
		particles = GetComponentsInChildren<ParticleSystem> ();
		ClimberStateManager.Subscribe (this);

		for (int i = 0; i < particles.Length; i++) {
			if (!particles[i].main.loop && particles[i].main.duration > timer){
				timer = particles [i].main.duration;
			}
		}
	}

	void Update(){
		if ( destroy && !particles[0].isPaused){
			timer -= Time.deltaTime;
			if (timer < 0){
				ClimberStateManager.Unsubscribe (this);
				Destroy (gameObject);
			}
		}
	}
	// Update is called once per frame
	public void Pause () {
		for (int i = 0; i < particles.Length; i++) {

			if (particles [i].isPlaying) {
				particles [i].Pause (true);
			}

		}

	}

	public void Unpause(){
		for (int i = 0; i < particles.Length; i++) {
			if (particles[i].isPaused){
				particles[i].Play (true);
			}
		}
	}

	public void Destroy(){
		
	}
}
