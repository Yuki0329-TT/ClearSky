using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// ステージクリア後に表示されるアイテムショップのUI管理クラス
/// ランダムにアイテムを抽選し、UI上に生成・配置する
/// </summary>
public class ItemShopManager : MonoBehaviour
{
    public GameObject itemUIPrefab;   //アイテムを表示する為の入れ物
    public Transform itemGridParent;　//アイテムを置く場所


    public List<ItemData> allItemData;//抽選対象のデータ


    public static ItemShopManager instance;


    private int slotNumber = 3;                                     //同時に表示するアイテム
    private List<GameObject> currentItems = new List<GameObject>(); // 生成済みのアイテムUIを管理


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    void Start()
    {
         GenerateRandomItems();
    }


    /// <summary>
    /// ランダムなアイテムを抽選してUIに生成する
    /// </summary>
    public void GenerateRandomItems()　　
    {
        Debug.Log("GenerateRandomItems呼ばれた");


        //バグ起きないよう既存のものを削除
        foreach (var item in currentItems)
        {
            Destroy(item);　　　　　　　
        }
        currentItems.Clear();


        //抽選候補をコピー
        var candidates = new List<ItemData>(allItemData);

      
        for (int i = 0; i < slotNumber; i++)
        {

            if (candidates.Count == 0)
            {
                Debug.LogWarning("アイテム候補がなくなった");
                break;
            }

                
            //データからランダムに1つ選んでUI生成
            int index = Random.Range(0, candidates.Count);
            ItemData item = candidates[index];
            candidates.RemoveAt(index);

            
            var itemUI = Instantiate(itemUIPrefab, itemGridParent);
            
            itemUI.GetComponent<ItemPickUI>().SetUp(item);

            currentItems.Add(itemUI);

            Debug.Log($"[{i}] {item.name} を生成 parent={itemUI.transform.parent.name}");
        }
        Debug.Log($"最終 ChildCount = {itemGridParent.childCount}");
    }


}
