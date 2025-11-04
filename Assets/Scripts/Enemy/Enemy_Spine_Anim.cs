using System.Collections;
using UnityEngine;
using Spine.Unity;
using DG.Tweening;
using UnityEngine.AI;
using Spine;


/// <summary>
/// 【EnemyCoreの旧型（Boss専用）】
/// 
/// ・敵キャラ（特にボス）のSpineアニメーション、HP管理、弾幕切替、死亡処理をまとめて担当するクラス
/// ・現在はEnemyCore等に機能分割済みだが、BossでEnemyCoreを使用すると１発で死んでしまう不具合があるため、
///   一時的にこの安定版をボス専用として使用中
/// 
/// 【今後の方針】
/// ・EnemyCore＋EnemyFormControllerへの統合を予定
/// </summary>
public class Enemy_Spine_Anim : MonoBehaviour, IDamaged
{
    [SerializeField] private SkeletonAnimation skeletonAnimation;
    [SerializeField] private string dieAnimation = "Dead";
    [SerializeField] private float dieDuration = 1.0f;
    [SerializeField] private GameObject dieEffect;


    public int scoreValue;
    private ScoreManager sm;


    private NavMeshAgent agent;
    private bool isDying = false;
    private bool canKnockback = true;
    [SerializeField] private float knockbackCooldown = 0.5f;

    private Vector3 originalPosition;
    private Tween moveTween;

    public int maxHp;
    private int currentHp;

    private UbhShotCtrl shotCtrl;
    public bool isBoss = false;

    public GameObject firstForm;
    public GameObject secondForm;
    public GameObject thirdForm;
    public GameObject fourForm;

   
    private int lastPatternIndex = -1;

  
    void Start()
    {
        if (currentHp <= 0)
        {
            currentHp = maxHp;
        }

        moveTween = GetComponent<DOTweenPath>()?.GetTween();
        sm = GameObject.Find("Score Manager")?.GetComponent<ScoreManager>();
        shotCtrl = GetComponent<UbhShotCtrl>();
        agent = GetComponent<NavMeshAgent>();
        originalPosition = transform.position;

        if (agent != null)
        {
            agent.updateRotation = false;
            agent.updateUpAxis = false;
            agent.isStopped = true;
        }
    }


    void Update()
    {
        originalPosition = transform.position;

        // HPに応じたボス形態・弾幕切替
        if (isBoss && shotCtrl != null)
        {
            int patternIndex = GetAttackPatternIndex();
            if (patternIndex != lastPatternIndex)
            {
                SwitchToForm(patternIndex);
                lastPatternIndex = patternIndex;
            }
        }
    }

    // 形態切替時に対応フォームを有効化
    void SwitchToForm(int index)
    {
        DeactivateAllForms();

        GameObject form = null;
        switch (index)
        {
            case 0: form = firstForm; break;
            case 1: form = secondForm; break;
            case 2: form = thirdForm; break;
            case 3: form = fourForm; break;
        }

        if (form != null)
        {
            form.SetActive(true);
            StartCoroutine(StartShotAfterDelay(form));
            StartCoroutine(StartShotForcefully(form));
        }
    }

    // ダメージ処理（ノックバック、HP減少、死亡処理）
    public void Damaged(int damage)
    {
        if (isDying) return;

        currentHp -= damage;

        if (canKnockback)
        {
            StartCoroutine(HitEffect());
            StartCoroutine(KnockbackEffect());
        }

        if (currentHp > 0) return;

        isDying = true;

        if (moveTween != null)
        {
            moveTween.Kill();
        }

        Vector3 position = transform.position;
        GameObject effect = Instantiate(dieEffect, position, Quaternion.identity);
        Destroy(effect, 2f);

        skeletonAnimation.AnimationState.SetAnimation(0, dieAnimation, false);

        if (sm != null)
        {
            sm.AddScore(scoreValue);
        }

        DOVirtual.DelayedCall(dieDuration, () =>
        {
            gameObject.SetActive(false);
        });
    }

    // ヒット時に赤く点滅させる
    private IEnumerator HitEffect()
    {
        Color originalColor = skeletonAnimation.Skeleton.GetColor();
        skeletonAnimation.Skeleton.SetColor(Color.red);
        yield return new WaitForSeconds(0.1f);
        skeletonAnimation.Skeleton.SetColor(originalColor);
    }

    // ノックバックエフェクト（DOTween）
    private IEnumerator KnockbackEffect()
    {
        canKnockback = false;

        if (agent != null)
        {
            agent.isStopped = false;
            Vector3 knockbackDir = (transform.position - Camera.main.transform.position).normalized;
            Vector3 knockbackPos = transform.position + knockbackDir * 2f;
            agent.SetDestination(knockbackPos);
            yield return new WaitForSeconds(0.5f);
            agent.isStopped = true;
        }

        yield return new WaitForSeconds(knockbackCooldown);
        canKnockback = true;
    }

    // すべてのフォームを非アクティブ化
    void DeactivateAllForms()
    {
        firstForm.SetActive(false);
        secondForm.SetActive(false);
        thirdForm.SetActive(false);
        fourForm.SetActive(false);
    }

    // 現在のHP割合から弾幕パターンを返す
    public int GetAttackPatternIndex()
    {
        if (!isBoss || shotCtrl == null) return 0;

        float rate = (float)currentHp / maxHp;

        if (rate > 0.75f) { shotForm1(); return 0; }
        else if (rate > 0.5f) { shotForm2(); return 1; }
        else if (rate > 0.25f) { shotForm3(); return 2; }
        else { shotForm4(); return 3; }
    }

    public void SetHP(int hp, bool isBossFlag)
    {
        maxHp = hp;
        currentHp = hp;
        isBoss = isBossFlag;
    }

    void shotForm1()
    {
        DeactivateAllForms();
        firstForm.SetActive(true);
        foreach (Transform child in firstForm.transform)
        {
            child.gameObject.SetActive(true);
        }
        StartCoroutine(StartShotForcefully(firstForm));
        StartCoroutine(StartShotAfterDelay(firstForm));
    }

    void shotForm2()
    {
        firstForm.SetActive(false);
        secondForm.SetActive(true);
    }

    void shotForm3()
    {
        secondForm.SetActive(false);
        thirdForm.SetActive(true);
    }

    void shotForm4()
    {
        thirdForm.SetActive(false);
        fourForm.SetActive(true);
    }


    // 弾幕発射を1フレーム遅らせて実行
    private IEnumerator StartShotAfterDelay(GameObject form)
    {
        yield return null;
        UbhShotCtrl ctrl = form.GetComponentInChildren<UbhShotCtrl>();
        if (ctrl != null)
        {
            ctrl.enabled = true;
            ctrl.StartShotRoutine();
        }
    }


    private IEnumerator StartShotForcefully(GameObject form)
    {
        yield return null;
        UbhBaseShot[] shots = form.GetComponentsInChildren<UbhBaseShot>(true);
        foreach (var shot in shots)
        {
            if (shot != null && shot.isActiveAndEnabled)
            {
                shot.Shot();
            }
        }
    }
}
