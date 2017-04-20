using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectrumToVisual : MonoBehaviour {

    public GameObject sampleTrianglePrefab;
    GameObject[] sampleTriangle = new GameObject[512];
    public float maxScale;
    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < 512; i++)
        {
            GameObject instanceSampleTriangle = (GameObject)Instantiate(sampleTrianglePrefab);
            instanceSampleTriangle.transform.position = this.transform.position;
            instanceSampleTriangle.transform.parent = this.transform;
            instanceSampleTriangle.name = "SampleTriangle" + i;
            this.transform.eulerAngles = new Vector3(0, -0.703125f * i, 0);
            instanceSampleTriangle.transform.position += Vector3.forward * 10;
            sampleTriangle[i] = instanceSampleTriangle;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 512; i++)
        {
            if (sampleTriangle != null)
            {
                sampleTriangle[i].transform.localScale = new Vector3(1.0f, (GetAudioSpectrum.samples[i] * maxScale) + 2, 1);
            }
        }
    }

    public void DestroySpectrumTriangles()
    {
        for (int i = 0; i < 512; i++)
        {
            Destroy(sampleTriangle[i]);
        }
    }
}
