using UnityEngine;
using ObjectPool; // `MyObjectPool` を使うために名前空間を追加

public class Bullet : MonoBehaviour
{
    public float speed = 10f; // 弾速
    private MyObjectPool pool; // オブジェクトプール
 //   private Renderer bulletRenderer;

  /*  public void Start()
    {
        bulletRenderer = GetComponent<Renderer>();
    }
  */
    public void SetPool(MyObjectPool objectPool)
    {
        pool = objectPool;
    }

    private void Update()  // 弾の挙動
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime); // 前方に移動

      /*  if (!bulletRenderer.isVisible)   // 画面外に出たらプールに戻す
        {
            pool.OnReleaseToPool(this); 
            Debug.Log("非表示");
        }*/

       /* if(Mathf.Abs(this.transform.position.x) >  30f || Mathf.Abs(this.transform.position.y) > 10f)  //横30、縦10超えたらpoolに戻す
        {
            if (UbhObjectPool.instance != null)
            {
                UbhObjectPool.instance.ReleaseBullet(this, false); // UBH のメソッド
            }
            else
            {
                Debug.LogError("UbhObjectPool が見つかりません！");
            }


        }*/
    }
}
