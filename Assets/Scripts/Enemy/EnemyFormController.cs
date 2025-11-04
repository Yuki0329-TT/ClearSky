using UnityEngine;
using System.Collections;


/// <summary>
/// ボスのHP割合に応じて弾幕パターン（フォーム）を切り替えるクラス
/// </summary>
public class EnemyFormController : MonoBehaviour
{
    [Header("Boss Forms")]


    public GameObject firstForm;　//HP75%以上で使用する


    public GameObject secondForm; //HP50%～75%で使用する


    public GameObject thirdForm;   //HP25%～50%で使用する


    public GameObject fourthForm;   //HP25%未満で使用する


    private int lastPatternIndex = -1;


    /// <summary>
    /// HPの割合をもとに、現在のフォームを判定して切り替える
    /// </summary>
    public void CheckFormByHP(int currentHp, int maxHp)
    {
        float rate = (float)currentHp / maxHp;
        int index = GetPatternIndex(rate);

        if (index != lastPatternIndex)
        {
            SwitchForm(index);
            lastPatternIndex = index;
        }
    }


    /// <summary>
    /// HP割合に応じてフォーム番号を返す
    /// </summary>
    private int GetPatternIndex(float rate)
    {
        if (rate > 0.75f) return 0;
        else if (rate > 0.5f) return 1;
        else if (rate > 0.25f) return 2;
        else return 3;
    }


    /// <summary>
    /// 指定されたフォームを有効化し、対応する弾発射スクリプトを起動する
    /// </summary>
    private void SwitchForm(int index)
    {
        DeactivateAll();　//全てのフォームを１度無効化


        GameObject form = index switch
        {
            0 => firstForm,
            1 => secondForm,
            2 => thirdForm,
            3 => fourthForm,
            _ => null
        };

        if (form != null)
        {
            form.SetActive(true);
            StartCoroutine(ActivateShots(form));
        }
    }


    /// <summary>
    /// 1度全てOFFにしてから再割り当て
    /// </summary>
    private void DeactivateAll()
    {
        firstForm.SetActive(false);
        secondForm.SetActive(false);
        thirdForm.SetActive(false);
        fourthForm.SetActive(false);
    }


    /// <summary>
    /// 選択されたフォーム内の全ての UbhShotCtrl を起動
    /// </summary>
    private IEnumerator ActivateShots(GameObject form)
    {
        yield return null;
        foreach (var ctrl in form.GetComponentsInChildren<UbhShotCtrl>(true))
        {
            ctrl.enabled = true;
            ctrl.StartShotRoutine();
        }
    }
}
