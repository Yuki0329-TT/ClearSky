using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    public int maxHp;
    private int currentHp;
    public int scoreValue;
    private bool isDying = false;

    private EnemyAnimationController animController;
    private EnemyDeathEffect deathEffect;

    private void Start()
    {
        currentHp = maxHp;
        animController = GetComponent<EnemyAnimationController>();
        deathEffect = GetComponent<EnemyDeathEffect>();
    }

    public void Damaged(int damage)
    {
        if (isDying) return;

        currentHp -= damage;
        animController?.PlayHitEffect();

        if(currentHp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDying = true;
        animController?.PlayDeathAnimation();
        deathEffect?.SpawnEffect(transform.position);

        ScoreManager.instance?.AddScore(scoreValue);
        gameObject.SetActive(false);
    }
}
