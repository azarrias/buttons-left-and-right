using UnityEngine;

public class EnemyController2D : CreatureController2D
{
    [SerializeField] private PlayerController2D player;
    
    protected override void HandleUpdate()
    {
        PrepareMove();
    }

    private void PrepareMove()
    {
        
    }
}
