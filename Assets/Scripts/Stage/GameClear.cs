using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲームクリア条件の判定・UI制御を担当するクラス
/// 
/// ・敵・ボスの全滅を検知してクリア演出を表示
/// ・シーン遷移時に自動リセット
/// </summary>
public class GameClear : MonoBehaviour
{
    public static GameClear instance;

    public GameObject gameClearCanvas; // UIキャンバス
    public bool bossSpawned = false;   // ボスがいるか判定
    public bool enemySpawned = false;　// 雑魚がいるか判定
    private bool gameClearTrigger = false;　//　クリアしているか判定
    private bool canCheck;　　　　　　　//　判定を開始していいか

    public bool IsGameCleared => gameClearTrigger;　//外部から状態確認用

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

    private void Start()
    {

        StartCoroutine( CheckInitialSpawns());
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // シーン読み込みごとに状態をリセット
        ResetClearCheck();
        StartCoroutine(DelayEnableCheck());
    }


    /// <summary>
    /// シーン開始直後の誤検知を防ぐため、少し待ってから判定を有効化
    /// </summary>
    private IEnumerator DelayEnableCheck() 
    {
        canCheck = false;
        yield return new WaitForSeconds(1.5f); // 生成完了まで少し待つ
        canCheck = true;
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "titleScene") return;
        if (gameClearTrigger) return;

        int bossCount = GameObject.FindGameObjectsWithTag("Boss").Length;
        int enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

        if (bossSpawned && bossCount == 0)
        {
            TriggerGameClear();　　　//ボス戦の場合　ボスを倒したらクリア
        }
        else if (!bossSpawned && enemySpawned && enemyCount == 0)
        {
            TriggerGameClear();　　　//雑魚戦の場合　殲滅でクリア
        }
    }

    private void TriggerGameClear()
    {
        if (gameClearCanvas != null)
            gameClearCanvas.SetActive(true);

        gameClearTrigger = true;
    }


    /// <summary>
    /// ステージ遷移時に状態をリセット
    /// </summary>
    public void ResetClearCheck()
    {
        UbhObjectPool.instance?.ReleaseAllBullet(true);
        bossSpawned = false;
        enemySpawned = false;
        gameClearTrigger = false;
        canCheck = false;

        if (gameClearCanvas != null)
            gameClearCanvas.SetActive(false);

        StartCoroutine(CheckInitialSpawns());

        Debug.Log("状態リセット");
    }


    /// <summary>
    /// ステージ開始時に敵・ボスの初期配置を確認
    /// </summary>
    public IEnumerator CheckInitialSpawns()
    {
        yield return new WaitForSeconds(0.5f);
        bossSpawned = GameObject.FindGameObjectsWithTag("Boss").Length > 0;
        enemySpawned = GameObject.FindGameObjectsWithTag("Enemy").Length > 0;
      
        Debug.Log($"[GameClearChecker] boss: {bossSpawned}, enemy: {enemySpawned}");
    }
}