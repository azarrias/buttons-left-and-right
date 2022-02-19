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

    private float lastBeatSample;
    private float nextBeatSample;
    private float samplePeriod;
    private float sampleOffset;
    private float currentSample;
    private int sampleRate;

    // TODO - For debugging
    [SerializeField] private Image dotImage;

    private void Awake()
    {
#if UNITY_ANDROID || UNITY_STANDALONE_WIN
        startingOffset += 0.10f;
#endif
        beatPeriod = 60f / bpm;
        sampleRate = AudioSettings.outputSampleRate;
        samplePeriod = beatPeriod * sampleRate;
        sampleOffset = startingOffset * sampleRate;
        ScheduleMusic();
        lastBeatSample = 0f;
        nextBeatSample = (float)musicStartTimestamp * sampleRate;
    }
    
    private void Update()
    {
        if (audioSource.isPlaying)
        {
            currentSample = (float)AudioSettings.dspTime * sampleRate;

            if (currentSample >= nextBeatSample + sampleOffset)
            {
                StartCoroutine(PaintDebugCircle(0.2f));
                lastBeatSample = nextBeatSample;
                nextBeatSample += samplePeriod;
            }
        }
    }

    IEnumerator PaintDebugCircle(float duration)
    {
        dotImage.color = Color.yellow;
        yield return new WaitForSeconds(duration);
        dotImage.color = Color.white;
    }

    public float GetDistanceToClosestBeat()
    {
        var distanceToPreviousBeat = Mathf.Abs(lastBeatSample + sampleOffset - currentSample);
        var distanceToCurrentBeat = Mathf.Abs(nextBeatSample + sampleOffset - currentSample);
        return Math.Min(distanceToCurrentBeat, distanceToPreviousBeat);
    }

    public float GetDistanceToClosestBeatNormalized()
    {
        return GetDistanceToClosestBeat() / samplePeriod;
    }
    
    private void ScheduleMusic()
    {
        Debug.Log("Start music");
        musicStartTimestamp = AudioSettings.dspTime + 2.0f;
        audioSource.PlayScheduled(musicStartTimestamp);
    }
}
