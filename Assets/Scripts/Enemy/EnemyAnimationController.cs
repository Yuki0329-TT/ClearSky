using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spineアニメーションを用いた敵の被弾・死亡演出を管理するクラス
/// </summary>
public class EnemyAnimationController : MonoBehaviour
{
    [SerializeField] private SkeletonAnimation skeletonAnimation;     //Spineアニメーション本体
    [SerializeField] private string dieAnimation = "Dead";       //死亡アニメーション名


    /// <summary>
    /// 被弾時のエフェクト再生（赤色点滅）
    /// </summary>
    public void PlayHitEffect()
    {
        StartCoroutine(HitFlash());
    }


    /// <summary>
    /// 被弾した時赤く点滅してダメージ演出を行う
    /// </summary>
    private IEnumerator HitFlash()　　
    {
        Color original = skeletonAnimation.Skeleton.GetColor();
        skeletonAnimation.Skeleton.SetColor(Color.red);
        yield return new WaitForSeconds(0.1f);
        skeletonAnimation.Skeleton.SetColor(original);
    }


    /// <summary>
    /// 死亡アニメーションを再生する
    /// </summary>
    public void PlayDeathAnimation()
    {
        skeletonAnimation.AnimationState.SetAnimation(0, dieAnimation, false);
    }
}
