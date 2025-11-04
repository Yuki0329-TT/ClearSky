/*using ClearSky;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private int shotIndex;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {

            Debug.Log("アイテム取得");  //アイテム取得はできてる　　下反応なし
            ShotManager shotManager = other.GetComponentInChildren<ShotManager>();  //衝突したものから親子含め探す
            if (shotManager != null)
            {
                shotManager.SetCurrentShot(shotIndex);
                Debug.Log($"弾を{shotIndex}番に変更したよ！");
            }

            else
            {
                Debug.Log("shotManager開けてない");
            }
                Destroy(gameObject);
        }
    }
}
*/