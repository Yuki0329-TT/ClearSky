using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// アイテムの購入履歴と価格変動を管理するクラス
/// 各アイテムの購入回数に応じて価格を上昇させる
/// </summary>
public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;


    //購入回数を記録する辞書
    private Dictionary<string, int> purchaseCounts = new Dictionary<string, int>();


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
    /// 指定したアイテムの購入回数を取得
    /// </summary>
    public int GetPurchaseCount(string itemName)
    {
        if (!purchaseCounts.ContainsKey(itemName))
            purchaseCounts[itemName] = 0;

        return purchaseCounts[itemName];
    }


    /// <summary>
    /// 指定したアイテムの購入回数を+1する
    /// </summary>
    public void IncrementPurchaseCount(string itemName)
    {
        if (!purchaseCounts.ContainsKey(itemName))
            purchaseCounts[itemName] = 0;

        purchaseCounts[itemName]++;
    }


    /// <summary>
    /// 購入回数に応じて商品価格の設定
    /// 数が変にならないように四捨五入
    /// </summary>
    public float GetCurrentPrice(ItemData item)
    {
        int count = GetPurchaseCount(item.itemName);
        return Mathf.Round(item.requiredScore * Mathf.Pow(1.5f, count));
    }
}
