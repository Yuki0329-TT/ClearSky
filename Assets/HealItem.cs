using ClearSky;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealItem : MonoBehaviour
{
    [SerializeField] private GameObject healEffect;
    public void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.Log("ヒールあたり:" + other.tag);
        if (other.CompareTag("Player"))
        {
            PlayerManager player = other.GetComponent<PlayerManager>();
         
            if (player != null)
            {
                

                HealEffect(player.transform.position); //playerの位置にエフェクト表示
                Destroy(gameObject);
                Debug.Log($"Hp回復 : {player.currentHP}");
            }
        }

    }
    public void HealEffect(Vector3 position)
    {

        GameObject effect = Instantiate(healEffect, position, Quaternion.identity);
        Destroy(effect, 2f);
    }
}
