using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// ダンジョンの自動生成モジュール
/// </summary>
public class DgGenerator : Generator
{
    /// <summary>
    /// 区画と部屋の余白サイズ
    /// </summary>
    [SerializeField, Tooltip("区画と部屋の余白")]
    private int OUTER_MERGIN = 3;
    /// <summary>
    /// 部屋配置の余白サイズ
    /// </summary>
    [SerializeField, Tooltip("部屋配置の余白サイズ")]
    private int POS_MERGIN = 2;
    /// <summary>
    /// 最小の部屋サイズ
    /// </summary>
    [SerializeField, Tooltip("最小の部屋サイズ")]
    private int MIN_ROOM = 3;
    /// <summary>
    /// 最大の部屋サイズ
    /// </summary>
    [SerializeField, Tooltip("最大の部屋サイズ")]
    private int MAX_ROOM = 5;

    /// <summary>
    /// 区画リスト
    /// </summary>
    private List<DgDivision> _divList = null;

    private Layer2D _layer;

    const string wallName = "Sprites\\test\\dot_0";
    const string tileName = "Sprites\\Tile\\tile";

    public override void Generate(List<Layer2D> layer)
    {
        // データ取得
        WIDTH = layer[(int)LayerType.Under].Width;
        HEIGHT = layer[(int)LayerType.Under].Height;

        // 初期化
        // 2次元配列初期化
        _layer = new Layer2D(WIDTH, HEIGHT);

        // 区画リスト作成
        _divList = new List<DgDivision>();

        // すべてを壁にする
        _layer.Fill((int)ChipType.Wall);

        // 最初の区画を作る
        CreateDivision(0, 0, WIDTH - 1, HEIGHT - 1);

        // 区画を分割する
        // 垂直 or 水平分割フラグの決定
        for (int i = 0; i < 5; i++)
        {
            bool bVertical = (Random.Range(0, 2) == 0);
            SplitDivison(bVertical);
        }
        // 区画に部屋を作る
        CreateRoom();

        // 部屋同士をつなぐ
        for (int i = 0; i < 2; i++) ConnectRooms();

        // デバッグ出力
        foreach (var div in _divList)
        {
            div.Dump();
        }
        _layer.Dump();

        // タイルを配置
        var dangeon = new GameObject("Dangeon");
        for (int j = 0; j < _layer.Height; j++)
        {
            for (int i = 0; i < _layer.Width; i++)
            {
                if (_layer.Get(i, j) == (int)ChipType.Wall)
                {
                    // 壁生成
                    float x = ChipUtil.GetWorldPosX(i);
                    float y = ChipUtil.GetWorldPosY(j);
                    Util.CreateToken(x, y, wallName, "", "Wall")
                        .transform.parent = dangeon.transform;
                }
                else if(_layer.Get(i, j) == (int)ChipType.Road)
                {
                    // 壁生成
                    float x = ChipUtil.GetWorldPosX(i);
                    float y = ChipUtil.GetWorldPosY(j);
                    Util.CreateToken(x, y, tileName, "", "Road")
                        .transform.parent = dangeon.transform;
                }
            }
        }

        layer[(int)LayerType.Under] = _layer;
    }

    /// <summary>
    /// 最初の区画を作る
    /// </summary>
    /// <param name="left">左</param>
    /// <param name="top">上</param>
    /// <param name="right">右</param>
    /// <param name="bottom">下</param>
    void CreateDivision(int left, int top, int right, int bottom)
    {
        var div = new DgDivision();
        div.Outer.Set(left, top, right, bottom);
        _divList.Add(div);
    }

