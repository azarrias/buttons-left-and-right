using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    
    // TODO - For debugging
    [SerializeField] private SpriteRenderer dotSpriteRenderer;

    private void Awake()
    {
        beatPeriod = 60f / bpm;
        StartMusic();
    }
    
    private void Update()
    {
        if (audioSource.time > beatPeriod * currentBeat + startingOffset)
        {
            currentBeat++;
        }
        
        double THRESHOLD = 0.1f;
        var distance = GetDistanceToClosestBeatNormalized();
        if (distance < THRESHOLD)
        {
            dotSpriteRenderer.color = Color.red;
        }
        else
        {
            dotSpriteRenderer.color = Color.white;
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
