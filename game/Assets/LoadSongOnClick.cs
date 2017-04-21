using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSongOnClick : MonoBehaviour
{
    public string[] songs;
    public static int index;

	void Start ()
    {
        songs = new string[5] { "/Resources/Music/example_song/example_song.dat", "/Resources/Music/BadSong/badsong.dat", "/Resources/Music/vaperwaver2/vaporwave.dat", null, null};
        index = 0;
	}
	
	void Update () {
		
	}

    public void LoadSongByIndex(int isEasy)
    {
        print("index: " + index + ", song: " + songs[index] + ", isEasy: " + isEasy);
        PlayerPrefs.SetString("songPath", songs[index]);
        PlayerPrefs.SetInt("songDifficulty", isEasy);
    }

    public void changeSong(bool direction)
    {
        if (direction)  // 0 = left, 1 = right
        {
            if (index == 4)
                index = 0;
            else
                index++;
        }
        else
        {
            if (index == 0)
                index = 4;
            else
                index--;
        }
    }
}
