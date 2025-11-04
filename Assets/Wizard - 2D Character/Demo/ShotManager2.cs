using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotManager2 : MonoBehaviour
{
    private UbhHomingShot ubhHomingShot;
    //  private PlayerManager playerManagerAttackAnim;
    private void Start()
    {
        ubhHomingShot = GetComponent<UbhHomingShot>();
        //  playerManagerAttackAnim = GetComponent<PlayerManager>();

        if (ubhHomingShot != null)
        {
            ubhHomingShot.enabled = true; // UbhLinearShot を有効化
                                          // ubhLinearShot.Shot(); // 自動的に発射開始
                                          // playerManagerAttackAnim.Attack();
        }
    }
}
