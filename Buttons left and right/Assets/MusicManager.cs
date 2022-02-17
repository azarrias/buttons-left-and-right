using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    private bool musicPlaying;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float startingOffset;
    [SerializeField] private float bpm;
    public AudioSource AudioSource => audioSource;
    public float Bpm => bpm;
    private int currentBeat;
    private float beatPeriod;
    public float BeatPeriod => beatPeriod;
    private double musicStartTime;

    // TODO - For debugging
    [SerializeField] private Image dotImage;

    private void Awake()
    {
        beatPeriod = 60f / bpm;
        //StartMusic();
        musicStartTime = AudioSettings.dspTime + 2.0f;
    }
    
    private void Update()
    {
        if (!musicPlaying)
        {
            var time = AudioSettings.dspTime;
            if (time + 1.0f > musicStartTime)
            {
                audioSource.PlayScheduled(musicStartTime);
                musicPlaying = true;
            }
        }
        
        if (AudioSettings.dspTime - musicStartTime > beatPeriod * currentBeat + startingOffset)
        {
            currentBeat++;
        }
        
        double THRESHOLD = 0.1f;
        var distance = GetDistanceToClosestBeatNormalized();
        if (distance < THRESHOLD)
        {
            dotImage.color = Color.red;
        }
        else
        {
            dotImage.color = Color.white;
        }
    }

    public float GetDistanceToClosestBeat()
    {
        var distanceToPreviousBeat = Mathf.Abs(beatPeriod * (currentBeat - 1) + startingOffset - audioSource.time);
        var distanceToCurrentBeat = Mathf.Abs(beatPeriod * (currentBeat) + startingOffset - audioSource.time);
        return Math.Min(distanceToCurrentBeat, distanceToPreviousBeat);
    }

    public float GetDistanceToClosestBeatNormalized()
    {
        return GetDistanceToClosestBeat() / beatPeriod;
    }
    
    private void StartMusic()
    {
        Debug.Log("Start music");
        audioSource.Play();
    }
}
