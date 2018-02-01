using UnityEngine;
using System.Collections;

// 2次元レイヤー
// -1以下は領域外
public class Layer2D
{
    Vector2Int _size;   // サイズ
    int[] _values = null; // マップデータ

    // サイズ
    public Vector2Int Size
    {
        get { return _size; }
    }

    // 幅
    public int Width
    {
        get { return _size.x; }
    }
    // 高さ
    public int Height
    {
        get { return _size.y; }
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public Layer2D(int width = 0, int height = 0)
    {
        if (width > 0 && height > 0)
        {
            Create(new Vector2Int(width, height));
        }
    }

    // 作成
    public void Create(Vector2Int size)
    {
        _size = size;
        _values = new int[Width * Height];
    }

    // 座標をインデックスに変換する
    public int ToIdx(int x, int y)
    {
        return x + (y * Width);
    }

    // 領域外かどうかチェックする
    public bool IsOutOfRange(int x, int y)
    {
        if (x < 0 || x >= Width) { return true; }
        if (y < 0 || y >= Height) { return true; }

        // 領域内
        return false;
    }

    /// <summary>
    /// 値の取得
    /// </summary>
    /// <param name="x">X座標</param>
    /// <param name="y">Y座標</param>
    /// <returns>指定の座標の値（領域外を指定したら_outOfRangeを返す）</returns>
    public int Get(int x, int y)
    {
        if (IsOutOfRange(x, y))
        {
            return -1;
        }

        return _values[y * Width + x];
    }

    /// <summary>
    /// 値の設定
    /// </summary>
    /// <param name="x">x座標</param>
    /// <param name="y">y座標</param>
    /// <param name="type">設定する値</param>
    public void Set(int x, int y, int type)
    {
        if (IsOutOfRange(x, y))
        {
            // 領域外を指定した
            return;
        }

        _values[y * Width + x] = type;
    }

    /// <summary>
    /// すべてのセルを特定の値で埋める
    /// </summary>
    /// <param name="val">埋める値</param>
    public void Fill(int type)
    {
        for (int j = 0; j < Height; j++)
        {
            for (int i = 0; i < Width; i++)
            {
                Set(i, j, type);
            }
        }
    }

    /// <summary>
    /// 矩形領域を指定の値で埋める
    /// </summary>
    /// <param name="x">矩形の左上(X座標)</param>
    /// <param name="y">矩形の左上(Y座標)</param>
    /// <param name="w">矩形の幅</param>
    /// <param name="h">矩形の高さ</param>
    /// <param name="val">埋める値</param>
    public void FillRect(int x, int y, int w, int h, int type)
    {
        for (int j = 0; j < h; j++)
        {
            for (int i = 0; i < w; i++)
            {
                int px = x + i;
                int py = y + j;
                Set(px, py, type);
            }
        }
    }

    /// <summary>
    /// 矩形領域を指定の値で埋める（4点指定)
    /// </summary>
    /// <param name="left">左</param>
    /// <param name="top">上</param>
    /// <param name="right">右</param>
    /// <param name="bottom">下</param>
    /// <param name="val">埋める値</param>
    public void FillRectLTRB(int left, int top, int right, int bottom, int type)
    {
        FillRect(left, top, right - left, bottom - top, type);
    }

    // デバッグ出力
    public void Dump()
    {
        Debug.Log("[Layer2D] (w,h)=(" + Width + "," + Height + ")");
        for (int y = 0; y < Height; y++)
        {
            string s = "";
            for (int x = 0; x < Width; x++)
            {
                s += (int)Get(x, y) + ",";
            }
            Debug.Log(s);
        }
    }
}
