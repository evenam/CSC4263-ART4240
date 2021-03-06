﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{

    public Text scoreText, multiplyerText, finalScoreText;
    private static double score;
    private double multiplyer;

    int highscore;
    static string highscoreKey;

    void Start()
    {
        if (PlayerPrefs.GetInt("songDifficulty") == 1)
            highscoreKey = "e_HIGH_SCORE_";
        else
            highscoreKey = "a_HIGH_SCORE_";

        if (PlayerPrefs.GetString("songPath").Contains("example_song"))
            highscoreKey += "1";
        else if (PlayerPrefs.GetString("songPath").Contains("badsong"))
            highscoreKey += "2";
        else if (PlayerPrefs.GetString("songPath").Contains("vaporwave"))
            highscoreKey += "3";

        // highscoreKey = (SongSelect.difficulty + "_HIGH_SCORE_" + SongSelect.songNumber)
        highscore = PlayerPrefs.GetInt(highscoreKey);

        score = 0;
        multiplyer = 1;
        UpdateScore();
    }

    // Update is called once per frame
    //void Update () {

    //}

    public void IncScore()
    {
        score += (multiplyer * 100); //score increases based off multiplyer when button hit
        multiplyer += 0.25; //multiplyer is increased after score is updated
        UpdateScore();
    }

    public void MultiplyerReset()
    {
        multiplyer = 1;
        UpdateScore();
    }

    void UpdateScore()
    {
        scoreText.text = ((int)score).ToString("D8");
        multiplyerText.text = "x" + multiplyer.ToString("0.00");
        finalScoreText.text = "<size=135>FINAL SCORE</size>\n~--~~--~--~--~\n~ <size=180>" + scoreText.text + "</size> ~\n~--~~--~--~--~";
    }

    public void SongOver()
    {
        if (score > highscore)
            PlayerPrefs.SetInt(highscoreKey, (int)score);
    }
}
