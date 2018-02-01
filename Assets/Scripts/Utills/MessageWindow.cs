using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageWindow : MonoBehaviour
{
    // 実体
    private static MessageWindow _instance;
    public static MessageWindow Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (MessageWindow)FindObjectOfType(typeof(MessageWindow));

                if (_instance == null) Debug.LogError(typeof(MessageWindow) + "is nothing");
            }

            return _instance;
        }

        private set { _instance = value; }
    }

    private struct MessageData
    {
        public string Message { get; set; }
    }

    /// 表示可能ログ最大数
    [SerializeField]
    private static int MESSAGE_MAX = 5;

    /// 各レベルのログ色
    private static Color[] MESSAGE_COLOR =
    {
        Color.gray,
        Color.white,
        Color.cyan,
        Color.yellow,
        Color.red
    };

    [SerializeField]
    private Text _messageWindow = null;

    private Queue<MessageData> _messageQue = new Queue<MessageData>(MESSAGE_MAX);    // ログキュー
    private Vector2 _scrollPosition = Vector2.zero;                  // スクロールビュー位置
    private bool _isNeedScrollReset = false;                         // スクロール位置リセットフラグ

    public void Message(string message, bool isConsole = false) { _Push(message, isConsole); }

    private void Awake()
    {
        // 既に存在している場合削除
        if (this != Instance)
        {
            Destroy(this.gameObject);
            return;
        }

        // シーン間をまたぐ
        DontDestroyOnLoad(this.gameObject);
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    private void FixedUpdate()
    {
        DrawMessageWindow();
    }

    /// <summary>
    /// ログを画面上に表示する
    /// OnGUI() 内で呼び出す
    /// </summary>
    /// <param name="drawArea">描画対象領域（左上が0,0）</param>
    private void DrawMessageWindow()
    {
        var text = _messageWindow.text;
        text = "";
        foreach (var message in _messageQue)
        {
            text = text + message.Message + "\n";
            _messageWindow.text = text;
        }
    }

    /// <summary>
    /// ログをキューに詰める
    /// </summary>
    /// <param name="level"></param>
    /// <param name="message"></param>
    /// <param name="isConsole"></param>
    private void _Push(string message, bool isConsole)
    {
        // キューが一杯だったら後ろを削除
        if (_messageQue.Count >= MESSAGE_MAX)
        {
            _messageQue.Dequeue();
        }

        // キューに詰める
        var data = new MessageData();
        {
            data.Message = message;
        }
        _messageQue.Enqueue(data);

        // ログ位置を調整する
        // @todo 出る度に勝手に動く。要らないかも？
        _isNeedScrollReset = true;

        // コンソール出力
        if (isConsole)
        {
            Debug.Log(message);
        }
    }
}