using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class PlayerController2D : MonoBehaviour
{
    [SerializeField] private LayerMask obstaclesLayerMask;
    private Direction selectedDirection;
    private readonly Stack<Direction> movementStack = new Stack<Direction>();
    private bool undoLastMovement;

    public delegate void SelectDirection(Direction direction);

    public event SelectDirection OnSelectDirection;

    // Music stuff, doesn't belong here
    private bool musicPlaying;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float musicStartingOffset;
    [SerializeField] private float musicBpm;
    private float movingRate;
    private int currentBeat;

    public enum Direction
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
        OnSelectDirection?.Invoke(selectedDirection);
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
            else if (undoLastMovement)
            {
                TryUndo();
            }

            currentBeat++;
        }
    }

    private Direction GetOppositeDirection(Direction direction) => direction switch
    {
        Direction.Right => Direction.Left,
        Direction.Down => Direction.Up,
        Direction.Left => Direction.Right,
        Direction.Up => Direction.Down,
        _ => throw new ArgumentOutOfRangeException(nameof(direction), $"Not expected direction value: {direction}")
    };

    private void TryMove()
    {
        var currentPosition = transform.position;
        var targetMovement = GetTargetMovement(selectedDirection);
        var obstacles = Physics2D.Linecast(currentPosition, currentPosition + targetMovement, obstaclesLayerMask);
        if (!obstacles)
        {
            Move(targetMovement);
        }
    }

    private void Move(Vector3 movement)
    {
        movementStack.Push(selectedDirection);
        StartCoroutine(SmoothMove(movement));
    }
    
    private void TryUndo()
    {
        if (movementStack.Count == 0)
        {
            return;
        }

        var currentPosition = transform.position;
        var direction = movementStack.Pop();
        var oppositeDirection = GetOppositeDirection(direction);
        var targetMovement = GetTargetMovement(oppositeDirection);
        var obstacles = Physics2D.Linecast(currentPosition, currentPosition + targetMovement, obstaclesLayerMask);
        if (!obstacles)
        {
            Undo(targetMovement);
        }
    }

    private void Undo(Vector3 movement)
    {
        StartCoroutine(SmoothMove(movement));
    }

    IEnumerator SmoothMove(Vector3 targetMovement)
    {
        // TODO - Replace magic number with the duration of the moving animation
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
    
    private Vector3 GetTargetMovement(Direction direction) => direction switch
    {
        Direction.Up => Vector3.up,
        Direction.Left => Vector3.left,
        Direction.Down => Vector3.down,
        Direction.Right => Vector3.right,
        _ => throw new ArgumentOutOfRangeException(nameof(direction), $"Not expected direction value: {direction}"),
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

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SetSelectedDirection(Direction.None);
            undoLastMovement = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            undoLastMovement = false;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SetSelectedDirection(GetNextAvailableDirection());
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
