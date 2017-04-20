using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayHighScores : MonoBehaviour {

    int[] EasyHighScores; //= new int[3] { PlayerPrefs.GetInt("e_HIGH_SCORE_1"), PlayerPrefs.GetInt("e_HIGH_SCORE_2"), PlayerPrefs.GetInt("e_HIGH_SCORE_3") };
    int[] AdvHighScores; // = new int[3] { PlayerPrefs.GetInt("a_HIGH_SCORE_1"), PlayerPrefs.GetInt("a_HIGH_SCORE_2"), PlayerPrefs.GetInt("a_HIGH_SCORE_3") };
    public Text highScoreText;
    
    // Use this for initialization
    void Start () {

		EasyHighScores = new int[3] { PlayerPrefs.GetInt("e_HIGH_SCORE_1"), PlayerPrefs.GetInt("e_HIGH_SCORE_2"), PlayerPrefs.GetInt("e_HIGH_SCORE_3")};
        AdvHighScores = new int[3] { PlayerPrefs.GetInt("a_HIGH_SCORE_1"), PlayerPrefs.GetInt("a_HIGH_SCORE_2"), PlayerPrefs.GetInt("a_HIGH_SCORE_3") };

        highScoreText.text = ("<size=200><color=#ffa500ff>Track 1</color></size>\n<size=100>" + EasyHighScores[0].ToString("D8") + "\t" + AdvHighScores[0].ToString("D8") + "</size>\n\n\n" +
            "<size=200><color=#ff0000ff>Track 2</color></size>\n<size=100>" + EasyHighScores[1].ToString("D8") + "\t" + AdvHighScores[1].ToString("D8") + "</size>\n\n\n" +
            "<size=200><color=#00ff00ff>Track 3</color></size>\n<size=100>" + EasyHighScores[2].ToString("D8") + "\t" + AdvHighScores[2].ToString("D8") + "</size>");

        print(EasyHighScores[0]);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
