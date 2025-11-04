using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

/// <summary>
/// Ubh shot ctrl (UniBulletHell標準設計に基づく).
/// 複数の弾パターン(m_shotList)を管理する。
/// </summary>
[AddComponentMenu("UniBulletHell/Controller/Shot Controller")]
public sealed class UbhShotCtrl : UbhMonoBehaviour
{
    [Serializable]
    public class ShotInfo
    {
        // 弾パターン (UbhBaseShotを継承したもの)
        [FormerlySerializedAs("_ShotObj")]
        public UbhBaseShot m_shotObj;

        // このパターン発射後のディレイ (sec)
        [FormerlySerializedAs("_AfterDelay")]
        public float m_afterDelay;
    }

    public UbhUtil.AXIS m_axisMove = UbhUtil.AXIS.X_AND_Y;
    public bool m_inheritAngle = false;

    public bool m_startOnAwake = true;
    public float m_startOnAwakeDelay = 1f;

    public bool m_startOnEnable = true;
    public float m_startOnEnableDelay = 1f;

    public bool m_loop = true;
    public bool m_atRandom = false;

    [FormerlySerializedAs("_ShotList")]
    public List<ShotInfo> m_shotList = new List<ShotInfo>();

    [Space(10)]
    public UnityEvent m_shotRoutineFinishedCallbackEvents = new UnityEvent();

    private bool m_shooting;
    private int m_nowIndex;
    private float m_delayTimer;
    private List<ShotInfo> m_randomShotList = new List<ShotInfo>(32);

    private UbhBaseShot currentShot;
    private int currentIndex = 0;

    private enum UpdateStep
    {
        StartDelay,
        StartShot,
        WaitDelay,
        UpdateIndex,
        FinishShot,
    }
    private UpdateStep m_updateStep;

    private void Awake()
    {
        if (m_shotList.Count > 0)
        {
            SetPatternIndex(currentIndex);
        }
    }

    private void OnEnable()
    {
        UbhShotManager.instance.AddShot(this);

        if (m_startOnEnable)
        {
            StartShotRoutine(m_startOnEnableDelay);
        }
    }

    private void OnDisable()
    {
        m_shooting = false;
        if (UbhShotManager.instance != null)
        {
            UbhShotManager.instance.RemoveShot(this);
        }
    }

    public void UpdateShot(float deltaTime)
    {
        if (!m_shooting) return;

        if (m_updateStep == UpdateStep.StartDelay)
        {
            if (m_delayTimer > 0f)
            {
                m_delayTimer -= deltaTime;
                return;
            }
            m_updateStep = UpdateStep.StartShot;
        }

        ShotInfo nowShotInfo = m_atRandom ? m_randomShotList[m_nowIndex] : m_shotList[m_nowIndex];

        if (m_updateStep == UpdateStep.StartShot)
        {
            if (nowShotInfo.m_shotObj != null)
            {
                nowShotInfo.m_shotObj.SetShotCtrl(this);
                nowShotInfo.m_shotObj.Shot();
            }
            m_delayTimer = 0f;
            m_updateStep = UpdateStep.WaitDelay;
        }

        if (m_updateStep == UpdateStep.WaitDelay)
        {
            if (nowShotInfo.m_afterDelay > 0 && nowShotInfo.m_afterDelay > m_delayTimer)
            {
                m_delayTimer += deltaTime;
            }
            else
            {
                m_delayTimer = 0f;
                m_updateStep = UpdateStep.UpdateIndex;
            }
        }

        if (m_updateStep == UpdateStep.UpdateIndex)
        {
            if (m_atRandom)
            {
                m_randomShotList.RemoveAt(m_nowIndex);
                if (m_loop && m_randomShotList.Count <= 0)
                {
                    m_randomShotList.AddRange(m_shotList);
                }
                if (m_randomShotList.Count > 0)
                {
                    m_nowIndex = UnityEngine.Random.Range(0, m_randomShotList.Count);
                    m_updateStep = UpdateStep.StartShot;
                }
                else
                {
                    m_updateStep = UpdateStep.FinishShot;
                }
            }
            else
            {
                if (m_loop || m_nowIndex < m_shotList.Count - 1)
                {
                    m_nowIndex = (int)Mathf.Repeat(m_nowIndex + 1f, m_shotList.Count);
                    m_updateStep = UpdateStep.StartShot;
                }
                else
                {
                    m_updateStep = UpdateStep.FinishShot;
                }
            }
        }

        if (m_updateStep == UpdateStep.FinishShot)
        {
            m_shooting = false;
            m_shotRoutineFinishedCallbackEvents.Invoke();
        }
    }

    // --- API ---
    public void SetPatternIndex(int index)
    {
        if (index < 0 || index >= m_shotList.Count) return;

        foreach (var shotInfo in m_shotList)
        {
            if (shotInfo.m_shotObj != null)
            {
                shotInfo.m_shotObj.StopShot();
                shotInfo.m_shotObj.gameObject.SetActive(false);
            }
        }

        currentIndex = index;
        currentShot = m_shotList[currentIndex].m_shotObj;

        if (currentShot != null)
        {
            currentShot.gameObject.SetActive(true);
        }
    }

    public void StartShot()
    {
        if (currentShot != null) currentShot.Shot();
    }

    public void StopShot()
    {
        if (currentShot != null) currentShot.StopShot();
    }

    public void StartShotRoutine(float startDelay = 0f)
    {
        if (m_shotList == null || m_shotList.Count <= 0)
        {
            Debug.LogWarning($"{name} ShotList が空です。");
            return;
        }

        m_shooting = true;
        m_delayTimer = startDelay;
        m_updateStep = m_delayTimer > 0f ? UpdateStep.StartDelay : UpdateStep.StartShot;

        if (m_atRandom)
        {
            m_randomShotList.Clear();
            m_randomShotList.AddRange(m_shotList);
            m_nowIndex = UnityEngine.Random.Range(0, m_randomShotList.Count);
        }
        else
        {
            m_nowIndex = 0;
        }
    }

    public void StopShotRoutine()
    {
        m_shooting = false;
    }

    public void StopShotRoutineAndPlayingShot()
    {
        m_shooting = false;
        foreach (var shotInfo in m_shotList)
        {
            if (shotInfo.m_shotObj != null)
            {
                shotInfo.m_shotObj.FinishedShot();
            }
        }
    }
}
