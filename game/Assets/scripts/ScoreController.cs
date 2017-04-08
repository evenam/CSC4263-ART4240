using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour {

    public Text scoreText, multiplyerText, finalScoreText;
    private double score;
    private double multiplyer;


	// Use this for initialization
	void Start () {
        score = 0;
        multiplyer = 1;
        updateScore();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void incScore()
    {
        score += (multiplyer * 100); //score increases based off multiplyer when button hit
        multiplyer += 0.25; //multiplyer is increased after score is updated
        updateScore();
    }

    public void multiplyerReset()
    {
        multiplyer = 1;
        updateScore();
    }

    void updateScore()
    {
        scoreText.text = ((int)score).ToString("D8");
        multiplyerText.text = "x" + multiplyer.ToString("0.00");
        finalScoreText.text = "Final Score \n~" + scoreText.text + "~";
    }
}
