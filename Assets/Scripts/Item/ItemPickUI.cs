using ClearSky;
using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// アイテムの情報をUIスロットに反映し、購入処理を制御するクラス
/// </summary>
public class ItemPickUI : MonoBehaviour
{
    public Image iconImage;      //アイテムアイコン
    public Text nameText;        //アイテム名
    public Button buyButton;     //購入ボタン
    public Text amountText;　　　//購入スコア表示

    private ItemData currentItem;　//表示中のアイテムの情報
    private bool buy = false;



    /// <summary>
    /// アイテムデータを受け取って、UI要素を初期化する
    /// </summary>
    public void SetUp(ItemData itemData)
    {
        currentItem = itemData;
        iconImage.sprite = itemData.icon;
        nameText.text = itemData.itemName;


        float displayPrice = ItemManager.Instance.GetCurrentPrice(itemData);
        amountText.text = displayPrice.ToString();

       
        buy = false;
        buyButton.interactable = true;

        float playerScore = ScoreManager.instance.score;

        //playerスコアがアイテムの購入必要スコアより大きいかどうか
        if (playerScore < displayPrice)
        {
            buyButton.interactable = false;
        }
        else
        {
            buyButton.interactable = true;
        }
         

        
        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(OnBuyClicked);
    }


    /// <summary>
    /// 購入ボタン押したときの処理
    /// </summary>
    private void OnBuyClicked()
    {
        if (buy) return;　　　//二重購入防止
        buy = true;
        buyButton.interactable = false;


        float price = ItemManager.Instance.GetCurrentPrice(currentItem);

    
        //スコアが足りているかチェック
        if (ScoreManager.instance.score >= price)
        {

            //スコア減算
            ScoreManager.instance.score -= price;
            　

            //  購入カウントを増やす
            ItemManager.Instance.IncrementPurchaseCount(currentItem.itemName);　　　　


            //アイテムの効果を適用
            GameManager.Instance.ApplyEffect(currentItem.effectType);


            Debug.Log($"購入:{currentItem.itemName} (価格: {price})");


            //新しいアイテムを生成
            ItemShopManager.instance.GenerateRandomItems();
        }


        //次のシーンに遷移
        if (GameClearSceneManager.instance != null)
        {
            GameClearSceneManager.instance.GameNextScene();
        }
        else
        {
            Debug.LogWarning("GameClearSceneManager が見つかりません。");
        }

           
    }
}
