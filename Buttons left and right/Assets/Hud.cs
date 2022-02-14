using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hud : MonoBehaviour
{
    [SerializeField] private PlayerController2D player;
    [SerializeField] private Arrow[] cursors;
    [SerializeField] private Arrow[] rewindArrows;
    
    private void OnEnable()
    {
        player.OnSelectDirection += ToggleCursors;
        player.OnSelectUndo += ToggleRewind;
    }

    private void OnDisable()
    {
        player.OnSelectDirection -= ToggleCursors;
        player.OnSelectUndo -= ToggleRewind;
    }

    private void ToggleCursors(PlayerController2D.Direction direction)
    {
        foreach (var cursor in cursors)
        {
            cursor.EnableHighlight(cursor.direction == direction);
        }
    }
    
    private void ToggleRewind(bool enable)
    {
        foreach (var rewindArrow in rewindArrows)
        {
            rewindArrow.EnableHighlight(enable);
        }
    }
}
