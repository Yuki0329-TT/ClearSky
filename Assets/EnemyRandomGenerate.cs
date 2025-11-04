using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRandomGenerate : MonoBehaviour
{
    [SerializeField] List<GameObject> enemyList;　　　　　　//生成オブジェクト
    [SerializeField] Transform pos;　　　　　　　　　　　//生成位置
    [SerializeField] Transform pos2;
    float minX, maxX, minY, maxY;          //生成範囲

    int frame = 0;
    [SerializeField] int generateFrame = 30;  //生成する間隔

    private void Start()
    {
        minX = Mathf.Min(pos.position.x, pos2.position.x);　　　　　//それぞれの位置を体格で結び、四角形を作りそれを範囲としている
        maxX = Mathf.Max(pos.position.x, pos2.position.x);
        minY = Mathf.Min(pos.position.y, pos2.position.y);
        maxY = Mathf.Max(pos.position.y, pos2.position.y);

    }

    private void Update()
    {
        ++frame;

        if (frame > generateFrame)　　　
        {
            frame = 0;      //frameリセット

            int index = Random.Range(0, enemyList.Count);
            float posX = Random.Range(minX, maxX);
            float posY = Random.Range(minY, maxY);

            Instantiate(enemyList[index], new Vector3(posX, posY, 0), Quaternion.identity);
        }
    }
}