    /// <summary>
    /// 区画を分割する
    /// </summary>
    /// <param name="bVertical">垂直分割するかどうか</param>
    void SplitDivison(bool bVertical)
    {
        // 末尾の要素を取り出し
        var parent = _divList[_divList.Count - 1];
        _divList.Remove(parent);

        // 子となる区画を生成
        var child = new DgDivision();

        if (bVertical)
        {
            // 縦方向に分割する
            if (CheckDivisionSize(parent.Outer.Height) == false)
            {
                // 縦の高さが足りない
                // 親区画を戻しておしまい
                _divList.Add(parent);
                return;
            }

            // 分割ポイントを求める
            int a = parent.Outer.Top    + (MIN_ROOM + OUTER_MERGIN);
            int b = parent.Outer.Bottom - (MIN_ROOM + OUTER_MERGIN);
            // AB間の距離を求める
            int ab = b - a;
            // 最大の部屋サイズを超えないようにする
            ab = Mathf.Min(ab, MAX_ROOM);

            // 分割点を決める
            int p = a + Random.Range(0, ab + 1);

            // 子区画に情報を設定
            child.Outer.Set(
                parent.Outer.Left, p, parent.Outer.Right, parent.Outer.Bottom);

            // 親の下側をp地点まで縮める
            parent.Outer.Bottom = child.Outer.Top;
        }
        else
        {
            // 横方向に分割する
            if (CheckDivisionSize(parent.Outer.Width) == false)
            {
                // 横幅が足りない
                // 親区画を戻しておしまい
                _divList.Add(parent);
                return;
            }

            // 分割ポイントを求める
            int a = parent.Outer.Left  + (MIN_ROOM + OUTER_MERGIN);
            int b = parent.Outer.Right - (MIN_ROOM + OUTER_MERGIN);
            // AB間の距離を求める
            int ab = b - a;
            // 最大の部屋サイズを超えないようにする
            ab = Mathf.Min(ab, MAX_ROOM);

            // 分割点を求める
            int p = a + Random.Range(0, ab + 1);

            // 子区画に情報を設定
            child.Outer.Set(
                p, parent.Outer.Top, parent.Outer.Right, parent.Outer.Bottom);

            // 親の右側をp地点まで縮める
            parent.Outer.Right = child.Outer.Left;
        }

        // 次に分割する区画をランダムで決める
        if (Random.Range(0, 2) == 0)
        {
            // 子を分割する
            _divList.Add(parent);
            _divList.Add(child);
        }
        else
        {
            // 親を分割する
            _divList.Add(child);
            _divList.Add(parent);
        }

        // 分割処理を再帰呼び出し (分割方向は縦横交互にする)
        SplitDivison(!bVertical);
    }

    /// <summary>
    /// 指定のサイズを持つ区画を分割できるかどうか
    /// </summary>
    /// <param name="size">チェックする区画のサイズ</param>
    /// <returns>分割できればtrue</returns>
    bool CheckDivisionSize(int size)
    {
        // (最小の部屋サイズ + 余白)
        // 2分割なので x2 する
        // +1 して連絡通路用のサイズも残す
        int min = (MIN_ROOM + OUTER_MERGIN) * 2 + 1;

        return size >= min;
    }

    /// <summary>
    /// 区画に部屋を作る
    /// </summary>
    void CreateRoom()
    {
        foreach (var div in _divList)
        {
            // 基準サイズを決める
            int dw = div.Outer.Width - OUTER_MERGIN;
            int dh = div.Outer.Height - OUTER_MERGIN;

            // 大きさをランダムに決める
            int sw = Random.Range(MIN_ROOM, dw);
            int sh = Random.Range(MIN_ROOM, dh);

            // 最大サイズを超えないようにする
            sw = Mathf.Min(sw, MAX_ROOM);
            sh = Mathf.Min(sh, MAX_ROOM);

            // 空きサイズを計算 (区画 - 部屋)
            int rw = (dw - sw);
            int rh = (dh - sh);
            
            // 部屋の左上位置を決める
            int rx = Random.Range(0, rw) + POS_MERGIN;
            int ry = Random.Range(0, rh) + POS_MERGIN;

            int left   = div.Outer.Left + rx;
            int right  = left + sw;
            int top    = div.Outer.Top + ry;
            int bottom = top + sh;

            // 部屋のサイズを設定
            div.Room.Set(left, top, right, bottom);

            // 部屋を通路にする
            FillDgRect(div.Room);
        }
    }

    /// <summary>
	/// DgRectの範囲を塗りつぶす
    /// </summary>
    /// <param name="rect">矩形情報</param>
    void FillDgRect(DgDivision.DgRect r)
    {
        _layer.FillRectLTRB(r.Left, r.Top, r.Right, r.Bottom, (int)ChipType.Road);
    }

    /// <summary>
    /// 部屋同士を通路でつなぐ
    /// </summary>
    void ConnectRooms()
    {
        for (int i = 0; i < _divList.Count - 1; i++)
        {
            // リストの前後の区画は必ず接続できる
            var a = _divList[i];
            var b = _divList[i + 1];

            // 2つの部屋をつなぐ通路を作成
            CreateRoad(a, b);

			// 孫にも接続する
			for(int j = i + 2; j < _divList.Count; j++)
			{
				var c = _divList[j];
				if(CreateRoad(a, c, true))
				{
					// 孫に接続できたらおしまい
					break;
				}
			}
        }
    }

