using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorBehavior : MonoBehaviour {

	private float animationTime = 1.0f / NoteGenerator.animationTime;
	public float time = 0.0f;

	void Start() {
		gameObject.GetComponent<Animator> ().speed = animationTime;
		Destroy (gameObject, animationTime); 
	}

	void Update() {
		
	}
}
