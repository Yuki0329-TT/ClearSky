using ClearSky;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private PlayerManager player;


    private void Start()
    {
        int currenHP  = player.currentHP;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Heal"))
        {
            Destroy(other.gameObject);
            player.currentHP += 100;
            Debug.Log("hp‰ñ•œ");
        }
    }
}
