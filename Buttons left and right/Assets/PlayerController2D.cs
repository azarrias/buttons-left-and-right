using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vector3 = UnityEngine.Vector3;

public class PlayerController2D : MonoBehaviour
{
    [SerializeField] private LayerMask obstaclesLayerMask;
    [SerializeField] private LayerMask goalLayerMask;
    [SerializeField] private MusicManager musicManager;
    [SerializeField] private float greatThreshold;
    [SerializeField] private float okThreshold;
    private Direction selectedDirection;
    private readonly Stack<Direction> movementStack = new Stack<Direction>();
    private bool undoLastMovement;

    public delegate void SelectDirection(Direction direction);
    public delegate void SelectUndo(bool enable);

    public event SelectDirection OnSelectDirection;
    public event SelectUndo OnSelectUndo;

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
        SetSelectedDirection(Direction.None);
    }

    private void Update()
    {
#if UNITY_ANDROID || UNITY_IOS
        HandleInputMobile();
#else
        HandleInput();
#endif
    }

    private void SetSelectedDirection(Direction direction)
    {
        selectedDirection = direction;
        OnSelectDirection?.Invoke(selectedDirection);
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
        var distance = musicManager.GetDistanceToClosestBeatNormalized();
        if (distance < greatThreshold)
        {
            Debug.Log("GREAT!!!");
        }
        else if (distance < okThreshold)
        {
            Debug.Log("OK");
        }
        else
        {
            Debug.Log("KO");
            return;
        }

        if (selectedDirection == Direction.None)
        {
            return;
        }
        
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
        var goal = Physics2D.Linecast(transform.position, transform.position, goalLayerMask);
        if (goal)
        {
            SceneManager.LoadScene(1);
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
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            TryMove();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SetSelectedDirection(GetNextAvailableDirection());
        }
    }

    private void HandleInputMobile()
    {
        if (Input.touches.Any(t => t.phase == TouchPhase.Began && t.position.x < Screen.width / 2f))
        {
            TryMove();
        }
        else if (Input.touches.Any(t => t.phase == TouchPhase.Began && t.position.x >= Screen.width / 2f))
        {
            SetSelectedDirection(GetNextAvailableDirection());
        }
    }
}
