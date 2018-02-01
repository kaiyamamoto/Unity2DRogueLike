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

        var player = CreatePlayer(layer, popList);
        CreateEnemies(layer, popList);
        // 生成可能位置に敵を作成

        // デバッグ出力
        Debug.Log(player.transform.position);
    }

    private Player CreatePlayer(List<Layer2D>layer, List<KeyValuePair<int, int>>list)
    {
        // 生成可能位置にプレイヤーを作成
        // プレイヤー生成
        int rand = Random.Range(0, list.Count - 1);

        // 実体作成
        var token = Util.CreatePlayer(0.0f, 0.0f, "Sprites\\Tile\\player", "", "Player");
        token.Renderer.sortingOrder = 1;
        var x = list[rand].Key;
        var y = list[rand].Value;

        token.SetChipPosition(x, y);

        // カメラ設定
        _camera.gameObject.transform.parent = token.gameObject.transform;
        _camera.transform.localPosition = new Vector3(0.0f, 0.0f, -10.0f);

        // リストから削除
        list.Remove(list[rand]);
        // レイヤーに登録
        layer[(int)LayerType.Top].Set(x, y, (int)ChipType.Player);

        return token;
    }

    private List<Enemy> CreateEnemies(List<Layer2D> layer, List<KeyValuePair<int, int>> list)
    {
        List<Enemy> enemyList = new List<Enemy>();

        for (int i = 0; i < _enemyNum; i++)
        {
            if (list.Count == 0) break;

            // 生成可能位置に敵を作成
            // 敵生成
            int rand = Random.Range(0, list.Count - 1);

            // 実体作成
            var token = Util.CreateEnemy(0.0f, 0.0f, "Sprites\\Tile\\enemy", "", "Enemy");
            enemyList.Add(token);

            token.Renderer.sortingOrder = 1;
            var x = list[rand].Key;
            var y = list[rand].Value;

            token.SetChipPosition(x, y);

            // リストから削除
            list.Remove(list[rand]);
            // レイヤーに登録
            layer[(int)LayerType.Top].Set(x, y, (int)ChipType.Enemy);
        }
        return enemyList;
    }

}
