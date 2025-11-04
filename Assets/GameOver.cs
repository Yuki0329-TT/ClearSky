using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// プレイヤーがやられた時のゲームオーバー処理を担当するクラス
/// 
/// ・プレイヤーが倒されたら自動的にGameOverCanvasを表示
/// ・一定スコアを消費して復活できる「リスタート」機能を提供
/// </summary>
public class GameOver : MonoBehaviour
{
    public GameObject Player; // プレイヤーオブジェクト
    public GameObject GameOverCanvas; // ゲームオーバー時に表示するキャンバス
    public GameObject InsufficientScorePanel; // スコア不足時の警告パネル


    [Header("復活ボタン")]
    public Button reviveButton;
    private void Update()
    {
        // プレイヤーが存在しない（倒された）場合、ゲームオーバー表示
        if (!Player)
        {
            GameOverCanvas.SetActive(true);   


            //スコアに応じて復活ボタンの有効、無効切り替え
            if(reviveButton != null && ScoreManager.instance != null)
            {
                reviveButton.interactable = ScoreManager.instance.CanRevive();
            }
        }
    }


    /// <summary>
    /// リスタート処理（復活条件あり）
    /// </summary>
    public void GameReStart()
    {
        if (ScoreManager.instance != null)
        {
            if (ScoreManager.instance.CanRevive())　　//スコアから-2000して復活
            {
                ScoreManager.instance.MinusScore();
                float currentScore = ScoreManager.instance.GetScore();


                // シーン再読み込み
                SceneManager.sceneLoaded += (scene, mode) =>
                {
                    ScoreManager.instance.score = currentScore; // 復活後に再設定
                };


                SceneManager.LoadScene(SceneManager.GetActiveScene().name); // 現在のシーンを再読み込み
                UbhObjectPool.instance.ReleaseAllBullet(true); // 弾消去
            }
            else
            {
                Debug.Log("スコアが足りないから復活できないよ");

                if (InsufficientScorePanel != null)
                {
                    InsufficientScorePanel.SetActive(true);
                }

                if (reviveButton != null)
                {
                    reviveButton.interactable = false;
                }
            }
        }
    }
   
} 
