using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorBehavior : MonoBehaviour {

	public float animationTime = 1.0f;
	public float time = 0.0f;

	void Start() {
		gameObject.GetComponent<Animator> ().speed = 1.03f / animationTime;
		Destroy (gameObject, this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length); 
	}

	void Update() {
		
	}
}
