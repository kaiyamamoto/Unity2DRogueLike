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
	}

    public List<Layer2D> Generate()
    {
        _layerList = new List<Layer2D>();
        _layerList.Capacity = (int)LayerType.length;

        for (int i = 0; i < (int)LayerType.length; i++){
            _layerList.Add(new Layer2D(WIDTH, HEIGHT));
        }

        _dgGenerator.Generate(_layerList);

        _layerList[(int)LayerType.Top].Fill((int)ChipType.None);

        _objGenerator.Generate(_layerList);
        //_itemGenerator.Generate(topLayer);
        _charGenerator.Generate(_layerList);

        InGameManager.GetInstance().LayerList = _layerList;

        return _layerList;
    }
}
