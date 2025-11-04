using ClearSky;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲーム全体の進行管理およびプレイヤー状態を統括するクラス
/// 
/// ・プレイヤーの弾幕（UbhShotCtrl）制御
/// ・アイテム効果の適用
/// 
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("プレイヤー状態")]
    public PlayerManager playerManager;
    public UbhShotCtrl playerShotCtrl;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        else
        {
            Destroy(gameObject);
        }
    }


    /// <summary>
    /// playerの弾変更
    /// </summary>
    public void ChangeBullet(int index)
    {
        if (playerShotCtrl != null)
        {
            playerShotCtrl.SetPatternIndex(index);
        }
    }


    /// <summary>
    /// アイテム効果を適用する呼び出し口
    /// </summary>
    public void ApplyEffect(ItemEffectType type)
    {
        ItemEffectApplier.ApplyEffect(type);
        UpdateUI();
    }
    

    /// <summary>
    /// プレイヤーの情報確認
    /// </summary>
    private void UpdateUI()
    {
        Debug.Log($"HP:{playerManager.currentHP},  SPD:{playerManager.moveSpeed},");
    }

  
}
