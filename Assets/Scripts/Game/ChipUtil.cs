using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipUtil : MonoBehaviour {
    // デフォルトタイルのpath
    static readonly string DEFAULT_SPRITE = "Sprites\\Tile\\tile";

    // チップのX座標を取得する.
    public static int GetChipPosX(float x)
    {
        var spr = Util.GetSprite(DEFAULT_SPRITE, "");
        var sprW = spr.bounds.size.x;

        return (int)(x / sprW);
    }

    // チップのy座標を取得する.
    public static int GetChipPosY(float y)
    {
        var spr = Util.GetSprite(DEFAULT_SPRITE, "");
        var sprH = spr.bounds.size.y;

        return (int)(y / sprH);
    }

    // チップ座標を取得する
    public static Vector2Int GetChipPos(Vector2 pos)
    {
        return new Vector2Int(GetChipPosX(pos.x), GetChipPosY(pos.y));
    }

    // ワールドX座標を取得する.
    public static float GetWorldPosX(int i)
    {
        var spr = Util.GetSprite(DEFAULT_SPRITE, "");
        var sprW = spr.bounds.size.x;

        return (sprW * i);
    }

    // ワールドy座標を取得する.
    public static float GetWorldPosY(int j)
    {
        var spr = Util.GetSprite(DEFAULT_SPRITE, "");
        var sprH = spr.bounds.size.y;

        return (sprH * j);
    }

    // ワールド座標を取得する
    public static Vector2 GetWorldPos(Vector2Int chip)
    {
        var pos = new Vector2(GetWorldPosX(chip.x), GetWorldPosY(chip.y));
        return pos;
    }


    public static Direction CreateDirection(Vector2Int vel)
    {
        var dir = Direction.None;
        if (vel.y > 0) dir = Direction.Up;
        if (vel.y < 0) dir |= Direction.Down;
        if (vel.x < 0) dir |= Direction.Left;
        if (vel.x > 0) dir |= Direction.Right;

        MessageWindow.Instance.Message(dir.ToString());

        return dir;
    }

    /// <summary>
    /// 移動先の座標取得
    /// </summary>
    /// <param name="position">現在の位置</param>
    /// <param name="dir">移動方向</param>
    /// <returns>移動後の位置</returns>
    public static Vector2Int MoveToPosition(Vector2Int position, Direction dir)
    {
        if (dir == Direction.None) return position;

        Vector2Int vel = new Vector2Int();
        if ((dir & Direction.Up)   != 0) vel.y +=  1;
        if ((dir & Direction.Down) != 0) vel.y += -1;
        if ((dir & Direction.Left) != 0) vel.x += -1;
        if ((dir & Direction.Right)!= 0) vel.x +=  1;

        return position + vel;
    }

    /// <summary>
    /// 位置に存在するチップの種類を取得
    /// </summary>
    /// <param name="layer">確認するレイヤー</param>
    /// <param name="pos">確認する位置</param>
    /// <returns>チップの種類</returns>
    public static ChipType GetExistsChip(Layer2D layer,Vector2Int pos)
    {
       return (ChipType)layer.Get(pos.x, pos.y);
    }
}
