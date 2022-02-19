using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyController2D : CreatureController2D
{
    private const string ATTACK_RIGHT_ANIMATION_PARAMETER = "AttackRight";
    private const string ATTACK_LEFT_ANIMATION_PARAMETER = "AttackLeft";
    private static readonly int AttackRight = Animator.StringToHash(ATTACK_RIGHT_ANIMATION_PARAMETER);
    private static readonly int AttackLeft = Animator.StringToHash(ATTACK_LEFT_ANIMATION_PARAMETER);
    [SerializeField] private PlayerController2D player;
    [SerializeField] private LayerMask playerLayerMask;

    private void OnEnable()
    {
        musicManager.OnBeat += ExecuteMove;
    }

    private void OnDisable()
    {
        musicManager.OnBeat -= ExecuteMove;
    }

    protected override void HandleUpdate()
    {
    }

    protected override void ExecuteMove()
    {
        var position = transform.position;
        var left = Physics2D.Linecast(position, position + Vector3.left, playerLayerMask);
        if (left)
        {
            player.Die();
            animator.SetTrigger(AttackLeft);
            return;
        }
        var right = Physics2D.Linecast(position, position + Vector3.right, playerLayerMask);
        if (right)
        {
            player.Die();
            animator.SetTrigger(AttackRight);
            return;
        }
        var directions = Enum.GetValues(typeof(Direction));
        selectedDirection = (Direction) directions.GetValue(Random.Range(0, directions.Length));
        base.ExecuteMove();
    }
}
