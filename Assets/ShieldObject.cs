using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldObject : MonoBehaviour
{
    public Status playerStatus;
    public int shieldHp;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Enemy_Bullet"))
        {
            Debug.Log("シールドが防御した");
            shieldHp -= 60;
            Destroy(other.gameObject);  // 敵や弾を消す

            if (shieldHp <= 0)
            {
                Destroy(gameObject);  // シールドが壊れる
            }

        }
    }
}
