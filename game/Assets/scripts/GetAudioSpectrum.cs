using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetAudioSpectrum : MonoBehaviour {

    public AudioSource audioSource;
    public static float[] samples = new float[512];

    // Use this for initialization
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
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
