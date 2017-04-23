using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorBehavior : MonoBehaviour {

	void Start() {
		// Play animations at 24fps. They look better that way.
		gameObject.GetComponent<Animator>().speed = NoteGenerator.animationScale;
		// Destroy animation only after the sparkles are done. 
		Destroy (gameObject, NoteGenerator.trueAnimationTime); 
	}

	void Update() {
		
	}
}
