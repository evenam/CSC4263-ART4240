using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
	public static float animationTime = 1.2f;

	// TODO: make this work with difficulty selection.
	public bool isEasy = true;

	// countdown box
	public Text uiTextCountdown;
	private int countdownSecondsRemaining;

	int index = 0;

	SongData song;
	List<NoteData> notesToUse;

    public ScoreController highScoreController;

	// Use this for initialization
	void Start()
	{
        // The stems don't start playing instantly, so I made it so it waits 5 
        // seconds before checking if each of the audio sources are done playing.
        StartCoroutine(waitForSongCoroutine());

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

        highScoreController = new ScoreController();
		// TODO: Give the user some indication a song is about to start

		// Does unity have a better way to "sleep"?
		Invoke("afterPreSong", prepTime);

		countdownSecondsRemaining = 3;
		Countdown ();
	}

	void afterPreSong() {
		// Be sure to take into account the delay needed for animations
		Invoke("beginAudio", animationTime);
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
		pads[note.midiPadIndex].GetComponent<Pad>().onReady(note);
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

    int stemsDonePlaying = 0;
    bool songsArePlaying = false;
    public IEnumerator waitForSongCoroutine()
    {
        yield return new WaitForSeconds(5);
        songsArePlaying = true;
    }

    private void Update()
    {
        if(songsArePlaying)
        {
            CheckStems();
        }
        if(stemsDonePlaying != 0)
        {
            highScoreController.SetHighScore();
            print("song over");
            GameObject songOverParent = GameObject.Find("Canvas");
            GameObject songOver = songOverParent.transform.FindChild("FinalScoreGroup").gameObject;
            GameObject gameplay = songOverParent.transform.FindChild("GameplayGroup").gameObject;
            gameplay.SetActive(false);
            songOver.SetActive(true);
        }
    }

    
    void CheckStems()
    {
        foreach (AudioSource src in gameObject.GetComponents<AudioSource>())
        {
            if (!src.isPlaying)
            {
                stemsDonePlaying++;
            }
        }
        // Added this so the counter is reset if not all of the stems are finished playing.
        if (stemsDonePlaying < 4)
            stemsDonePlaying = 0;
    }


	// countdown function
	void Countdown() {
		if (countdownSecondsRemaining > 0) {
			uiTextCountdown.text = countdownSecondsRemaining.ToString ();
			countdownSecondsRemaining--;
			Invoke ("Countdown", 1f);
		} else if (countdownSecondsRemaining == 0) {
			uiTextCountdown.text = "GO!";
			countdownSecondsRemaining--;
			Invoke ("Countdown", 1f);
		} else {
			uiTextCountdown.text = "";
		}
	}
}
