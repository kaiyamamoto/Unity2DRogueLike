using UnityEngine;
using System.Collections;
using UnityEngine.UI;	//Allows us to use UI.
using UnityEngine.SceneManagement;

public class Player : MovingObject
{
    private float _restartLevelDelay = 1f;       // レベル表示から再起動までの遅延時間
    private int _wallDamage = 1;                 // 壁に与えるダメージ
    [SerializeField]
    private int _stamina = 100;
    public int Stamina
    {
        get { return _stamina; }
        set { _stamina = value; }
    }

#if UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
        private Vector2 touchOrigin = -Vector2.one;	//Used to store location of screen touch origin for mobile controls.
#endif

    protected override void Start()
    {
        // 基底のStart
        base.Start();
    }

    private void Update()
    {
        // 自分のターンじゃないとき処理しない
        if (!InGameManager.GetInstance().playersTurn) return;

        int horizontal = 0;     // 水平移動方向
        int vertical = 0;       // 垂直移動方向

        // 水平移動方向設定
        horizontal = (int)(Input.GetAxisRaw("Horizontal"));
        // 垂直移動方向取得
        vertical = (int)(Input.GetAxisRaw("Vertical"));

        // 移動があるか確認
        if (horizontal != 0 || vertical != 0)
        {
            var dir = ChipUtil.CreateDirection(new Vector2Int(horizontal, vertical));
            var list = InGameManager.GetInstance().LayerList;
            AttemptMove(list[(int)LayerType.Under], dir);
        }
    }

    protected override Vector3 AttemptMove(Layer2D layer,Direction dir)
    {
        var move = base.AttemptMove(layer,dir);

        // スタミナを減らす
        _stamina--;
        InGameManager.GetInstance().staminaText.text = "Stamina: " + Stamina;

        CheckIfGameOver();

        // ゴールがある場合リロード
        if(InGameManager.GetInstance().GoalCheck(ChipUtil.GetChipPos(new Vector2(move.x,move.y))))
        {
            // 遅延してリスタート
            Restart();

            // プレイヤーを一度無効
            enabled = false;
        }

        // プレイヤーのターンを終了する
        InGameManager.GetInstance().playersTurn = false;

        return move;
    }

    protected override void OnCantMove<T>(T component)
    {
        // 壁と当たったらダメージを与える
        Wall hitWall = component as Wall;
        hitWall.DamageWall(_wallDamage);
    }

    private void Restart()
    {
        InGameManager.GetInstance().InitGame();
    }

    private void CheckIfGameOver()
    {
        if (_stamina <= 0) StartCoroutine(InGameManager.GetInstance().GameOver());
    }
}


