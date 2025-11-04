using ClearSky;
using UnityEngine;
using UnityEngine.Serialization;

[AddComponentMenu("UniBulletHell/Shot Pattern/Linear Shot")]
public class UbhLinearShot : UbhBaseShot
{
    [Header("===== LinearShot Settings =====")]
    [Range(0f, 360f), FormerlySerializedAs("_Angle")]
    public float m_angle = 180f;  // 弾の発射角度
    [FormerlySerializedAs("_BetweenDelay")]
    public float m_betweenDelay = 0.1f;  // 弾と弾の発射間隔

    [SerializeField] private FixedJoystick joystick;  // ジョイスティックを参照


    private int m_nowIndex;
    private float m_delayTimer;

   
    public override void Shot()
    {
        if (m_bulletNum <= 0 || m_bulletSpeed <= 0f)
        {
            UbhDebugLog.LogWarning(name + " Cannot shot because BulletNum or BulletSpeed is not set.", this);
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
        if (!m_shooting) return;

        m_delayTimer -= UbhTimer.instance.deltaTime;

        // joystickが無い（敵）なら固定角度で撃ち続ける
        if (joystick == null)
        {
            // 自動発射モード
            AutoFire(m_angle);
            return;
        }

        // joystick入力があるときのみ撃つ（プレイヤー用）
        if (joystick.Horizontal == 0 && joystick.Vertical == 0) return;

        //初回操作時でのずれ防止
        m_angle = Mathf.Atan2(joystick.Vertical, joystick.Horizontal) * Mathf.Rad2Deg - 90f;

        
        AutoFire(m_angle);
       

        // ジョイスティックが操作されている場合にのみ発射角度を変更
        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
        {
            // JoystickのX,Yから角度を計算して、弾の発射角度を設定 
            // なぜか90度ずれるので-90f
            m_angle = Mathf.Atan2(joystick.Vertical, joystick.Horizontal) * Mathf.Rad2Deg - 90f;
       
        }

       
        while (m_delayTimer <= 0f && m_nowIndex < m_bulletNum)
        {
            FireBullet(m_angle);  // 計算された発射角度に基づいて弾を発射
            m_delayTimer += m_betweenDelay;
            m_nowIndex++;
        }

        // すべての弾を発射したら終了
        if (m_nowIndex >= m_bulletNum)
        {
            FinishedShot();
        }
    }

    // 弾を発射するロジック
    private void FireBullet(float angle)
    {
        // 弾を取得
        UbhBullet bullet = GetBullet(transform.position);
        if (bullet == null)
        {
            FinishedShot();
            return;
        }

        // 弾を発射する
        ShotBullet(bullet, m_bulletSpeed, angle);


        // 弾の移動を開始
        bullet.UpdateMove(0f);
    }


    // 発射処理
    private void AutoFire(float angle)
    {
        while (m_delayTimer <= 0f && m_nowIndex < m_bulletNum)
        {
            FireBullet(angle);
            m_delayTimer += m_betweenDelay;
            m_nowIndex++;
        }

        if (m_nowIndex >= m_bulletNum)
        {
            FinishedShot();
        }
    }
}
