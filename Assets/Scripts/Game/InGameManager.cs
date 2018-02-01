using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class InGameManager : MonoBehaviour
{
    public float levelStartDelay = 2f;                      // 開始前の待機時間
    public float turnDelay = 0.1f;                          // 移動のディレイ
    public int playerStaminaPoints = 100;                   // スタミナの開始値
    private static InGameManager instance = null;             // インスタンス
    [HideInInspector]
    public bool playersTurn = true;       // プレイヤーの移動フラグ

    private Player player;                                  // プレイヤー
    private Text levelText;                                 // レベル表記用テキスト
    private Text staminaText;                               // スタミナ表記用テキスト
    private GameObject levelImage;                          // レベルのプレハブ
    private int level = 1;                                  // 現在のレベル
    private List<Enemy> enemies;                            // 敵のリスト
    private bool enemiesMoving;                             // 敵の移動フラグ
    private bool doingSetup = true;                         // ボードを設定中のフラグ

    // レイヤーのリスト
    private List<Layer2D> _layerList;
    public List<Layer2D> LayerList
    {
        get { return _layerList; }
        set { _layerList = value; }
    }

    public static InGameManager GetInstance() { return instance; }

    void Awake()
    {
        // 実体作成
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);

        // 消えなくする
        DontDestroyOnLoad(gameObject);

        // 敵のリスト作成
        enemies = new List<Enemy>();

        // ゲームの初期化
        InitGame();
    }

    // シーンロード後に呼ばれる
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static public void CallbackInitialization()
    {
        // コールバック登録
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // シーンロード後に呼ばれるコールバック
    static private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        // レベルを+してゲーム初期化
        instance.level++;
        instance.InitGame();
    }

    // ゲームの初期化
    void InitGame()
    {
        doingSetup = true;
        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        levelText.text = "Lv" + level;
        levelImage.SetActive(true);

        staminaText = GameObject.Find("FoodText").GetComponent<Text>();

        Invoke("HideLevelImage", levelStartDelay);

        // 敵を全て削除
        enemies.Clear();
    }

    void HideLevelImage()
    {
        // 非表示
        levelImage.SetActive(false);
        doingSetup = false;
    }

    void Update()
    {

        //staminaText.text = "Stamina: " + player.Stamina;

        if (playersTurn || enemiesMoving || doingSetup)
            return;

        // 敵の動き
        StartCoroutine(MoveEnemies());
    }

    public void AddEnemyToList(Enemy script)
    {
        // 敵を追加
        enemies.Add(script);
    }


    public void GameOver()
    {
        levelText.fontSize = 20;
        levelText.text = "GameOver You Lv:" + level;
        levelImage.SetActive(true);

        enabled = false;
    }

    // 敵移動コルーチン
    IEnumerator MoveEnemies()
    {
        // 敵移動フラグ
        enemiesMoving = true;

        // 遅延
        yield return new WaitForSeconds(turnDelay);

        if (enemies.Count == 0)
        {
            yield return new WaitForSeconds(turnDelay);
        }

        for (int i = 0; i < enemies.Count; i++)
        {
            // 敵の移動
            enemies[i].MoveEnemy();
            yield return new WaitForSeconds(enemies[i].MoveTime);
        }
        // プレイヤーターン開始
        playersTurn = true;
        enemiesMoving = false;
    }
}

