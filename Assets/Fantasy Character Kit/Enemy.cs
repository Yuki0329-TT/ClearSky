using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
// using DG.Tweening;
public class Enemy : MonoBehaviour, IDamaged
{
   public int currentHp;
   public int maxHp;
   private Animator anim;
   public Slider hpBar;
   public Text hpText;
   public int scoreValue;
   private ScoreManager sm;
    private bool alive = true;
    [SerializeField] private GameObject dieEffect;
  // [SerializeField] private Transform target;

    NavMeshAgent agent;
   
    private bool isKnockBack = false;
    // Start is called before the first frame update
    void Start()
    {
        maxHp = 2000;
        currentHp = maxHp;
        hpBar.maxValue = maxHp;
        sm = GameObject.Find("Score Manager").GetComponent<ScoreManager>();
        anim = GetComponent<Animator>();

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = 8.0f;

        var sr = GetComponentInChildren<SpriteRenderer>();
        if (sr != null)
        {
            Debug.Log($"SpriteRenderer found! Enabled: {sr.enabled}, Alpha: {sr.color.a}");
        }

        Debug.Log($"Scale: {transform.localScale}");
        // transform.DOMove(new Vector3(4, 0, 0), 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        hpBar.value = currentHp;
       // agent.SetDestination(target.position);
        // hpText.text = currentHp.ToString() + " / " + maxHp.ToString();

        if (IsBulletNearby(out Vector3 awayDir))
        {
            // 自分の位置から少しだけ離れる
            Vector3 dodgePosition = transform.position + awayDir * 2f;　　//回避場所を計算　今回は２メートル先
            agent.SetDestination(dodgePosition);　　　　　　　　　　　　　//計算場所に移動開始
          //  agent.destination = dodgePosition;
           // agent.Move();
        }
    }


    bool IsBulletNearby(out Vector3 awayDir)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 5f);  //半径５メートルのバリアを張るイメージ　　
        foreach (var hit in hits)　　　　　　　　　　　　　　　　　　　　　　　　//　バリアにタグが当たっているか検知
        {
            if (hit.CompareTag("Player-bullet"))
            {
                awayDir = (transform.position - hit.transform.position).normalized;　　//　当たっていたら弾の方向とは逆方向に避ける
                return true;
            }
        }
        awayDir = Vector3.zero;　　　　　　　　　　　　　　　　　　//　当たっていなかったら処理戻る
        return false;
    }


    public void Damaged(int damage)
    {
        if (alive == false)
        {
            return;
        }
        currentHp -= 100;  // ダメージ処理（HPを10減少）
        Debug.Log("敵がダメージを受けた！残りHP: " + currentHp);

        if(currentHp > 0)
        {
            Hurt();
        }
        else
        {
            Die();
            // HPが0以下になったら敵を破壊し、スコアを加算
            
            if (sm != null)
            {
                sm.AddScore(scoreValue);  // スコアを追加
            }
        }
    }

   
    void Hurt()
    {
        if (isKnockBack == true)
        {
            return;
        }
        anim.SetTrigger("hurt");

        isKnockBack = true;
        Invoke("CantKnockBack", 2.0f);
        Debug.Log("ノックバックしない");
    }

    void Die()
    {
        if(alive == false)
        {
            return;
        }
        DieEffect();
        anim.SetTrigger("die");
        alive = false;
    }

    public void OnAnimationEnd()
    {
        Destroy(gameObject);
    }

    void CantKnockBack()
    {
        isKnockBack = false;
    }

    public void DieEffect()
    {
        Vector3 position = transform.position;
        GameObject effect = Instantiate(dieEffect, position, Quaternion.identity);
        Destroy(effect, 2f);
    }

    //  private void OnTriggerEnter2D(Collider2D other)
    //    {
    //   if (other.gameObject.CompareTag("Player-bullet"))
    //   {
    //       Destroy(other.gameObject);
    //   }


    // }
}