    /// <summary>
    /// 指定した部屋の間を通路でつなぐ
    /// </summary>
    /// <param name="divA">部屋1</param>
    /// <param name="divB">部屋2</param>
	/// <param name="bGrandChild">孫チェックするかどうか</param>
    /// <returns>つなぐことができたらtrue</returns>
	bool CreateRoad(DgDivision divA, DgDivision divB, bool bGrandChild=false)
    {
        if (divA.Outer.Bottom == divB.Outer.Top || divA.Outer.Top == divB.Outer.Bottom)
        {
            // 上下でつながっている
            // 部屋から伸ばす通路の開始位置を決める
            int x1 = Random.Range(divA.Room.Left, divA.Room.Right);
            int x2 = Random.Range(divB.Room.Left, divB.Room.Right);
            int y  = 0;

			if(bGrandChild)
			{
				// すでに通路が存在していたらその情報を使用する
				if(divA.HasRoad()) { x1 = divA.Road.Left; }
				if(divB.HasRoad()) { x2 = divB.Road.Left; }
			}

            if (divA.Outer.Top > divB.Outer.Top)
            {
                // B - A (Bが上側)
                y = divA.Outer.Top;
                // 通路を作成
				divA.CreateRoad(x1, y + 1, x1 + 1, divA.Room.Top);
				divB.CreateRoad(x2, divB.Room.Bottom, x2 + 1, y);
            }
            else
            {
                // A - B (Aが上側)
                y = divB.Outer.Top;
                // 通路を作成
				divA.CreateRoad(x1, divA.Room.Bottom, x1 + 1, y);
				divB.CreateRoad(x2, y, x2 + 1, divB.Room.Top);
            }
			FillDgRect(divA.Road);
			FillDgRect(divB.Road);

            // 通路同士を接続する
            FillHLine(x1, x2, y);

            // 通路を作れた
            return true;
        }

        if (divA.Outer.Left == divB.Outer.Right || divA.Outer.Right == divB.Outer.Left)
        {
            // 左右でつながっている
            // 部屋から伸ばす通路の開始位置を決める
            int y1 = Random.Range(divA.Room.Top, divA.Room.Bottom);
            int y2 = Random.Range(divB.Room.Top, divB.Room.Bottom);
            int x  = 0;

			if(bGrandChild)
			{
				// すでに通路が存在していたらその情報を使う
				if(divA.HasRoad()) { y1 = divA.Road.Top; }
				if(divB.HasRoad()) { y2 = divB.Road.Top; }
			}

            if (divA.Outer.Left > divB.Outer.Left)
            {
                // B - A (Bが左側)
                x = divA.Outer.Left;
                // 通路を作成
				divB.CreateRoad(divB.Room.Right, y2, x, y2 + 1);
				divA.CreateRoad(x + 1, y1, divA.Room.Left, y1 + 1);
            }
            else
            {
                // A - B (Aが左側)
                x = divB.Outer.Left;
				divA.CreateRoad(divA.Room.Right, y1, x, y1 + 1);
				divB.CreateRoad(x, y2, divB.Room.Left, y2 + 1);
            }
			FillDgRect(divA.Road);
			FillDgRect(divB.Road);

            // 通路同士を接続する
            FillVLine(y1, y2, x);

            // 通路を作れた
            return true;
        }


        // つなげなかった
        return false;
    }

    /// <summary>
    /// 水平方向に線を引く (左と右の位置は自動で反転する)
    /// </summary>
    /// <param name="left">左</param>
    /// <param name="right">右</param>
    /// <param name="y">Y座標</param>
    void FillHLine(int left, int right, int y)
    {
        if (left > right)
        {
            // 左右の位置関係が逆なので値をスワップする
            int tmp = left;
            left = right;
            right = tmp;
        }
        _layer.FillRectLTRB(left, y, right + 1, y + 1, (int)ChipType.Road);
    }

    /// <summary>
    /// 垂直方向に線を引く (上と下の位置は自動で反転する)
    /// </summary>
    /// <param name="top">上</param>
    /// <param name="bottom">下</param>
    /// <param name="x">X座標</param>
    void FillVLine(int top, int bottom, int x)
    {
        if (top > bottom)
        {
            // 上下の位置関係が逆なので値をスワップする
            int tmp = top;
            top = bottom;
            bottom = tmp;
        }
        _layer.FillRectLTRB(x, top, x + 1, bottom + 1, (int)ChipType.Road);
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(160, 160, 128, 32), "もう１回"))
        {
            SceneManager.LoadScene(0);
        }
    }
}
