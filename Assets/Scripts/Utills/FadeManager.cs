using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;



/// <summary>
/// フェード制御クラス
/// </summary>
public class FadeManager : MonoBehaviour
{
    // 実体
    private static FadeManager _instance;

    public static FadeManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (FadeManager)FindObjectOfType(typeof(FadeManager));

                if (_instance == null) Debug.LogError(typeof(FadeManager) + "is nothing");
            }

            return _instance;
        }

        private set { _instance = value; }
    }

    // 透明度
    [SerializeField]
    private float _fadeAlpha = 0.0f;

    // フェード中のフラグ
    [SerializeField]
    private bool _isFading = false;

    // フェードの色
    [SerializeField]
    private Color _fadeColor = Color.black;

    private Image _image = null;

    public void Awake()
    {
        // 既に存在している場合削除
        if (this != Instance)
        {
            Destroy(this.gameObject);
            return;
        }

        // 初期透明度反映
        StartCoroutine(FadeIn(0.0f));

        _image = GetComponent<Image>();

        // シーン間をまたぐ
        DontDestroyOnLoad(this.gameObject);
    }

    private void FixedUpdate()
    {
        // フェード中更新
        if (this._isFading)
        {
            AlphaUpdate();
        }
    }

    // フェードの更新
    private void AlphaUpdate()
    {
        // 色と透明度を更新して白テクスチャを描画
        this._fadeColor.a = this._fadeAlpha;
        var mat = _image.material;
        mat.color = this._fadeColor;
        _image.material = mat;
    }

    /// <summary>
    /// 画面遷移
    /// </summary>
    /// <param name='scene'>シーン名</param>
    /// <param name='interval'>暗転時間(秒)</param>
    public void LoadScene(string scene, float interval)
    {
        StartCoroutine(TransFade(() => {
            SceneManager.LoadScene(scene);
            return true;
        }, 1.0f));
    }

    /// <summary>
    /// フェードインコルーチン
    /// </summary>
    /// <param name="interval">暗転時間</param>
    /// <returns></returns>
    public IEnumerator FadeIn(float interval)
    {
        // 徐々に暗転
        this._isFading = true;
        float time = 0;
        while (time <= interval)
        {
            if (interval == 0) this._fadeAlpha = 1;
            else this._fadeAlpha = Mathf.Lerp(0f, 1f, time / interval);
            time += Time.deltaTime;
            yield return 0;
        }
    }

    /// <summary>
    /// フェードアウトコルーチン
    /// </summary>
    /// <param name="interval">暗転時間</param>
    /// <returns></returns>
    public IEnumerator FadeOut(float interval)
    {
        // 徐々に明るく
        this._isFading = true;
        float time = 0;
        while (time <= interval)
        {
            this._fadeAlpha = Mathf.Lerp(1f, 0f, time / interval);
            time += Time.deltaTime;
            yield return 0;
        }

        this._isFading = false;
    }

    /// <summary>
    /// フェードコルーチン
    /// </summary>
    /// <param name='scene'>フェード後の処理</param>
    /// <param name='interval'>暗転時間(秒)</param>
    public IEnumerator TransFade(Func<bool> func, float interval)
    {
        yield return TransFade(func, interval, interval);
    }
    public IEnumerator TransFade(Func<bool> func, float inInterval, float outInterval)
    {
        // 徐々に暗転
        yield return FadeIn(inInterval);

        // シーン切替
        yield return func();

        // 徐々に明るく
        yield return FadeOut(outInterval);
    }

}

