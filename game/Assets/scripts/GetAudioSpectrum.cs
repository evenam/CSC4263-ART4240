using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetAudioSpectrum : MonoBehaviour {

    AudioSource audioSource;
    public AudioClip clip1, clip2, clip3;
    public static float[] samples = new float[512];

    // Use this for initialization
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (PlayerPrefs.GetString("songPath").Contains("example_song"))
            audioSource.clip = clip1;
        else if (PlayerPrefs.GetString("songPath").Contains("badsong"))
            audioSource.clip = clip2;
        else if (PlayerPrefs.GetString("songPath").Contains("vaporwave"))
            audioSource.clip = clip3;
        else
            print("No song selected.");

        audioSource.PlayDelayed(3);
    }

    // Update is called once per frame
    void Update()
    {
        GetSpectrumAudioSource();
    }

    void GetSpectrumAudioSource()
    {
        audioSource.GetSpectrumData(samples, 0, FFTWindow.Blackman);
    }
}
