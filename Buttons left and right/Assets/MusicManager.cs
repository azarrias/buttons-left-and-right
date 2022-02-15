using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    // Music stuff, doesn't belong here
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
        beatPeriod = 60 / bpm;
        StartMusic();
    }
    
    private void Update()
    {
        if (audioSource.time > beatPeriod * currentBeat + startingOffset)
        {
            currentBeat++;
        }

        double THRESHOLD = 0.1f;
        if (audioSource.time > beatPeriod * currentBeat + startingOffset - THRESHOLD &&
            audioSource.time < beatPeriod * currentBeat + startingOffset + THRESHOLD)
        {
            dotSpriteRenderer.color = Color.red;
        }
        else
        {
            dotSpriteRenderer.color = Color.white;
        }
    }

    public float GetBeatProgressNormalized()
    {
        var progress = beatPeriod * (currentBeat + 1) + startingOffset - audioSource.time;
        return progress / beatPeriod;
    }

    public void DistanceToClosestBeat()
    {
        var distanceToPreviousBeat = Mathf.Abs(beatPeriod * (currentBeat - 1) + startingOffset - audioSource.time);
        var distanceToCurrentBeat = Mathf.Abs(beatPeriod * (currentBeat) + startingOffset - audioSource.time);
        Debug.Log("distanceToPreviousBeat " + distanceToPreviousBeat + " | distanceToCurrentBeat " + distanceToCurrentBeat);
    }
    
    private void StartMusic()
    {
        Debug.Log("Start music");
        //musicPlaying = true;
        audioSource.Play();
    }
}
