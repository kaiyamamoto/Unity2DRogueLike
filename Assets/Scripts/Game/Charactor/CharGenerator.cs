using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharGenerator : Generator {

    [SerializeField]
    private Camera _camera = null;

    [SerializeField, Tooltip("出現的数")]
    private int _enemyNum = 0;

    public override Layer2D Generate(Layer2D layer)
    {
        // データ取得
        WIDTH = layer.Width;
        HEIGHT = layer.Height;

        // 初期化
        _layer = new Layer2D();
        _layer.Fill((int)ChipType.None);

        // 生成可能位置リストを取得
        var popList = new List<KeyValuePair<int, int>>();
        popList = GetGeneratedList(layer);

        // 生成可能位置にプレイヤーを作成

        // プレイヤー生成
        int rand = Random.Range(0, popList.Count - 1);

        var token = Util.CreatePlayer(0.0f, 0.0f, "Sprites\\Tile\\player", "", "Player");
        token.Renderer.sortingOrder = 1;
        var player = token.gameObject.AddComponent<Player>();
        player.SetChipPosition(popList[rand].Key, popList[rand].Value);
        _camera.gameObject.transform.parent = token.gameObject.transform;
        _camera.transform.localPosition = new Vector3(0.0f, 0.0f, -10.0f);

        // 生成可能位置に敵を作成

        // デバッグ出力
        Debug.Log(player.transform.position);
//        _layer.Dump();

        // 情報の追加されたデータを送る
        return layer;
    }

    /// <summary>
    /// 生成可能位置のインデックスリスト作成
    /// </summary>
    /// <param name="list">現在のLayerデータ</param>
    /// <returns>生成可能位置のインデックスリスト</returns>
    private List<KeyValuePair<int, int>> GetGeneratedList(Layer2D layer)
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
