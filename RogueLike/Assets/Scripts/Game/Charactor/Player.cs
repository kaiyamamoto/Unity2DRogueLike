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
        //if (!InGameManager.GetInstance().playersTurn) return;

        int horizontal = 0;     // 水平移動方向
        int vertical = 0;       // 垂直移動方向

        // 水平移動方向設定
        horizontal = (int)(Input.GetAxisRaw("Horizontal"));
        // 垂直移動方向取得
        vertical = (int)(Input.GetAxisRaw("Vertical"));
        var dir = ChipUtil.CreateDirection(new Vector2Int(horizontal, vertical));

        // 移動があるか確認
        if (horizontal != 0 || vertical != 0)
        {
            var list = InGameManager.GetInstance().LayerList;
            AttemptMove(list[(int)LayerType.Under], dir);
        }
    }

    protected override void AttemptMove(Layer2D layer,Direction dir)
    {
        base.AttemptMove(layer,dir);

        // スタミナを減らす
        _stamina--;

        CheckIfGameOver();

        // プレイヤーのターンを終了する
        InGameManager.GetInstance().playersTurn = false;
    }

    protected override void OnCantMove<T>(T component)
    {
        // 壁と当たったらダメージを与える
        Wall hitWall = component as Wall;
        hitWall.DamageWall(_wallDamage);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 出口の時
        if (other.tag == "Exit")
        {
            // 遅延してリスタート
            Invoke("Restart", _restartLevelDelay);

            // プレイヤーを一度無効
            enabled = false;
        }
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    private void CheckIfGameOver()
    {
        //if (_stamina < 0) InGameManager.GetInstance().GameOver();
    }
}


