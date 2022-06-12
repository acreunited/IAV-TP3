using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mic : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip audioClip;
    public bool useMicrophone;
    public string deviceName;
    public Transform player1;
    public Transform player2;
    Transform currentPlayer;
    int counter = 0;
    float timeLeftPlay = 3;
    float timeLeftToPrepare = 5;
    bool timeToPlay = false;
    bool timeToPrepare = true;
    public Text text;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        ///currentPlayer = player2;
        if (useMicrophone)
        {
            if (Microphone.devices.Length > 0)
            {
                deviceName = Microphone.devices[0];
                audioSource.clip = Microphone.Start(deviceName, true, 10, AudioSettings.outputSampleRate);
                print(AudioSettings.outputSampleRate);
                while (Microphone.GetPosition(deviceName) < (AudioSettings.outputSampleRate / 1000) * 30) ;

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

        if (timeToPlay)
        {
            if (timeLeftPlay > 0)
            {
                timeLeftPlay -= Time.deltaTime;
                UpdateTimePlay(timeLeftPlay);
                if (AudioAnalysis.ConvertToDB(energy) > 40)
                {
                    float peakFrequency = AudioAnalysis.ComputeSpectrumPeak(audioSource, true);
                    // Debug.Log(peakFrequency);
                    float concentration = AudioAnalysis.ConcentrationAroundPeak(peakFrequency);
                    //Debug.Log(concentration);
                    if (concentration > 0.8f)
                    {
                        currentPlayer.position += new Vector3(concentration, 0, 0) * Time.deltaTime;
                    }

                }

            }
            else
            {
                Debug.Log("Time is up");
                timeToPlay = false;
                timeToPrepare = true;
                timeLeftPlay = 3;
            }
        }
        if (timeToPrepare)
        {
            if (timeLeftToPrepare > 0)
            {
                timeLeftToPrepare -= Time.deltaTime;
                UpdateTimePrepare(timeLeftToPrepare);
            }
            else
            {
                ChangePlayer();
                timeToPrepare = false;
                timeToPlay = true;
                timeLeftToPrepare = 5;
            }
        }

    }

    private void ChangePlayer()
    {
        if (currentPlayer == player1)
        {
            currentPlayer = player2;
            currentPlayer.name = "Player 2";
        }
        else if (currentPlayer == player2)
        {
            currentPlayer = player1;
            currentPlayer.name = "Player 1";
        }
        else if (currentPlayer == null)
        {
            currentPlayer = player1;
            currentPlayer.name = "Player 1";
        }

    }
    void UpdateTimePlay(float currentTime)
    {
        currentTime += 1;
        float seconds = Mathf.FloorToInt(currentTime % 60);
        text.text = "" + seconds;

    }

    void UpdateTimePrepare(float currentTime)
    {
        currentTime += 1;
        float seconds = Mathf.FloorToInt(currentTime % 60);
        text.text = "Get Ready" + " " + seconds;

    }

    public void SetTimeToPlay(bool timePlay)
    {
        timeToPlay = timePlay;
    }

    public void SetTimeToPrepare(bool timePrepare)
    {
        timeToPrepare = timePrepare;
    }
    public Transform getCurrentPlayer()
    {
        return currentPlayer;
    }
}






