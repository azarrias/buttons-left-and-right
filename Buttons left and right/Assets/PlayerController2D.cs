using System;
using UnityEngine;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

public class PlayerController2D : MonoBehaviour
{
    [SerializeField] private LayerMask obstaclesLayerMask;
    private const float MOVING_RATE = 1f;
    private float timeLeftToMove;
    private Direction selectedDirection;
    private bool undoLastMovement;
    
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
        timeLeftToMove = MOVING_RATE;
        SetSelectedDirection(Direction.None);
    }

    private void SetSelectedDirection(Direction direction)
    {
        selectedDirection = direction;
        selectedDirectionText.text = "Selected direction: " + selectedDirection;
    }
    
    private void Update()
    {
        HandleInput();

        timeLeftToMove -= Time.deltaTime;
        if (timeLeftToMove < 0)
        {
            if (selectedDirection != Direction.None)
            {
                TryMove();
            }
            timeLeftToMove = MOVING_RATE;
        }
    }

    private void TryMove()
    {
        var currentPosition = transform.position;
        var targetMovement = GetTargetMovement();
        var obstacles = Physics2D.Linecast(currentPosition, currentPosition + targetMovement, obstaclesLayerMask);
        if (obstacles)
        {
            SetSelectedDirection(Direction.None);
        }
        else
        {
            transform.Translate(targetMovement);
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
}
