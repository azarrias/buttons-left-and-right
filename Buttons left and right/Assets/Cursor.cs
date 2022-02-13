using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    [SerializeField] private GameObject highlight;
    public PlayerController2D.Direction direction;

    public void EnableHighlight(bool enable)
    {
        highlight.SetActive(enable);
    }
}
