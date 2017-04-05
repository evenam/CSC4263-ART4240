using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Script used to animate buttons, and probably get player input and calculate score.
 * Called by the NoteGenerator Script.
 */
public class Pad : MonoBehaviour {

	public KeyCode triggerKey;

	public const uint windowMS = 250;
	public const uint alertMS = 100;

	public int wrongMS = 100;


	private float window, alert;

	private NoteData currentBeat;

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

		if (Input.GetKeyDown (triggerKey)) {
			hitTheNote ();
		}

        int i = 0;
        while (i < Input.touchCount)
        {

            if (Input.GetTouch(i).phase == TouchPhase.Began)
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position), -Vector2.up);

                if (hit.collider != null)
                {

                    if (hit.collider == GetComponent<CircleCollider2D>()) 
                    {
                        hitTheNote();
                    }

                }
            }
            ++i;
        }


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
		// Only reset stem when user hits a note
		// TODO: play a miss sound effect
		// TODO: reset only the missed stem maybe?
		Invoke("resetStems", ((float)wrongMS) / 1000f);
	}

	private void handleMiss(float dt) {
		if (state != State.MISS)
			return;
		alert -= dt;
		if (alert <= 0) {
			state = State.OFF;
		}
		Camera.main.GetComponents<AudioSource>()[currentBeat.stemIndex].volume = 0f;
	}

	// Called when this pad should begin showing a beat
	public void onBeat(NoteData beat) {
		currentBeat = beat;
		state = State.READY;
		window = windowMS / 1000.0f;
		print("Pad " + beat.midiPadIndex + " got note at " + beat.offsetMS);
	}

	private void hitTheNote() {
		// KICK IT!
		if (state == State.READY) {
			print ("HIT!");
			state = State.HIT;
			alert = alertMS / 1000.0f;
		} else {
			print ("MISS!");
			state = State.MISS;
			alert = alertMS / 1000.0f;
		}
	}

	void resetStems()
	{
		foreach (AudioSource src in Camera.main.GetComponents<AudioSource>())
			src.volume = 1.0f;
	}
}
