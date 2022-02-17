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
    private double musicStartTimestamp;
    private double musicElapsedTime;

    // TODO - For debugging
    [SerializeField] private Image dotImage;

    private void Awake()
    {
        beatPeriod = 60f / bpm;
        ScheduleMusic();
    }
    
    private void Update()
    {
        musicElapsedTime = AudioSettings.dspTime - musicStartTimestamp;
        if (musicElapsedTime <= 0)
        {
            return;
        }
        
        if (musicElapsedTime > beatPeriod * currentBeat + startingOffset)
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
        var distanceToPreviousBeat = Mathf.Abs(beatPeriod * (currentBeat - 1) + startingOffset - (float)musicElapsedTime);
        var distanceToCurrentBeat = Mathf.Abs(beatPeriod * (currentBeat) + startingOffset - (float)musicElapsedTime);
        return Math.Min(distanceToCurrentBeat, distanceToPreviousBeat);
    }

    public float GetDistanceToClosestBeatNormalized()
    {
        return GetDistanceToClosestBeat() / beatPeriod;
    }
    
    private void ScheduleMusic()
    {
        Debug.Log("Start music");
        musicStartTimestamp = AudioSettings.dspTime + 2.0f;
        audioSource.PlayScheduled(musicStartTimestamp);
    }
}
