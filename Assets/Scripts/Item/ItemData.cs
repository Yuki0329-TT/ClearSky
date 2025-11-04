using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 各アイテムの基本情報を保持するデータクラス
/// ScriptableObjectとして管理し、エディタ上で個別設定可能
/// </summary>
[CreateAssetMenu(fileName = "ItemData", menuName = "Item/ItemData")]
public class ItemData : ScriptableObject
{
    [Header("基本情報")]
    public string itemName;　　　　　　　　// アイテム名
    public Sprite icon;　　　　　　　　　　//表示用アイコン
    public ItemEffectType effectType;　　　//効果タイプ

    [Header("購入条件")]
    public int requiredScore;　　　　　　　// 購入に必要なスコア

}
