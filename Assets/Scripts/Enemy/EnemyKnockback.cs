using UnityEngine;
using UnityEngine.AI;
using System.Collections;


/// <summary>
/// 敵の被弾時にノックバックするクラス
/// NavMeshAgentを利用して位置を押し戻す
/// </summary>
public class EnemyKnockback : MonoBehaviour
{
    [SerializeField] private float knockbackPower = 2f;　　　//ノックバックの強さ
    [SerializeField] private float knockbackCooldown = 0.5f; //次にノックバックするまでのクールタイム


    private bool canKnockback = true;　　//クールダウン中かどうか
    private NavMeshAgent agent;　　　　　//移動制御用エージェント


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent)
        {
            agent.updateRotation = false;
            agent.updateUpAxis = false;
            agent.isStopped = true;
        }
    }

    /// <summary>
    /// ノックバックを試行する
    /// </summary>
    public void TryKnockback()
    {
        if (canKnockback && agent != null)
        {
            StartCoroutine(KnockbackRoutine());
        }
    }


    /// <summary>
    /// 実際のノックバック処理
    /// </summary>
    private IEnumerator KnockbackRoutine()
    {
        canKnockback = false;
        agent.isStopped = false;

        //カメラ位置から逆方向に押し戻す
        Vector3 dir = (transform.position - Camera.main.transform.position).normalized;
        agent.SetDestination(transform.position + dir * knockbackPower);


        //一定時間後に停止
        yield return new WaitForSeconds(0.5f);
        agent.isStopped = true;


        //連続で発生しないよう待機
        yield return new WaitForSeconds(knockbackCooldown);  
        canKnockback = true;
    }
}
