using ObjectPool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickShooter : MonoBehaviour
{
    public FixedJoystick joystick;
  //  public MyObjectPool objectPool;
    public Transform firepoint;  // 発射位置
    public float betweenDelay = 0.2f;  // 発射間隔
    private float fireTimer = 0f;  // タイマー

    private void Update()
    {
        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
        {
            fireTimer += Time.deltaTime;

            if (fireTimer > betweenDelay)
            {
                Fire();
                fireTimer = 0f;
            }
        }
    }

    private void Fire()
    {
        // プールから `Bullet` を取得
     /*   UbhBullet bullet = UbhObjectPool.instance.GetBullet(bulletPrefab, firepoint, false);
       if(bullet == null)
        {
            Debug.Log("弾の取得に失敗しました");
            return;
        }

        // 発射位置と向きを設定
        bullet.transform.position = firepoint.position;
        bullet.transform.rotation = Quaternion.LookRotation(Vector3.forward, new Vector3(joystick.Horizontal, joystick.Vertical, 0));

       /* // 弾にオブジェクトプールを設定
        bullet.SetPool(objectPool);*/
    }
}
