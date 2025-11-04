using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//背景を動かすクラス
public class Scroll : MonoBehaviour
{
    public float speed = 1.0f;
    public float startPosition;  //開始位置
    public float endposition;　　//終端位置

    void Update()
    {
        transform.Translate(-1 * speed * Time.deltaTime, 0, 0);

        if (transform.position.x <= endposition) ScrollEnd();
    }

    void ScrollEnd()　//目標地点まで行ったらもう一度
    {
        float diff = transform.position.x - endposition;
        Vector3 restartPosition = transform.position;
        restartPosition.x = startPosition + diff;
        transform.position = restartPosition;

       
    }
}
