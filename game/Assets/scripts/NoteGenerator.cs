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
	public static float prepTime = 3.0f;
	// Scaling factor of the animation. We like it better when played at double speed.
	// Retrieved by IndicatorBehavior and passed to Animator.speed there.
	public static int animationScale = 2;
	// The full length of the animation.
	// Should only be changed if the animation file itself changes.
	public static float trueAnimationTime = 2.5f / animationScale;
	// The total time from the beginning of the pad animation to the onset in the song
	// Animation time is 2.5 seconds, but the animation includes a sparkle effect that lasts 14 frames
	// The animation should be treated as a 16 frame animation at 12 fps. 
	public static float animationTime = 1.333333333f / animationScale;

	// TODO: make this work with difficulty selection.
	public bool isEasy = true;

	// True if missing a note does nothing
	public bool godMode = false;

	// countdown box
	public Text uiTextCountdown;
	// int version of prepTime
	private int countdownSecondsRemaining;

	int index = 0;

	SongData song;
	List<NoteData> notesToUse;

    public ScoreController highScoreController;
    public SpectrumToVisual spectrumVisual;

    // counts number of stems so song ends correctly for both easy and adv
    int stemCount = 0;

	// Track completion of song
	int stemsDonePlaying = 0;
	bool songsArePlaying = false;

	public TextAsset[] songDatFiles;

	// Use this for initialization
	void Start()
	{
		// Load the song data
		// song = new SongData(Application.dataPath+"/Resources/Music/example_song/example_song.dat");

        // Now gets which song to play from which song is selected in song select.
        // Will play last song selected when the game is running, so to test different songs have to start from song select.
		int index = PlayerPrefs.GetInt("songIndex");
		song = new SongData(songDatFiles[index].text);
        // Gets song difficulty the same way, but can save bool with PlayerPrefs so used 1 = easy, 0 = hard.
        if (PlayerPrefs.GetInt("songDifficulty") == 1)
            isEasy = true;
        else
            isEasy = false;

        // stems: example_track/track_pad.wav

        // Load audio sources for every stem
        foreach (string stem in song.stems)
		{
			Debug.Log(stem);
			AudioSource src = gameObject.AddComponent<AudioSource>();
			src.clip = Resources.Load("Music/"+stem) as AudioClip;

            stemCount++;
		}

		if (isEasy)
			notesToUse = song.easyNoteData;
		else
			notesToUse = song.advNoteData;


        highScoreController = new ScoreController();

		// Does unity have a better way to "sleep"?
		Invoke("afterPreSong", prepTime);

		countdownSecondsRemaining = (int)prepTime;
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
		songsArePlaying = true;
	}

	void deployBeat() {
		NoteData note = notesToUse[index];
		pads[note.midiPadIndex].GetComponent<Pad>().onBeat(note, isEasy ? 0.65f : 0.15f);
		index++;
		// if this note is a chord
		while (notesToUse[index].offsetMS == note.offsetMS)
		{
			pads[notesToUse[index].midiPadIndex].GetComponent<Pad>().onBeat(notesToUse[index], isEasy ? 0.65f : 0.15f);
			index++;
		}

		// Deploy the next note, remember index was already incremented
		// Invoke is in seconds, not milliseconds
		Invoke("deployBeat", (float)(notesToUse[index].offsetMS - note.offsetMS) / 1000F);
	}

    private void Update()
    {
        if(songsArePlaying)
        {
            CheckStems();
        }
        if(stemsDonePlaying != 0)
        {
			// The song is over
			songsArePlaying = false;
            highScoreController.SongOver();
            print("song over");
            GameObject songOverParent = GameObject.Find("Canvas");
            GameObject ActiveGameGroup = GameObject.Find("ActiveGameGroup");
            GameObject songOver = songOverParent.transform.FindChild("FinalScoreGroup").gameObject;
            GameObject gameplay = songOverParent.transform.FindChild("GameplayGroup").gameObject;
            spectrumVisual.DestroySpectrumTriangles();
            gameplay.SetActive(false);
            songOver.SetActive(true);
            ActiveGameGroup.SetActive(false);
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
		if (stemsDonePlaying < song.stems.Count)
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
