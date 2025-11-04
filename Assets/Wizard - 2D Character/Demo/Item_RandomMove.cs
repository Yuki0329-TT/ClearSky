using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

public class Item_RandomMove : MonoBehaviour
{
    [SerializeField] public GameObject item;
    public int speed = 5;
    Vector3 movePosition;  //オブジェクトの目的地

    void Start()
    {
        movePosition = MoveRandomPosition();
    }

    void Update()
    {
        if (movePosition == item.transform.position) // 目的地に達すると
        {
            movePosition = MoveRandomPosition();　// 目的地再設定
        }

        this.item.transform.position = Vector3.MoveTowards(item.transform.position, movePosition, speed * Time.deltaTime);
    }
        public Vector3 MoveRandomPosition()
    {
        Vector3 randomPosition = new Vector3(Random.Range(-7, 7), Random.Range(-4, 4), 5);
        return randomPosition;
    }


    
}
