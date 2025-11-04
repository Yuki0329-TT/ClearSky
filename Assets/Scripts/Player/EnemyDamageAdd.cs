using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 弾が敵に衝突した際にダメージを与えるクラス。
/// </summary>
/// 
public class DamageAdd : MonoBehaviour
{
    public int damage = 100;

   public void OnTriggerEnter2D(Collider2D collision)
    {
       if (collision.CompareTag("Enemy")|| collision.CompareTag("Boss"))
        {

            IDamaged[] damageArr = collision.GetComponentsInChildren<IDamaged>();
            foreach (IDamaged d in damageArr)
            {
                d.Damaged(damage);
            }

           Destroy(gameObject);
        }
       


    }
}
