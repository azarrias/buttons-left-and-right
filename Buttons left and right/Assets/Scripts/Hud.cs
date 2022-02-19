using System;
using UnityEngine;

public class Hud : MonoBehaviour
{
    [SerializeField] private PlayerController2D player;
    [SerializeField] private MusicManager musicManager;
    [SerializeField] private Arrow[] cursors;
    [SerializeField] private Circle tempoCircle;

    private const float TEMPO_CIRCLE_HIGHLIGHT_DURATION = 0.3f;
    
    private void OnEnable()
    {
        player.OnSelectDirection += ToggleCursors;
        player.OnExecuteMove += HighlightTempoCircle;
    }

    private void OnDisable()
    {
        player.OnSelectDirection -= ToggleCursors;
        player.OnExecuteMove -= HighlightTempoCircle;
    }

    private void ToggleCursors(PlayerController2D.Direction direction)
    {
        foreach (var cursor in cursors)
        {
            cursor.EnableHighlight(cursor.direction == direction);
        }
    }
    
    private void HighlightTempoCircle(PlayerController2D.MoveQuality moveQuality)
    {
        var highlightColor = GetTempoCircleHighlightColor(moveQuality);
        tempoCircle.Highlight(highlightColor, TEMPO_CIRCLE_HIGHLIGHT_DURATION);
    }

    private Color GetTempoCircleHighlightColor(PlayerController2D.MoveQuality moveQuality) => moveQuality switch
    {
        PlayerController2D.MoveQuality.Ace => Color.magenta,
        PlayerController2D.MoveQuality.Ok => Color.green,
        PlayerController2D.MoveQuality.Ko => Color.red,
        _ => throw new ArgumentOutOfRangeException(nameof(moveQuality), $"Not expected moveQuality value: {moveQuality}")
    };
}
