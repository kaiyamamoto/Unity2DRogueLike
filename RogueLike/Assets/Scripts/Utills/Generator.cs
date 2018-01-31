using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Generator : MonoBehaviour {

    /// <summary>
    /// マップ全体の幅
    /// </summary>
    protected int WIDTH = 0;
    /// <summary>
    /// マップ全体の高さ
    /// </summary>
    protected int HEIGHT = 0;

    /// <summary>
    /// 2次元配列情報
    /// </summary>
    protected Layer2D _layer = null;
    public Layer2D Layer
    {
        get { return _layer; }
    }

    public abstract Layer2D Generate(Layer2D layer);
}
