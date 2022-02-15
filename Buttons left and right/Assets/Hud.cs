using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hud : MonoBehaviour
{
    [SerializeField] private PlayerController2D player;
    [SerializeField] private MusicManager musicManager;
    [SerializeField] private Arrow[] cursors;
    [SerializeField] private Circle tempoCircle;
    
    private void OnEnable()
    {
        player.OnSelectDirection += ToggleCursors;
        //player.OnSelectUndo += ToggleRewind;
    }

    private void OnDisable()
    {
        player.OnSelectDirection -= ToggleCursors;
        //player.OnSelectUndo -= ToggleRewind;
    }

    private void ToggleCursors(PlayerController2D.Direction direction)
    {
        foreach (var cursor in cursors)
        {
            cursor.EnableHighlight(cursor.direction == direction);
        }
    }
    
    private void HighlightCircle(bool enable)
    {
        // TODO highlight circle, in green if OK, in red if KO
    }

    private void Update()
    {

    }
}
