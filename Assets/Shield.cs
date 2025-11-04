using ClearSky;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{

    [SerializeField] private GameObject shieldEffect;

    public void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.Log("ヒールあたり:" + other.tag);
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Status>().ActivateShield();

            PlayerManager player = other.GetComponent<PlayerManager>();
            if (player != null)
            {
               

                ShieldEffect(player.transform.position); //playerの位置にエフェクト表示
                Destroy(gameObject);
            }

        }

    }
    public void ShieldEffect(Vector3 position)
    {

        GameObject effect = Instantiate(shieldEffect, position, Quaternion.identity);
        Destroy(effect, 2f);
    }
}
