using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ハイスコアを管理し、PlayerPrefsを通して永続化するクラス。
/// </summary>
public class HighScoreManager : MonoBehaviour
{
    public static HighScoreManager instance;


    private const string key = "HIGH SCORE";    //保存キー
    private float highScore;                   //現在のハイスコア

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            highScore = PlayerPrefs.GetFloat(key, 0);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }


    /// <summary>
    /// 現在のスコアがハイスコアを超えていたら更新する
    /// </summary>
    public void TryUpdateHighScore(float score)
    {
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetFloat(key, highScore);
        }
    }


    /// <summary> 現在のハイスコアを取得する </summary>
    public float GetHighScore() => highScore;
}
