using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Consumes beats and notifies pads to animate themselves
 */
public class NoteGenerator : MonoBehaviour
{
	// References to the pad gameobjects' scripts
	public Pad[] pads;
	// The time of the "GET READY" animation, should there be one.
	public float prepTime = 3;
	// The total time from the beginning of the pad animation to the onset in the song
	public float animationTime = 3;

	// TODO: make this work with difficulty selection.
	public bool isEasy = true;

	int index = 0;

	SongData song;
	List<NoteData> notesToUse;

	// Use this for initialization
	void Start()
	{
		// Load the song
		print(Application.persistentDataPath);
		song = new SongData("<PATH_OMITTED>");

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
		Invoke("beginSong", animationTime);
		Invoke("deployBeat", notesToUse[0].offsetMS);
	}

	// TODO: begin music
	void beginAudio() { 
		// use song.stems
	}

	void deployBeat() {
		NoteData note = notesToUse[index];
		pads[note.midiPadIndex].onBeat(note);
		index++;
		// if this note is a chord
		while (notesToUse[index].offsetMS == note.offsetMS)
		{
			pads[notesToUse[index].midiPadIndex].onBeat(notesToUse[index]);
			index++;
		}

		// Deploy the next note, remember index was already incremented
		Invoke("deployBeat", notesToUse[index].offsetMS - note.offsetMS);
	}

}