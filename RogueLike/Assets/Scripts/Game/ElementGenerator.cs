using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ElementGenerator : MonoBehaviour {

    /// <summary>
    /// マップ全体の幅
    /// </summary>
    [SerializeField, Tooltip("マップの幅")]
    private int WIDTH = 30;
    /// <summary>
    /// マップ全体の高さ
    /// </summary>
    [SerializeField, Tooltip("マップの高さ")]
    private int HEIGHT = 30;

    [SerializeField,Tooltip("ダンジョン生成クラス")]
    private DgGenerator _dgGenerator = null;

    [SerializeField, Tooltip("アイテム生成クラス")]
    private ItemGenerator _itemGenerator = null;

    [SerializeField, Tooltip("オブジェクト生成クラス")]
    private ObjectGenerator _objGenerator = null;

    [SerializeField, Tooltip("キャラクター生成クラス")]
    private CharGenerator _charGenerator = null;

    List<Layer2D> _layerList = null;

    void Start () {
        this.Generate();
	}

    public List<Layer2D> Generate()
    {
        _layerList = new List<Layer2D>();
        _layerList.Capacity = (int)LayerType.length;

        for (int i = 0; i < (int)LayerType.length; i++){
            _layerList.Add(new Layer2D(WIDTH, HEIGHT));
        }
 
        var topLayer = _layerList[(int)LayerType.Top];
        var underLayer = _layerList[(int)LayerType.Under];

        underLayer = _dgGenerator.Generate(underLayer);

        //_topLayer = _objGenerator.Generate(underLayer);
        //_topLayer = _itemGenerator.Generate(topLayer);
        topLayer = _charGenerator.Generate(underLayer);

        _layerList[(int)LayerType.Top] = topLayer;
        _layerList[(int)LayerType.Under] = underLayer;

        InGameManager.GetInstance().LayerList = _layerList;

        return _layerList;
    }
}
