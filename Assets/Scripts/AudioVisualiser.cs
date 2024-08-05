using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioVisualiser : MonoBehaviour
{
    [SerializeField] AudioSource mainTrack;
    public List<GameObject> audioCubes = new List<GameObject>();
    [Range(0, 50)]
    public float radius;
    public float radians, vertical, horizontal;
    public Transform rotCentre;
    public GameObject cubePrefab;
    public GameObject samplerCube;
    public float[] samples;
    public float averageAmp = 0f;
    public float scaleValue;
    public float scaleMult;
    public float heightMult = 15f;
    public float maxHeight = 10f;
    public Vector3 initialScale, spawnPos;

    private void Start()
    {
        samples = new float[256];
        InitializeAudioCubes();
        CreateAudioPattern();
    }

    private void Update()
    {
        mainTrack.GetSpectrumData(samples, 0, FFTWindow.Rectangular);

        foreach (GameObject cube in audioCubes)
        {
            cube.transform.LookAt(rotCentre, cube.transform.forward);
        }

        if (samples == null || samples.Length == 0)
        {
            return;
        }
        for (int i = 0; i < samples.Length; i++)
        {
            if (i < samples.Length)
            {
                float newHeight = Mathf.Clamp(samples[i] * heightMult, 0f, maxHeight);
                audioCubes[i].transform.localScale = new Vector3(audioCubes[i].transform.localScale.x, audioCubes[i].transform.localScale.y , newHeight);
            }
            else
            {
                audioCubes[i].transform.localScale = new Vector3(audioCubes[i].transform.localScale.z, audioCubes[i].transform.localScale.x, 0f);

            }
        }


    }

    void InitializeAudioCubes()
    {
        foreach (GameObject cubes in audioCubes)
        {
            if (cubes != null)
            {
                cubes.transform.localScale = new Vector3(cubes.transform.localScale.x, cubes.transform.localScale.y, 0f);

            }
        }
    }

    public void ReceiveAudioClip(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.Log("No clips brah");

        }

        int numSamples = clip.samples;
        samples = new float[numSamples];
        clip.GetData(samples, 0);
    }

    void CreateAudioPattern()
    {
        for (int i = 0; i < 128; i++)
        {
            radians = 2 * Mathf.PI / 128 * i;

            vertical = Mathf.Sin(radians);
            horizontal = Mathf.Cos(radians);
            Vector3 spawnDir = new Vector3(horizontal, 0f, vertical);
            Quaternion spawnRot = new Quaternion(-90f, 0f, 0f, 0f);
            spawnPos = rotCentre.transform.position + spawnDir * radius;

            samplerCube = Instantiate(cubePrefab, spawnPos, Quaternion.identity, rotCentre);
            audioCubes.Add(samplerCube);
        }

    }
}
