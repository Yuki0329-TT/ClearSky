using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// スコアとハイスコアをUIに表示するクラス
/// ScoreManager / HighScoreManager から値を取得して反映する
/// </summary>
public class ScoreUI : MonoBehaviour
{
    public Text scoreText;　　　　//スコア表示用text
    public Text highScoreText;    //ハイスコア表示用text


    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.2f); // シーンロード直後のズレを防ぐ

        //未設定の場合は自動で取得
        if (scoreText == null)
            scoreText = GameObject.Find("ScoreText")?.GetComponent<Text>();

        if (highScoreText == null)
            highScoreText = GameObject.Find("HighScoreText")?.GetComponent<Text>();

        UpdateScoreUI();
    }


    private void Update()
    {
        UpdateScoreUI();
    }

    /// <summary>
    /// 現在のスコアとハイスコアをUIに表示する
    /// </summary>
    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"score : {ScoreManager.instance.GetScore()}";
        }

        if (highScoreText != null)
        {
            highScoreText.text = $"HIGH SCORE : {HighScoreManager.instance.GetHighScore()}";
        }
    }
}
