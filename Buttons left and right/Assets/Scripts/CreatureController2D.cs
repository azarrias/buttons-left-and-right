using System;
using System.Collections;
using UnityEngine;

public abstract class CreatureController2D : MonoBehaviour
{
    private const string MOVE_UP_ANIMATION_PARAMETER = "MoveUp";
    private const string MOVE_RIGHT_ANIMATION_PARAMETER = "MoveRight";
    private const string MOVE_DOWN_ANIMATION_PARAMETER = "MoveDown";
    private const string MOVE_LEFT_ANIMATION_PARAMETER = "MoveLeft";
    [SerializeField] private LayerMask obstaclesLayerMask;
    [SerializeField] protected MusicManager musicManager;
    [SerializeField] protected Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject invisibleWall;
    protected Direction selectedDirection;

    public enum Direction
    {
        None,
        Right,
        Down,
        Left,
        Up
    }

    public enum MoveQuality
    {
        Ace,
        Ok,
        Ko
    }

    private void Awake()
    {
        SetSelectedDirection(Direction.None);
    }

    private void Update()
    {
        HandleUpdate();
    }

    protected abstract void HandleUpdate();

    protected virtual void SetSelectedDirection(Direction direction)
    {
        selectedDirection = direction;
    }

    private Direction GetOppositeDirection(Direction direction) => direction switch
    {
        Direction.Right => Direction.Left,
        Direction.Down => Direction.Up,
        Direction.Left => Direction.Right,
        Direction.Up => Direction.Down,
        _ => throw new ArgumentOutOfRangeException(nameof(direction), $"Not expected direction value: {direction}")
    };

    protected virtual void ExecuteMove()
    {
        if (selectedDirection == Direction.None)
        {
            return;
        }
        
        var currentPosition = transform.position;
        var targetMovement = GetTargetMovement(selectedDirection);
        var obstacles = Physics2D.Linecast(currentPosition, currentPosition + targetMovement, obstaclesLayerMask);
        if (!obstacles)
        {
            invisibleWall.transform.localPosition = targetMovement;
            AdjustOrientation();
            StartCoroutine(Move(targetMovement));
        }
    }

    private void AdjustOrientation()
    {
        animator.SetTrigger(GetAnimationTrigger());
    }

    private string GetAnimationTrigger() => selectedDirection switch
    {
        Direction.Up => MOVE_UP_ANIMATION_PARAMETER,
        Direction.Right => MOVE_RIGHT_ANIMATION_PARAMETER,
        Direction.Down => MOVE_DOWN_ANIMATION_PARAMETER,
        Direction.Left => MOVE_LEFT_ANIMATION_PARAMETER,
        _ => throw new ArgumentOutOfRangeException(nameof(selectedDirection), $"Not expected direction value: {selectedDirection}")
    };

    protected virtual IEnumerator Move(Vector3 targetMovement)
    {
        // TODO - Replace magic number with the duration of the moving animation
        var duration = 0.25f;
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
        invisibleWall.transform.localPosition = Vector3.zero;
    }

    protected Direction GetNextAvailableDirection() => selectedDirection switch
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
}
