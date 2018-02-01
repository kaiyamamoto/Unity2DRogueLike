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

    public abstract void Generate(List<Layer2D> layer);

    /// <summary>
    /// 何も存在しない位置のインデックスリスト作成
    /// </summary>
    /// <param name="list">現在のLayerデータ</param>
    /// <returns>生成可能位置のインデックスリスト</returns>
    protected List<KeyValuePair<int, int>> GetGeneratedList(Layer2D layer)
    {
        var gList = new List<KeyValuePair<int, int>>();

        for (int j = 0; j < layer.Height; j++)
        {
            for (int i = 0; i < layer.Width; i++)
            {
                if (layer.Get(i, j) == (int)ChipType.Road)
                {
                    gList.Add(new KeyValuePair<int, int>(i, j));
                }
            }
        }
        return gList;
    }
}
