using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharGenerator : Generator {

    [SerializeField]
    private Camera _camera = null;

    [SerializeField, Tooltip("出現的数")]
    private int _enemyNum = 0;

    public override void Generate(List<Layer2D> layer)
    {
        // データ取得
        WIDTH = layer[(int)LayerType.Under].Width;
        HEIGHT = layer[(int)LayerType.Under].Height;

        // 生成可能位置リストを取得
        var popList = new List<KeyValuePair<int, int>>();
        popList = GetGeneratedList(layer[(int)LayerType.Under]);

        // 生成可能位置にプレイヤーを作成
        // プレイヤー生成
        int rand = Random.Range(0, popList.Count - 1);

        var token = Util.CreatePlayer(0.0f, 0.0f, "Sprites\\Tile\\player", "", "Player");
        token.Renderer.sortingOrder = 1;
        var player = token.gameObject.AddComponent<Player>();
        var x = popList[rand].Key;
        var y = popList[rand].Value;
        layer[(int)LayerType.Top].Set(x, y, (int)ChipType.Player);
        player.SetChipPosition(x, y);
        _camera.gameObject.transform.parent = token.gameObject.transform;
        _camera.transform.localPosition = new Vector3(0.0f, 0.0f, -10.0f);

        // 生成可能位置に敵を作成

        // デバッグ出力
        Debug.Log(player.transform.position);
    }
}
