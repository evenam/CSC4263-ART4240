using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteData : MonoBehaviour
{
    public uint midiPadIndex;
    public uint stemIndex;
    public uint bar;
    public uint fraction;
    public uint offsetMS;
}

public class SongData : MonoBehaviour
{
    public const uint EASY_STEM_COUNT = 4;
    public const uint ADV_STEM_COUNT = 9;
    public string title;
    public string artist;
    public uint BPM;
    public string[] easyStems;
    public string[] advStems;
    public uint offsetMS;
    public uint notePrecision;  // bars advance 1/precision, hits on fraction/precision
    public uint tailMS;
    public NoteData[] easyNoteData;
    public NoteData[] advNoteData;
    public uint easyHighScore;
    public uint advHighScore;

    /*

    duration = offsetMS + tailMS + bars / BPM 

    misc:
    https://docs.unity3d.com/ScriptReference/Application-persistentDataPath.html
    http://answers.unity3d.com/questions/279750/loading-data-from-a-txt-file-c.html
    */
}

public class Song : MonoBehaviour
{
    SongData data;

    public Song(string filename)
    {

    }
}
