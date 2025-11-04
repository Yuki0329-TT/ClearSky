using UnityEngine;

/// <summary>
/// ステージ進行を管理するクラス
/// スコアやレベル情報を保存し、次のステージ生成やリセットを制御する
/// </summary>
/// StageProgressManager → ScoreProgressManager　のほうがいいかも？　被るしわかりづらい？
public class StageProgressManager : MonoBehaviour
{
    public static StageProgressManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    /// <summary>
    /// 次のシーンをロード、生成する
    /// </summary>
    public void LoadNextStage()
    {
        //ステージ番号を増やして保存
        int level = PlayerPrefs.GetInt("LevelNum", 1) + 1;
        PlayerPrefs.SetInt("LevelNum", level);


        //難易度上昇
        StageManager stageManager = FindObjectOfType<StageManager>();
        if (stageManager != null)
        {
            stageManager.difficultyLevel++;
            stageManager.RegenerateStage();
        }


        //ゲームクリア状態をリセット
        FindObjectOfType<GameClear>()?.ResetClearCheck();
    }



    /// <summary>
    /// ステージレベルを初期状態にリセットする
    /// </summary>
    public void ResetLevel()
    {
        PlayerPrefs.SetInt("LevelNum", 1);
    }
}
