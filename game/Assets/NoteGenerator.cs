using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Consumes beats and notifies pads to animate themselves
 */
public class NoteGenerator : MonoBehaviour
{

	public Pad[] pads;
	// The time of the "GET READY" animation, should there be one.
	public float prepTime = 3;
	// The total time from the beginning of the pad animation to the onset in the song
	public float animationTime = 3;

	// TODO: populate this array
	Note[] notes;
	// Unity is dumb.
	int index = 0;

	// Use this for initialization
	void Start()
	{
		// TODO: Give the user some indication a song is about to start

		// Does unity have a better way to "sleep"?
		Invoke("afterPreSong", prepTime);
	}

	void afterPreSong() {
		// Be sure to take into account the delay needed for animations
		Invoke("beginSong", animationTime);
		Invoke ("deployBeat", notes [0].time);
	}

	// TODO: begin music
	void beginAudio() { 
	}

	void deployBeat() {
		foreach(int i in notes[index].pads) {
			pads [i].onBeat (notes[index]);
		}
		index++;
		if (notes.Length > index)
			Invoke ("deployBeat", notes [index].time - notes [index - 1].time);
	}

}

public struct Note
{
	public int[] pads;
	public float time;
}