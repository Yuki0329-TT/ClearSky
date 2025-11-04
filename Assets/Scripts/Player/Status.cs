using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーがシールドを展開する機能を管理するクラス。
/// </summary>
/// 
public class Status : MonoBehaviour
{
    public GameObject shieldPrefab;  // 生成するシールドのプレハブ

    public static Status Instance;

    private void Awake()
    {
        Instance = this;
        Debug.Log("statusが呼ばれた");
    }

    /// <summary>
    /// シールドを生成してプレイヤーに付与する。
    /// </summary>

    public void ActivateShield()
    {
        Debug.Log(" ActivateShield() の中に入ったよ");

        if (shieldPrefab == null)
        {
            Debug.LogWarning("プレファブがアタッチされてないよ");
            return;
        }

        // シールド生成
        GameObject shield = Instantiate(shieldPrefab, transform.position, Quaternion.identity);

        // プレイヤーの子オブジェクトとして配置
        shield.transform.SetParent(transform);

        ShieldObject shieldScript = shield.GetComponent<ShieldObject>();

        if (shieldScript != null)
        {
            shieldScript.playerStatus = this;
            Debug.Log(" シールド生成成功");
        }
        else
        {
            Debug.Log("シールド建てられてない");
        }
    }
}

