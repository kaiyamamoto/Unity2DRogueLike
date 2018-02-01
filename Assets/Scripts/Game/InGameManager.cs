using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class InGameManager : MonoBehaviour
{

    private const float DASH_SPEED = 0.1f;

    public float levelStartDelay = 2f;                      // 開始前の待機時間
    public float turnDelay = 0.1f;                          // 移動のディレイ
    public int playerStaminaPoints = 100;                   // スタミナの開始値
    private static InGameManager instance = null;           // インスタンス
    [HideInInspector]
    public bool playersTurn = true;                         // プレイヤーの移動フラグ

    private Player player;                                  // プレイヤー
    private Text levelText;                                 // レベル表記用テキスト
    private Text staminaText;                               // スタミナ表記用テキスト
    private int level = 0;                                  // 現在のレベル
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
    }

    // ゲームの初期化
    public void InitGame()
    {
        StartCoroutine(InitSetting());
    }

    private IEnumerator InitSetting()
    {
        doingSetup = true;
        yield return FadeManager.Instance.FadeIn(1.0f);

        // レベルを+してゲーム初期化
        instance.level++;

        var async = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
        while (!async.isDone)
        {
            yield return null;
        }

        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        levelText.text = "Lv" + level;
        levelText.gameObject.SetActive(true);

        staminaText = GameObject.Find("FoodText").GetComponent<Text>();

        // 敵を全て削除
        enemies.Clear();

        ElementGenerator e = (ElementGenerator)FindObjectOfType(typeof(ElementGenerator));
        e.Generate();
        yield return new WaitForSeconds(1.0f);
        // 非表示
        levelText.gameObject.SetActive(false);

        yield return FadeManager.Instance.FadeOut(1.0f);
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
        levelText.gameObject.SetActive(true);

        enabled = false;
    }

    // 敵移動コルーチン
    IEnumerator MoveEnemies()
    {
        // 敵移動フラグ
        enemiesMoving = true;

        if (Input.GetKey(KeyCode.LeftShift) == false) 
        {
            // 遅延
            yield return new WaitForSeconds(turnDelay);
            if (enemies.Count == 0)
            {
                yield return new WaitForSeconds(turnDelay);
            }
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

    // ゴールのチェック
    public bool GoalCheck(Vector2Int pos)
    {
        if(_layerList[(int)LayerType.Top].Get(pos.x,pos.y)==(int)ChipType.Goal)
        {
            return true;
        }
        return false;
    }
}

