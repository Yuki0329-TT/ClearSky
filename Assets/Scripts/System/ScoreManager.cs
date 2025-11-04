using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲーム全体のスコアを管理するクラス。
/// ・スコア加算／減算／リセット
/// ・倍率補正（アイテム効果など）
/// ・ハイスコア更新処理
/// </summary>
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public float score = 0;     //現在のスコア
    public float scoreMultiplier = 1.0f;　//スコア倍率

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }


    /// <summary>
    /// スコア加算処理
    /// </summary>
    public void AddScore(float amount)
    {
        score += Mathf.RoundToInt(amount * scoreMultiplier);
        HighScoreManager.instance.TryUpdateHighScore(score);
    }

    /// <summary>
    /// スコアを2000減算
    /// </summary>
    public void MinusScore()
    {
        if (score >= 2000)
        {
            score -= 2000;
        }
    }

    /// <summary>スコアリセット </summary>
    public void ResetScore() => score = 0;


    /// <summary>現在のスコアを取得 </summary>
    public float GetScore() => score;


    /// <summary>復活可能か判定 </summary>
    public bool CanRevive() => score >= 2000;
}
