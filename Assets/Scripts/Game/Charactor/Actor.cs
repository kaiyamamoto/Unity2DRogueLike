using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : Token {

    // 行動状態
    public enum Act
    {
        KeyInput, // キー入力待ち。もしくは待機中

        // アクション
        ActBegin,   // 開始
        Act,        // 実行中
        ActEnd,     // 終了
        
        // 移動
        MoveBegin,  // 開始
        Move,       // 移動中
        MoveEnd,    // 完了

        TurnEnd,      // ターン終了
    };

    // 更新関数
    public void Proc()
    {

    }

    public void SetChipPosition(Vector2Int pos)
    {
        var wPos = ChipUtil.GetWorldPos(pos);
        base.SetPosition(wPos.x, wPos.y);
    }
    public void SetChipPosition(int x,int y)
    {
        this.SetChipPosition(new Vector2Int(x, y));
    }

    public Vector2Int GetChipPosition()
    {
        var cPos = ChipUtil.GetChipPos(transform.position);
        return cPos;
    }
}
