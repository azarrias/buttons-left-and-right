using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyController2D : CreatureController2D
{
    [SerializeField] private PlayerController2D player;
    
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
        var directions = Enum.GetValues(typeof(Direction));
        selectedDirection = (Direction) directions.GetValue(Random.Range(0, directions.Length));
        base.ExecuteMove();
    }
}
