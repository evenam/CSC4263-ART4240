using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        Button EasyButton = GameObject.Find("EasyDiffButton").GetComponent<Button>();
        Button AdvButton = GameObject.Find("AdvDiffButton").GetComponent<Button>();
        if (index == 3 || index == 4)
        {
            
            EasyButton.interactable = false;
            AdvButton.interactable = false;
        }
        else
        {
            EasyButton.interactable = true;
            AdvButton.interactable = true;
        }
    }
}
