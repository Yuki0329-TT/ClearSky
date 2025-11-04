using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 画面外に出た敵オブジェクトを自動で削除するクラス
/// </summary>
public class EnemyDestroy : MonoBehaviour
{
    private float xLimit = 40f; // 横方向の削除範囲
    private float yLimit = 20f; // 縦方向の削除範囲


    private void Update()
    {
        Vector3 pos = transform.position;

        // 画面外に出たらオブジェクトを破壊
        if (Mathf.Abs(pos.x) > xLimit || Mathf.Abs(pos.y) > yLimit)
        {
            Destroy(gameObject);
        }
    }
} 
