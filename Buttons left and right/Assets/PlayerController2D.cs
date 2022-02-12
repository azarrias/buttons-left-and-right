using System;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

public class PlayerController2D : MonoBehaviour
{
    [SerializeField] private LayerMask obstaclesLayerMask;
    private const float MOVING_RATE = 1f;
    private const float CHANGE_DIRECTION_RATE = MOVING_RATE / 4;
    private float timeLeftToMove;
    private float timeLeftToChangeDirection;
    private bool isInputAllowed;
    private Direction availableDirection;
    private Direction selectedDirection;
    
    // Just for debugging
    [SerializeField] private Text availableDirectionText;
    [SerializeField] private Text selectedDirectionText;

    private enum Direction
    {
        Up,
        Left,
        Down,
        Right,
        None
    }

    private void Awake()
    {
        isInputAllowed = true;
        timeLeftToMove = MOVING_RATE;
        SetAvailableDirection(Direction.Up);
        SetSelectedDirection(Direction.None);
    }

    private void SetAvailableDirection(Direction direction)
    {
        availableDirection = direction;
        timeLeftToChangeDirection = CHANGE_DIRECTION_RATE;
        availableDirectionText.text = "Available direction: " + availableDirection;
    }

    private void SetSelectedDirection(Direction direction)
    {
        selectedDirection = direction;
        selectedDirectionText.text = "Selected direction: " + selectedDirection;
    }
    
    private void Update()
    {
        if (isInputAllowed)
        {
            HandleInput();
        }

        timeLeftToChangeDirection -= Time.deltaTime;
        if (timeLeftToChangeDirection < 0)
        {
            SetAvailableDirection(GetNextAvailableDirection());
        }

        timeLeftToMove -= Time.deltaTime;
        if (timeLeftToMove < 0)
        {
            if (selectedDirection != Direction.None)
            {
                Move();
            }
            timeLeftToMove = MOVING_RATE;
            selectedDirection = Direction.None;
            isInputAllowed = true;
        }
    }

    private void Move()
    {
        var currentPosition = transform.position;
        var targetMovement = GetTargetMovement();
        var obstacles = Physics2D.Linecast(currentPosition, currentPosition + targetMovement, obstaclesLayerMask);
        if (!obstacles)
        {
            transform.Translate(targetMovement);
        }
    }

    private Direction GetNextAvailableDirection() => availableDirection switch
    {
        Direction.Up => Direction.Left,
        Direction.Left => Direction.Down,
        Direction.Down => Direction.Right,
        Direction.Right => Direction.Up,
        _ => throw new ArgumentOutOfRangeException(nameof(availableDirection), $"Not expected direction value: {availableDirection}"),
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
            SetSelectedDirection(availableDirection);
            isInputAllowed = false;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // TODO - Undo last movement
        }
    }
}
