using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 各ステージでの敵生成・難易度調整・ボス出現・アイテム生成を統括するクラス
/// </summary>
public class StageManager : MonoBehaviour
{
    [Header("雑魚敵リスト")]
    public List<GameObject> normalEnemies;

    [Header("ボスリスト")]
    public List<GameObject> bossEnemies;

    // ゲーム難易度　1スタートで上昇していく
    public int difficultyLevel = 1; 

    private void Start()
    {
        // ステージ初期化時に敵、ボス、アイテムを生成
        GenerateStage(); 
    }

    /// <summary>
    /// 現在の難易度に応じて敵・ボスを生成する
    /// </summary>
    private void GenerateStage()
    {
        // 生成前に既存のボスを削除
        foreach (var boss in GameObject.FindGameObjectsWithTag("Boss"))
        {
            Destroy(boss);
        }

        // 難易度に応じて雑魚敵の出現数を調整
        int spawnCount = 1;

        if (difficultyLevel >= 2 && difficultyLevel <= 5)
        {
            spawnCount = 2;
        }
        else if (difficultyLevel >= 6 && difficultyLevel <= 9)
        {
            spawnCount = 3;
        }

        // 雑魚敵をランダムな位置に指定数生成
        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 pos = GetRandomPosition();
            GameObject enemy = normalEnemies[Random.Range(0, normalEnemies.Count)];
            Instantiate(enemy, pos, Quaternion.identity);
        }

        // 難易度が3の倍数のときにボスを出現させる（順番にローテーション）
        if (difficultyLevel % 3 == 0)
        {
            Vector3 pos = GetBossPosition();
            int bossIndex = (difficultyLevel / 3 - 1) % bossEnemies.Count;

            GameObject bossPrefab = bossEnemies[bossIndex];
            GameObject bossInstance = Instantiate(bossPrefab, pos, Quaternion.identity);
            bossInstance.tag = "Boss";

            var enemyScript = bossInstance.GetComponent<EnemyCore>();
            if (enemyScript != null)
            {
                enemyScript.Initialize(true);
            }
        }


        //アイテム生成
        ItemShopManager.instance.GenerateRandomItems();  
    }


    /// <summary>
    /// 雑魚敵を出現させるランダム位置を返す
    /// </summary>
    Vector3 GetRandomPosition()
    {
        return new Vector3(Random.Range(-5f, 5f), Random.Range(-3f, 3f), 0f);
    }


    /// <summary>
    /// ボスの出現位置（固定）を返す
    /// </summary>
    Vector3 GetBossPosition()
    {
        return new Vector3(0, 4f, 0f);
    }


    /// <summary>
    ///  ステージ再生成時に敵・ボスを一旦削除してから再配置
    /// </summary>
    public void RegenerateStage()
    {
        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Destroy(enemy);
        }

        foreach (var boss in GameObject.FindGameObjectsWithTag("Boss"))
        {
            Destroy(boss);
        }
        GenerateStage();
    }
}