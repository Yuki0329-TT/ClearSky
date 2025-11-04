using ClearSky;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーにダメージを与えるクラス
/// </summary>
public class PlayerDamagedAdd : MonoBehaviour
{
    [SerializeField] int damage;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            IDamaged[] damageArr = collision.GetComponentsInChildren<IDamaged>();
            foreach (IDamaged d in damageArr)
            {
                d.Damaged(damage);
            }
            // 衝突時、自分自身を破壊
            Destroy(gameObject);
        }
    }
}
