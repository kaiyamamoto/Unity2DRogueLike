using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 方向
/// </summary>
[Flags]
public enum Direction
{

    None = 0,
    Up = 1 << 0,
    Down = 1 << 1,
    Left = 1 << 2,
    Right = 1 << 3
}

/// <summary>
/// チップの種類
/// </summary>
public enum ChipType
{
    None = -1,
    Road,
    Wall,
    length
}

public enum LayerType
{
    Top = 0,
    Under,
    length
}
