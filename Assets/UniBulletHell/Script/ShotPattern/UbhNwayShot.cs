using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Ubh nWay shot.
/// </summary>
[AddComponentMenu("UniBulletHell/Shot Pattern/nWay Shot")]
public class UbhNwayShot : UbhBaseShot
{
    [Header("===== NwayShot Settings =====")]
   
    [FormerlySerializedAs("_WayNum")]　　　　//発射数
    public int m_wayNum = 5;
   
  [Range(0f, 360f), FormerlySerializedAs("_CenterAngle")]　　//中心角度
   public float m_centerAngle = 180f;
   
    [Range(0f, 360f), FormerlySerializedAs("_BetweenAngle")]　　　//範囲
    public float m_betweenAngle = 10f;
    
    [FormerlySerializedAs("_NextLineDelay")]　//間隔
    public float m_nextLineDelay = 0.1f;

    [SerializeField] private FixedJoystick joystick;  // ジョイスティックを参照

    public Transform player;    //プレイヤー参照

    private int m_nowIndex;
    private float m_delayTimer;

    public override void Shot()
    {
        if (m_bulletNum <= 0 || m_bulletSpeed <= 0f || m_wayNum <= 0)
        {
            UbhDebugLog.LogWarning(name + " Cannot shot because BulletNum or BulletSpeed or WayNum is not set.", this);
            return;
        }

        if (m_shooting)
        {
            return;
        }

        m_shooting = true;
        m_nowIndex = 0;
        m_delayTimer = 0f;
    }

    protected virtual void Update()
    {
        if (m_shooting == false)
        {
            return;
        }

        if (!m_shooting || joystick.Horizontal == 0 && joystick.Vertical == 0)
        {
            return;
        }

        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
        {
            m_centerAngle = Mathf.Atan2(joystick.Vertical, joystick.Horizontal) * Mathf.Rad2Deg - 90f;
            Debug.Log($"joystickの方向 : {m_centerAngle}度");
        }
        //joystickで角度変える
       


        m_delayTimer -= UbhTimer.instance.deltaTime;

        if (player == null)
        {
            UbhDebugLog.LogWarning(name + " Player not assigned.", this);
            return;
        }

        Vector2 direction = player.position - transform.position;
      


        while (m_delayTimer <= 0)　　//発射タイミングが来たら
        {
            for (int i = 0; i < m_wayNum; i++)//nwayで設定されている弾数まで充填
            {
                UbhBullet bullet = GetBullet(transform.position);
                if (bullet == null)
                {
                    break;
                }

                float baseAngle = m_wayNum % 2 == 0 ?m_centerAngle - (m_betweenAngle / 2f) : m_centerAngle;
                //　　　　　　　　偶数か判断　　　　　偶数なら弾と弾の間を　　　　　　　　奇数なら真ん中の弾を中心に合わせる
  
                float angle = UbhUtil.GetShiftedAngle(i, baseAngle, m_betweenAngle);

                ShotBullet(bullet, m_bulletSpeed, angle);

                bullet.UpdateMove(-m_delayTimer);

                m_nowIndex++;

                if (m_nowIndex >= m_bulletNum)
                {
                    break;
                }
            }

            FiredShot();

            if (m_nowIndex >= m_bulletNum)
            {
                FinishedShot();
                return;
            }

            m_delayTimer += m_nextLineDelay;
        }
    }
}