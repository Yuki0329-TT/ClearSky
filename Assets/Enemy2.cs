using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


/// <summary>
/// プレイヤーの弾をリアルタイムで検知し、自動的に回避行動を行う敵AIクラス
/// 
/// ・Physics2D.OverlapCircleAll で弾を探索
/// ・NavMeshAgent を2D用に調整して回避移動
/// ・回避方向・距離にランダム性を持たせることで自然な動きを表現
/// </summary>
public class Enemy2 : MonoBehaviour
{
    private NavMeshAgent agent;


    //移動制限
    float xLimit = 16.5f;
    float yLimit = 5.9f;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false; 
        agent.updateUpAxis = false;
        agent.speed = 20.0f; // 移動速度
        agent.acceleration = 100f; // 加速度
        agent.angularSpeed = 700f;　//　回転速度


        //弾コライダーがTriggerでも検出できるように
        Physics2D.queriesHitTriggers = true;  
    }


    void Update()
    {
        // 一定範囲内に弾があれば回避行動を行う
        if (IsBulletNearby(out Vector3 awayDir))
        {
            Vector3 dodgePosition = transform.position + awayDir * 2f; // 弾と逆方向に2m移動
            agent.SetDestination(dodgePosition);
        }
    }


    /// <summary>
    /// 弾が近くにあるかどうかを調べ、回避方向を計算する
    /// </summary>
    bool IsBulletNearby(out Vector3 awayDir)
    {
        Physics2D.queriesHitTriggers = true; 

        int mask = LayerMask.GetMask("Default", "PlayerBullet");

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 6f, mask); // 半径6の範囲内をチェック
       

        foreach (var hit in hits)
        {
            Debug.Log($"タグ: {hit.tag}");
            if (hit.CompareTag("Player-bullet"))
            {
                awayDir = (transform.position - hit.transform.position).normalized;

                // ±45度の範囲で回転させて、斜めに逃げる
                float angleOffset = Random.Range(-45f, 45f);
                awayDir = Quaternion.Euler(0, 0, angleOffset) * awayDir;

                // 回避距離もランダムに
                float dodgeDistance = Random.Range(5f, 15f);
                Vector3 dodgePosition = transform.position + awayDir * dodgeDistance;

                //回避行動で画面外から出ないように
                dodgePosition.x = Mathf.Clamp(dodgePosition.x, -xLimit, xLimit);
                dodgePosition.y = Mathf.Clamp(dodgePosition.y, -yLimit, yLimit);

                agent.SetDestination(dodgePosition);

                return true;
            }
        }
        awayDir = Vector3.zero;
        return false;
    }


    /// <summary>
    /// 弾の検知範囲を可視化
    /// </summary>
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 6f); // 確認用
    }

}
