using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hud : MonoBehaviour
{
    [SerializeField] private PlayerController2D player;
    [SerializeField] private Cursor[] cursors;
    
    private void OnEnable()
    {
        player.OnSelectDirection += ToggleCursors;
    }

    private void OnDisable()
    {
        player.OnSelectDirection -= ToggleCursors;
    }

    private void ToggleCursors(PlayerController2D.Direction direction)
    {
        foreach (var cursor in cursors)
        {
            cursor.EnableHighlight(cursor.direction == direction);
        }
    }
}
