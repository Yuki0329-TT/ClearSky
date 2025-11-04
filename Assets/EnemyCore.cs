using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵キャラクターのHP管理・ダメージ処理・死亡演出を統括するクラス
/// 通常敵／ボスどちらにも対応する
/// </summary>
[RequireComponent(typeof(EnemyAnimationController))]
[RequireComponent(typeof(EnemyScoreHandler))]
[RequireComponent(typeof(EnemyKnockback))]
public class EnemyCore : MonoBehaviour,IDamaged
{
    [SerializeField] private bool isBoss = false; // ボスかどうか
    [SerializeField] private int maxHp;           //　最大Hp
    [SerializeField] private int currentHp;       //　現在のHp


    private EnemyAnimationController anim;        //アニメーション制御
    private EnemyScoreHandler scoreHandler;       //スコア加算処理
    private EnemyKnockback knockback;             //ノックバック処理
    private EnemyFormController formController;   //形態変化(任意)


    [SerializeField] private GameObject dieEffect;    //死亡エフェクト
    [SerializeField] private float dieDuration = 1f;  //消滅するまでの時間


    private bool isDying = false;                      //死亡中フラグ
    private bool isInvincible = false;                 //一時無敵
    [SerializeField] private float hitCooldown = 0.3f; // 弾の多段Hit対策


    void Awake()
    {
        currentHp = maxHp;
        anim = GetComponent<EnemyAnimationController>();
        scoreHandler = GetComponent<EnemyScoreHandler>();
        knockback = GetComponent<EnemyKnockback>();
        formController = GetComponent<EnemyFormController>();

        Debug.Log("現在のhpがMaxになったはず");
    }


    /// <summary>
    /// 敵の初期化（ボス／雑魚の区別設定など）
    /// </summary>
    public void Initialize(bool bossFlag)
    {
        isBoss = bossFlag;
        Debug.Log($"敵初期化: isBoss = {isBoss}");
    }


    void Start()
    {
        if (currentHp <= 0)
        {
            currentHp = maxHp;
        }
        isDying = false;
    }


    /// <summary>
    /// ダメージ処理（被弾、ノックバック、死亡判定）
    /// </summary>
    public void Damaged(int damage)
    {
        if (isDying || isInvincible) return;


        currentHp -= damage;
        Debug.Log($"[EnemyCore] {gameObject.name} がダメージを受けた (残HP: {currentHp})");

        anim.PlayHitEffect();
        knockback.TryKnockback();

        //Hpの割合に応じて形態変化(存在する場合のみ)
        formController?.CheckFormByHP(currentHp, maxHp);


        if (currentHp <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(InvincibleCoroutine());
        }
    }


    /// <summary>
    /// 無敵時間処理（多段ヒット対策）
    /// </summary>
    private IEnumerator InvincibleCoroutine()
    {
        isInvincible = true;
        yield return new WaitForSeconds(hitCooldown);
        isInvincible = false;
    }


    /// <summary>
    /// 死亡処理(スコア加算、死亡演出)
    /// </summary>
    private void Die()
    {
        if (isDying) return;
        isDying = true;


        Debug.Log($"[Die呼び出し] {gameObject.name} のHP: {currentHp}");


        anim.PlayDeathAnimation();
        scoreHandler.AddScore();


        //エフェクト生成と削除
        if (dieEffect)
        {
            //Hpが0になったらdieEffectを再生して2f後に削除
            var eff = Instantiate(dieEffect, transform.position, Quaternion.identity);　　
            Destroy(eff, 2f);
        }


        //遅延削除(DOTweenを仕様)
        DOVirtual.DelayedCall(dieDuration, () =>
        {
            gameObject.SetActive(false);
        });
    }
}
