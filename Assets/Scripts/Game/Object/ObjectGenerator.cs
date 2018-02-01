using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGenerator : Generator
{
    public override void Generate(List<Layer2D> layer)
    {
        // データ取得
        WIDTH = layer[(int)LayerType.Under].Width;
        HEIGHT = layer[(int)LayerType.Under].Height;

        // 生成可能位置リストを取得
        var popList = new List<KeyValuePair<int, int>>();
        popList = GetGeneratedList(layer[(int)LayerType.Under]);

        // 生成可能位置にプレイヤーを作成

        int rand = Random.Range(0, popList.Count - 1);

        var token = Util.CreateToken(0.0f, 0.0f, "Sprites\\Tile\\goal", "", "Goal");
        token.Renderer.sortingOrder = 1;
        var x = popList[rand].Key;
        var y = popList[rand].Value;
        layer[(int)LayerType.Top].Set(x, y, (int)ChipType.Goal);
        token.SetPosition(ChipUtil.GetChipPos(new Vector2(x, y)));
    }
}
