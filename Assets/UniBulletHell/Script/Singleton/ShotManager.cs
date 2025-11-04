using ClearSky;
using System.Collections;
using UnityEngine;

public class ShotManager : MonoBehaviour
{

   /* [SerializeField] private UbhBaseShot shot;

    private void Start()
    {
        if (shot != null)
        {
            shot.gameObject.SetActive(true);
            Debug.Log($"égópÇ∑ÇÈíe: {shot.name}");
        }
        else
        {
            Debug.LogWarning("Shot Ç™ê›íËÇ≥ÇÍÇƒÇ¢Ç‹ÇπÇÒ");
        }
    }
    */
    private UbhShotCtrl shotCtrl;

    void Start()
    {
        shotCtrl = GetComponent<UbhShotCtrl>();
    }

/*   public void SetBulletType(BulletType type) 
    {
        if (shotCtrl != null)
        {
            shotCtrl.ChangeBullet(type);
        }
    }*/
}
