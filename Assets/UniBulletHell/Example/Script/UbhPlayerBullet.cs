using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
public class UbhPlayerBullet : UbhBulletSimpleSprite2d
{
    [FormerlySerializedAs("_Power")]
    public int m_power = 1;

    public int speed = 10;

    private void Update()  // 弾の挙動
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime); // 前方に移動
    }
}

