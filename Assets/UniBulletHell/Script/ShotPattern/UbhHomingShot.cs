using ClearSky;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

[AddComponentMenu("UniBulletHell/Shot Pattern/Homing Shot")]
public class UbhHomingShot : UbhBaseShot
{
    [Header("===== HomingShot Settings =====")]
    [FormerlySerializedAs("_BetweenDelay")]
    public float m_betweenDelay = 0.1f;
    [FormerlySerializedAs("_HomingAngleSpeed")]
    public float m_homingAngleSpeed = 20f;
    [FormerlySerializedAs("_SetTargetFromTag")]
    public bool m_setTargetFromTag = true;
    [FormerlySerializedAs("_TargetTagName"), UbhConditionalHide("m_setTargetFromTag")]
    public string m_targetTagName = "Player";
    [UbhConditionalHide("m_setTargetFromTag")]
    public bool m_randomSelectTagTarget;
    [UbhConditionalHide("m_setTargetFromTag")]
    public bool m_nearestSelectTagTarget;
    [FormerlySerializedAs("_TargetTransform")]
    public Transform m_targetTransform;

    private int m_nowIndex;
    private float m_delayTimer;

    // プレイヤーの参照をキャッシュ
    private PlayerManager player;

    private void Start()
    {
        player = FindObjectOfType<PlayerManager>();  // プレイヤーを一度だけ取得
    }

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

        // プレイヤーが存在し、HPが0以下なら弾を撃たない
        if (player == null || player.currentHP <= 0)
        {
            return;
        }

        m_shooting = true;
        m_nowIndex = 0;
        m_delayTimer = 0f;
    }

    public override void FinishedShot()
    {
        if (m_setTargetFromTag)
        {
            m_targetTransform = null;
        }
        base.FinishedShot();
    }

    private void Update()
    {
        if (!m_shooting)
        {
            return;
        }

        m_delayTimer -= UbhTimer.instance.deltaTime;

        if (m_delayTimer <= 0)
        {
            if (m_targetTransform == null && m_setTargetFromTag)
            {
                m_targetTransform = UbhUtil.GetTransformFromTagName(m_targetTagName, m_randomSelectTagTarget, m_nearestSelectTagTarget, transform);
            }
        }

        while (m_delayTimer <= 0)
        {
            UbhBullet bullet = GetBullet(transform.position);
            if (bullet == null)
            {
                FinishedShot();
                return;
            }

            float angle = UbhUtil.GetAngleFromTwoPosition(transform, m_targetTransform, shotCtrl.m_axisMove);

            ShotBullet(bullet, m_bulletSpeed, angle, true, m_targetTransform, m_homingAngleSpeed);

            bullet.UpdateMove(-m_delayTimer);

            m_nowIndex++;

            FiredShot();

            if (m_nowIndex >= m_bulletNum)
            {
                FinishedShot();
                return;
            }

            m_delayTimer += m_betweenDelay;
        }
    }
}
