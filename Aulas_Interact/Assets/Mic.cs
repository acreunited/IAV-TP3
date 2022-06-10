using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mic : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip audioClip;
    public bool useMicrophone;
    public string deviceName;
    public Transform agent;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (useMicrophone)
        {
            if (Microphone.devices.Length > 0)
            {
                deviceName = Microphone.devices[0];
                audioSource.clip = Microphone.Start(deviceName, true, 10, AudioSettings.outputSampleRate);
                print(AudioSettings.outputSampleRate);
                while(Microphone.GetPosition(deviceName)<(AudioSettings.outputSampleRate/1000)*30);
               
            }
            else
                useMicrophone = false;
        }


        if (!useMicrophone)
        {
            audioSource.clip = audioClip;
           
        }
        audioSource.loop = true;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        float energy = AudioAnalysis.MeanEnergy(audioSource);
        if (AudioAnalysis.ConvertToDB(energy) > 40)
        {
            float peakFrequency = AudioAnalysis.ComputeSpectrumPeak(audioSource, true);
            Debug.Log(peakFrequency);
            float concentration = AudioAnalysis.ConcentrationAroundPeak(peakFrequency);
            //Debug.Log(concentration);
            if (concentration > 0.8f)
            {
                transform.position += new Vector3(concentration, 0, 0) * Time.deltaTime*2f;
            }

        }
    }
}

