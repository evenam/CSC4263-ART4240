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
		// Load the song
		print(Application.dataPath);
		song = new SongData(Application.dataPath+"/Music/example_song/example_song.dat");

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
		Invoke("beginAudio", animationTime);
		// TODO: are all inital offsets at 0?
		Invoke("deployBeat", notesToUse[0].offsetMS);
	}

	// TODO: use stems instead of full track
	void beginAudio() {
		Camera.main.GetComponent<AudioSource>().Play();
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
