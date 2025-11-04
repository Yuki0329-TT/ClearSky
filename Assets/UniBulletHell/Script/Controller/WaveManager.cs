/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] List<GameObject> wavePrefab;
    [SerializeField, Min(0)] int waveIndex = 0;

    List<GameObject> cloneList = new List<GameObject>();    // クローンリスト
    int cloneIndex = 0;                         // クローン番号
    bool listEnd = false;                       // 終了フラグ

    void Start()
    {
        StartWave(waveIndex);
    }

    void Update()
    {
        // ウェーブの削除確認
        for (int i = 0; i < cloneList.Count; ++i)
        {
            if (cloneList[i] && cloneList[i].IsDelete())
            {
                Destroy(cloneList[i].gameObject);
            }
        }

        if (wave.Count <= 0 || IsEnd()) return;

        // ウェーブが終わった
        if (cloneList[cloneIndex].IsEnd())
        {
            // 次のウェーブへ
            NextWave();
        }
    }

    // ウェーブ開始
    void StartWave(int _index)
    {
        cloneList.Add((Wave)Instantiate(wavePrefab[_index]));
    }

    // 次のウェーブへ
    void NextWave()
    {
        if (waveIndex < (wavePrefab.Count - 1))
        {
            ++waveIndex;
            ++cloneIndex;
            StartWave(waveIndex);
        }
        else
        {
            listEnd = true;
        }
    }

    // 全てのウェーブが終了したか
    public bool IsEnd()
    {
        return listEnd;
    }
}
*/

using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Waveプレファブのリスト")]
    [SerializeField] private List<GameObject> wavePrefabs;

    private int currentWave = 0;
    private GameObject currentWaveGO = null;

    void Start()
    {
        StartNextWave();　　　//今はwave.csがないから２つ目のwaveが再生しないはず。。。
    }

    void Update()
    {
        if (currentWaveGO == null) return;

        
        Wave wave = currentWaveGO.GetComponent<Wave>();

       /* if(wave == null)
        {
            Debug.Log("次にすすめないよ");
        }*/
        if (wave != null && wave.IsEnd())
        {
            Destroy(currentWaveGO);　　　　　　　//　前のwaveが終了していたら下に進む
            StartNextWave();
        }
    }

    void StartNextWave()
    {
        if (currentWave >= wavePrefabs.Count) return;

        currentWaveGO = Instantiate(wavePrefabs[currentWave], transform);  //次のwave生成してカウント1進める
        currentWave++;
    }
}