using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Consumes beats and notifies pads to animate themselves
 */
public class NoteGenerator : MonoBehaviour
{
	// References to the pad gameobjects
	public GameObject[] pads;
	// The time of the "GET READY" animation, should there be one.
	public float prepTime = 3;
	// The total time from the beginning of the pad animation to the onset in the song
	public float animationTime = 0;

	// TODO: make this work with difficulty selection.
	public bool isEasy = true;

	int index = 0;

	SongData song;
	List<NoteData> notesToUse;

	// Use this for initialization
	void Start()
	{
		// Load the song data
		song = new SongData(Application.dataPath+"/Resources/Music/example_song/example_song.dat");

		// stems: example_track/track_pad.wav

		// Load audio sources for every stem
		foreach (string stem in song.stems)
		{
			Debug.Log(stem);
			AudioSource src = gameObject.AddComponent<AudioSource>();
			src.clip = Resources.Load("Music/"+stem) as AudioClip;
		}

		if (isEasy)
			notesToUse = song.easyNoteData;
		else
			notesToUse = song.advNoteData;
			

		// TODO: Give the user some indication a song is about to start

		// Does unity have a better way to "sleep"?
		Invoke("afterPreSong", prepTime);
	}

	void afterPreSong() {
		// Be sure to take into account the delay needed for animations
		Invoke("beginAudio", animationTime / 1000.0f);
		// TODO: are all inital offsets at 0?
		Invoke("deployBeat", notesToUse[0].offsetMS);
	}

	void beginAudio()
	{
		foreach (AudioSource src in gameObject.GetComponents<AudioSource>())
		{
			src.Play();
		}
	}

	// TODO: handle the obviously-omitted end of song case
	void deployBeat() {
		NoteData note = notesToUse[index];
		pads[note.midiPadIndex].GetComponent<Pad>().onBeat(note);
		index++;
		// if this note is a chord
		while (notesToUse[index].offsetMS == note.offsetMS)
		{
			pads[notesToUse[index].midiPadIndex].GetComponent<Pad>().onBeat(notesToUse[index]);
			index++;
		}

		// Deploy the next note, remember index was already incremented
		// Invoke is in seconds, not milliseconds
		Invoke("deployBeat", (float)(notesToUse[index].offsetMS - note.offsetMS) / 1000F);
	}

}
