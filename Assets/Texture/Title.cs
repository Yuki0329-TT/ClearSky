using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    private bool firstPush = false;
    
    // ボタンが押されたら呼ばれる
    public void PressStart()
    {
        Debug.Log("Press Start!");

        if(firstPush == false)  //次のシーンに行く　連打対策
        {

            Debug.Log("Go to Next Scene!");
            SceneManager.LoadScene("Scene1");
            firstPush = true;
        }

    
    }
}
