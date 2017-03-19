﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Script used to animate buttons, and probably get player input and calculate score.
 * Called by the NoteGenerator Script.
 */
public class Pad : MonoBehaviour {

	public KeyCode triggerKey;

	public const uint windowMS = 1000;
	public const uint alertMS = 100;

	private float window, alert;

	enum State {
		OFF,
		READY,
		HIT,
		MISS
	}

	public Sprite off, ready, hit, miss;

	private State state;

	// Use this for initialization
	void Start () {
		state = State.OFF;
	}
	
	// Update is called once per frame
	void Update () {
		SpriteRenderer spr = GetComponent<SpriteRenderer>();
		float dt = Time.deltaTime;
		switch (state) {
		case (State.OFF):
			spr.sprite = off;
			handleOff (dt); 
			break;
		case (State.READY): 
			spr.sprite = ready; 
			handleReady (dt);
			break;
		case (State.HIT): 
			spr.sprite = hit; 
			handleHit (dt);
			break;
		case (State.MISS): 
			spr.sprite = miss; 
			handleMiss (dt);
			break;
		} 
	}

	private void handleOff(float dt) {
		if (state != State.OFF)
			return;
		
	}

	private void handleReady(float dt) {
		if (state != State.READY)
			return;
		window -= dt;
		if (window <= 0.0f) {
			alert = alertMS / 1000.0f;
			state = State.MISS;
		}
	}
		
	private void handleHit(float dt) {
		if (state != State.HIT)
			return;
		alert -= dt;
		if (alert <= 0) {
			state = State.OFF;
		}
	}

	private void handleMiss(float dt) {
		if (state != State.MISS)
			return;
		alert -= dt;
		if (alert <= 0) {
			state = State.OFF;
		}

	}

	// Called when this pad should begin showing a beat
	public void onBeat(NoteData beat) {
		state = State.HIT;
		alert = alertMS / 1000.0f;
		window = windowMS / 1000.0f;
		print("Pad " + beat.midiPadIndex + " got note at " + beat.offsetMS);
	}
}
