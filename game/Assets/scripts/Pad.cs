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

	// Reference to indicator that will be instantiated and destroyed
	public GameObject indicatorRef;


	private float window, alert;

	private NoteData currentBeat;

    public ScoreController scoreCont;

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

        GameObject scoreControllerObject = GameObject.FindWithTag("scoreController");
        scoreCont = scoreControllerObject.GetComponent<ScoreController>();
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

            scoreCont.incScore();   //increases the score on successful hit
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

            scoreCont.multiplyerReset();    //resets the multiplyer to x1
		}
		Camera.main.GetComponents<AudioSource>()[currentBeat.stemIndex].volume = 0f;
	}

	// Called when this pad should pre-animate a beat
	public void onReady(NoteData beat) {
		// Spawn an indicator. It will destroy itself
		GameObject instance = Instantiate(indicatorRef, this.transform.position, this.transform.rotation);
		// move the instance above this game object
		// Unity is stupid.
		instance.transform.position = new Vector3(instance.transform.position.x - 0.032236f, instance.transform.position.y - 0.026407f, 2.0f);

		StartCoroutine(this.onBeat (beat));
	}

	// Called when this pad should perform a beat
	public IEnumerator onBeat(NoteData beat) {
		yield return new WaitForSeconds(NoteGenerator.animationTime);
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
