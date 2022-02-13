using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

public class PlayerController2D : MonoBehaviour
{
    [SerializeField] private LayerMask obstaclesLayerMask;
    private Direction selectedDirection;
    private bool undoLastMovement;
    
    // Music stuff, doesn't belong here
    private bool musicPlaying;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float musicStartingOffset;
    [SerializeField] private float musicBpm;
    private float movingRate;
    private int currentBeat;
    
    // Just for debugging
    [SerializeField] private Text selectedDirectionText;

    private enum Direction
    {
        None,
        Right,
        Down,
        Left,
        Up
    }

    private void Awake()
    {
        movingRate = 60 / musicBpm;
    }

    private void SetSelectedDirection(Direction direction)
    {
        selectedDirection = direction;
        selectedDirectionText.text = "Selected direction: " + selectedDirection;
    }
    
    private void Update()
    {
        HandleInput();
        
        if (audioSource.time > movingRate * currentBeat + musicStartingOffset)
        {
            if (selectedDirection != Direction.None)
            {
                TryMove();
            }
            currentBeat++;
        }
    }

    private void TryMove()
    {
        var currentPosition = transform.position;
        var targetMovement = GetTargetMovement();
        var obstacles = Physics2D.Linecast(currentPosition, currentPosition + targetMovement, obstaclesLayerMask);
        if (!obstacles)
        {
            Move(targetMovement);
        }
    }

    private void Move(Vector3 targetMovement)
    {
        StartCoroutine(SmoothMove(targetMovement));
    }

    IEnumerator SmoothMove(Vector3 targetMovement)
    {
        var duration = 0.1f;
        var startTime = Time.time;
        var startPos = transform.position;
        var interpolationPoint = 0f;
        while (interpolationPoint < 1f)
        {
            interpolationPoint = (Time.time - startTime) / duration;
            var move = Mathf.Lerp(0, 1, interpolationPoint);
            transform.position = startPos + move * targetMovement;
            yield return null;
        }
    }

    private Direction GetNextAvailableDirection() => selectedDirection switch
    {
        Direction.None => Direction.Right,
        Direction.Right => Direction.Down,
        Direction.Down => Direction.Left,
        Direction.Left => Direction.Up,
        Direction.Up => Direction.Right,
        _ => throw new ArgumentOutOfRangeException(nameof(selectedDirection), $"Not expected direction value: {selectedDirection}"),
    };
    
    private Vector3 GetTargetMovement() => selectedDirection switch
    {
        Direction.Up => Vector3.up,
        Direction.Left => Vector3.left,
        Direction.Down => Vector3.down,
        Direction.Right => Vector3.right,
        _ => throw new ArgumentOutOfRangeException(nameof(selectedDirection), $"Not expected direction value: {selectedDirection}"),
    };

    private void HandleInput()
    {
        if (!musicPlaying)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartMusic();
            }
            else
            {
                return;
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SetSelectedDirection(GetNextAvailableDirection());
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (selectedDirection == Direction.None)
            {
                // UNDO last movement
            }
            else
            {
                SetSelectedDirection(Direction.None);
            }
        }
    }

    private void StartMusic()
    {
        Debug.Log("Start music");
        musicPlaying = true;
        audioSource.Play();
        SetSelectedDirection(Direction.None);
    }
}
