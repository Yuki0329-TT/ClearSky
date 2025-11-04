using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 画面外に出た弾を自動的に削除する。
/// </summary>
public class DestroyBullet : MonoBehaviour
{
    private float xLimit = 30f;
    private float yLimit = 15f;

    private void Update()
    {
        Vector3 pos = transform.position;

        if (Mathf.Abs(pos.x) > xLimit || Mathf.Abs(pos.y) > yLimit)
        {
            Destroy(gameObject);
        }
    }
}
