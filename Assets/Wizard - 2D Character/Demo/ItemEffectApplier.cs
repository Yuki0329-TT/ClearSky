using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// アイテムの効果をゲーム内の実際の挙動に反映するクラス
/// 
/// ItemData（効果種別）を受け取り、対応する処理を GameManager や各システムに指示する
/// 例：スピード上昇、HP回復、弾幕変更、シールド展開、スコア倍率上昇など
/// </summary>
public static class ItemEffectApplier
{
    /// <summary>
    /// ItemEffectType に応じた効果を適用
    /// </summary>
    public static void ApplyEffect(ItemEffectType type )
    {
        switch (type)
        {
            case ItemEffectType.LinearWay:　　　//通常弾に切り替え
                ChangeBullet(0);
                break;


            case ItemEffectType.ThreeWay:　　　　//３方向弾に切り替え
                ChangeBullet(1);
                break;


            case ItemEffectType.SpeedUp:　　　　　//スピードアップ
                SpeedUp(5.0f);
                break;

            case ItemEffectType.Heal:　　　　　　　//HP回復
                HealHp(600);
                break;


            case ItemEffectType.Shield:　　　　　　//シールド展開
                Shield();
                break;

            case ItemEffectType.ScoreUp:　　　　　　//獲得スコアアップ
                ScoreUp();
                break;
        }
    }


    /// <summary>
    /// プレイヤーの発射弾を切り替え
    /// </summary>
    private static void ChangeBullet(int index)
    {
       if (GameManager.Instance != null)    
        {
            var shotCtrl = GameManager.Instance.playerShotCtrl.GetComponent<UbhShotCtrl>();
            if (shotCtrl != null)
            {
                shotCtrl.SetPatternIndex(index);
                Debug.Log($"弾変更: Index = {index}");
            }
        }
    }
    

    /// <summary>
    /// プレイヤーの移動速度を上昇させる
    /// </summary>
    private static void SpeedUp(float amount)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.playerManager.IncreaseSpeed(5.0f);
            Debug.Log("スピードアップ");
        }
        
    }


    /// <summary>
    /// プレイヤーのHPを回復
    /// </summary>
    private static void HealHp(int amount)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.playerManager.Heal(600);
        }
        Debug.Log("Hp回復");
    }


    /// <summary>
    /// プレイヤーの周りにシールドを展開する
    /// </summary>
    private static void Shield()
    {
      
        Debug.Log(" Shield() 呼ばれた");

        if (Status.Instance != null)
        {
            Status.Instance.ActivateShield();
            Debug.Log("シールド展開");
        }
        else
        {
            Debug.Log(" Status.Instance = null");
        }
    }


    /// <summary>
    /// 獲得スコアの倍率を上昇させる
    /// </summary>
    private static void ScoreUp()
    {
        if(ScoreManager.instance != null)
        {
            ScoreManager.instance.scoreMultiplier += 0.5f;  //0.5倍ずつ上がる
            Debug.Log("スコアアップ");
        }
        
    }
}
