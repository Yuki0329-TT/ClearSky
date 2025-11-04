using UnityEngine;
using System.Collections;

/// <summary>
/// ゲームクリア後のステージ遷移を管理するクラス
/// 
/// ・次ステージロード処理の統括
/// ・弾幕オブジェクトプールのリセット
/// ・ボタン多重押下防止制御
/// 
/// GameClear.cs → GameClearSceneManager → StageProgressManager
/// の流れでステージ進行を制御する
/// </summary>
public class GameClearSceneManager : MonoBehaviour
{
    public static GameClearSceneManager instance;

    private bool nextButtonPressed = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("GameClearの重複発見　古い方削除");
            Destroy(gameObject);
            return;
        }
    }


    /// <summary>
    /// ゲームクリア時に次のステージをロードする処理
    /// </summary>
    public void GameNextScene()
    {
        if (nextButtonPressed) return;　　　//多重入力を防ぐ

        if (GameClear.instance != null)
        {
            GameClear.instance.ResetClearCheck();　　　//ゲームクリアの状態をリセット
        }

        UbhObjectPool.instance?.ReleaseAllBullet(true); // 弾リセット

        StageProgressManager.instance?.LoadNextStage(); // ステージ進行

        nextButtonPressed = true;

        StartCoroutine(ResetNextButtonFlag());
    }


    /// <summary>
    /// 入力制御を一定時間後に解除
    /// </summary>
    private IEnumerator ResetNextButtonFlag()
    {
        yield return new WaitForSeconds(0.6f); // シーンロード時間に合わせて調整
        nextButtonPressed = false;
        Debug.Log(" nextButtonPressed をリセットしました");
    }
}
