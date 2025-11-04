using UnityEngine;

/// <summary>
/// 敵が倒されたときにスコアを加算する処理を担当するクラス
/// </summary>
public class EnemyScoreHandler : MonoBehaviour
{
    public int scoreValue;  //敵のスコア値
    private ScoreManager sm;


    void Start()
    {
        sm = GameObject.Find("Score Manager")?.GetComponent<ScoreManager>();

        if (sm == null)
        {
            Debug.LogWarning("Score Manager が見つかりませんでした。");
        }
    }


    /// <summary>
    /// 敵撃破時にスコアを加算する
    /// </summary>
    public void AddScore()
    {
        if (sm == null)
        {
            Debug.LogWarning("ScoreManager が null のためスコア加算できません。");
            return;
        }

        sm.AddScore(scoreValue);
        Debug.Log($"スコア {scoreValue} 加算しました。");
    }
}
