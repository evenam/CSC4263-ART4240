using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSongOnClick : MonoBehaviour
{
    int[] songs;
    public int index;

	void Start ()
    {
        songs = new int[5];
        index = 0;
	}
	
	void Update () {
		
	}

    public void LoadSongByIndex()
    {
        print("index: " + index);
        // TODO: Use index value to choose song for gameplay
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
