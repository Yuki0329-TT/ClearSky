using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵の出現タイミングを管理するクラス
/// 
/// ・あらかじめ登録した敵（WaveObj）を generateFrame に応じて順番に出現させる
/// ・ステージ中の「ウェーブ制御」として機能
/// 
/// 主に StageManager から呼ばれ、特定の構成に沿って敵が出現する流れを演出する
/// </summary>
public class Wave : MonoBehaviour
{
    [System.Serializable]
    public struct ObjData
    {
        public int generateFrame;　//敵を出現させるフレーム数


        public WaveObj waveObj;　　//出現対象
    }

    [SerializeField] private List<ObjData> objList;


    private int nowObj = 0;
    private int frame = 0;
    private bool waveEnd = false;

    private void Start()
    {
        // ゲーム開始時は全敵を非表示（生成タイミングまで待機）
        foreach (var obj in objList)
        {
            if (obj.waveObj != null)
            {
                obj.waveObj.gameObject.SetActive(false);
            }
        }
    }


    private void Update()
    {
        if (objList.Count <= 0 || waveEnd) return;


        frame++;


        //現在のフレームに達した敵を出現させる
        while (nowObj < objList.Count && frame >= objList[nowObj].generateFrame)
        {
            if (objList[nowObj].waveObj != null)
            {
                objList[nowObj].waveObj.gameObject.SetActive(true);        //waveが再生されるまでの時間
            }


            nowObj++;


            //すべて出現したらウェーブ終了
            if (nowObj >= objList.Count)
            {
                waveEnd = true;
                break;
            }
        }
    }


    /// <summary>
    /// すべての敵が行動を完了していればウェーブ終了
    /// </summary>
    public bool IsEnd()
    {
        foreach (var obj in objList)
        {
            if (obj.waveObj != null && !obj.waveObj.IsEnd())　　　//waveが終了か
            {
                return false;
            }
        }
        return waveEnd;
    }
}
