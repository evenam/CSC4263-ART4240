using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Text;
using System.IO;
using System;

public struct NoteData
{
    public uint midiPadIndex;
    public uint stemIndex;
    public uint bar;
    public uint fraction;
    public uint offsetMS;
}

public class SongData
{
    public string title;
    public string artist;
    public uint BPM;
    public List<string> stems;
    public uint offsetMS;
    public uint notePrecision;  // bars advance 1/precision, hits on fraction/precision
    public uint tailMS;
    public List<NoteData> easyNoteData;
    public List<NoteData> advNoteData;
    public uint easyHighScore;
    public uint advHighScore;

    /*

    duration = offsetMS + tailMS + bars / BPM 

    misc:
    https://docs.unity3d.com/ScriptReference/Application-persistentDataPath.html
    http://answers.unity3d.com/questions/279750/loading-data-from-a-txt-file-c.html
    */

    private string parseString(string input)
    {
        if (input[0] == '\"')
        {
            int index = 1;
            string str = "";
            while (input[index] != '\"')
            {
                if (input[index] == '\\')
                {
                    index++;
                    if (input[index] == 'n')
                    {
                        str += '\n';
                    }
                    else if (input[index] == 't')
                    {
                        str += '\t';
                    }
                    else if (input[index] == '\'')
                    {
                        str += '\'';
                    }
                    else if (input[index] == '\"')
                    {
                        str += '\"';
                    }
                    index++;
                }
                else
                {
                    str += input[index++];
                }
            }
            return str;
        }
        else
        {
            return "";
        }
    }

    private NoteData parseNoteData(string args)
    {
        // woe is me without LINQ
        string[] sArgs = args.Split(',');
        NoteData data;
        data.midiPadIndex = UInt32.Parse(sArgs[0]);
        data.stemIndex = UInt32.Parse(sArgs[1]);
        // data.offsetMS = UInt32.Parse(sArgs[2]);
        data.bar = UInt32.Parse(sArgs[3]);
        data.fraction = UInt32.Parse(sArgs[4]);

		double beatsPerMilli = (this.notePrecision / 4.0) * (double)(this.BPM) / 60d / 1000d;
		double beatIndex = ((data.bar * this.notePrecision) + data.fraction);
		data.offsetMS = (uint)(beatIndex / beatsPerMilli);
        return data;
    }

    private void parseStems(string args)
    {
        stems.Clear();
        string[] sArgs = args.Split(',');
        for (uint i = 0; i < sArgs.Length; i ++)
        {
			string stem = parseString(sArgs[i].Trim());
            stems.Add(stem);
        }
    }

    public SongData(string contents)
    {
        stems = new List<string>();
        easyNoteData = new List<NoteData>();
        advNoteData = new List<NoteData>();
        try
        {
            // file reading stuff
            string line;
            StringReader theReader = new StringReader(contents);
            using (theReader)
            {
                do
                {
                    // each line...
                    line = theReader.ReadLine();
                    if (line == null) continue;
                    line = line.Trim();
                    if (line == "") continue;
                    if (line.Substring(0, 2) == "//")
                    {
                        // comment line
                        continue;
                    }

                    // determine line type
                    string command = line.Split(':')[0].TrimEnd();
                    string args = line.Split(':')[1].TrimStart();

                    if (command == "Title")
                    {
                        title = parseString(args);
                    }
                    else if (command == "Artist")
                    {
                        artist = parseString(args);
                    }
                    else if (command == "BPM")
                    {
                        BPM = UInt32.Parse(args);
                    }
                    else if (command == "Offset")
                    {
                        offsetMS = UInt32.Parse(args);
                    }
                    else if (command == "Tail")
                    {
                        tailMS = UInt32.Parse(args);
                    }
                    else if (command == "Precision")
                    {
                        notePrecision = UInt32.Parse(args);
                    }
                    else if (command == "Offset")
                    {
                        offsetMS = UInt32.Parse(args);
                    }
                    else if (command == "Stems")
                    {
                        parseStems(args);
                    }
                    else if (command == "EasyNote")
                    {
                        NoteData data = parseNoteData(args);
                        easyNoteData.Add(data);
                    }
                    else if (command == "AdvNote")
                    {
                        NoteData data = parseNoteData(args);
                        advNoteData.Add(data);
                    }
                }
                while (line != null);

                // cleanup
                theReader.Close();
            }
			// Sort notes by millisecond
			easyNoteData.Sort((x, y) => (int)(x.offsetMS - y.offsetMS));
			advNoteData.Sort((x, y) => (int)(x.offsetMS - y.offsetMS));
        }
        catch (Exception e)
        {
            MonoBehaviour.print(e.StackTrace.ToString());
        }

        // TODO: persistant highscores using unity's build in score system
    }
}
