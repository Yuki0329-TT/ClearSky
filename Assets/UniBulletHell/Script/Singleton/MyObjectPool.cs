using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace ObjectPool
{
    public class MyObjectPool : MonoBehaviour
    {
        [SerializeField] private Bullet _bulletPrefab; // `BulletObject` → `Bullet` に変えた
        [SerializeField] private Transform _content; //初期サイズ
        [SerializeField] private int InitialPoolSize = 10;
        public ObjectPool<Bullet> pool;

        void Start()
        {
            // 初期プールの作成
            pool = new ObjectPool<Bullet>(
                OnCreatePooledObject,
                OnGetFromPool,
                OnReleaseToPool,
                OnDestroyPooledObject);

            for (int i = 0; i < InitialPoolSize; i++)  //初期弾作る
            {
                Bullet bullet = pool.Get();　//プール設定して表示
                pool.Release(bullet);　　　　//プールに戻して非表示

            }
        }

        public Bullet OnCreatePooledObject()
        {
            Bullet bullet = Instantiate(_bulletPrefab, _content); // `BulletObject` → `Bullet` に変更
            bullet.SetPool(this);  // 'Bullet' に 'MyObjectPool' の情報を伝える
            return bullet;
        }

        public void OnGetFromPool(Bullet bullet)
        {
            bullet.SetPool(this);
            bullet.gameObject.SetActive(true);
        }

        public void OnReleaseToPool(Bullet bullet)
        {
            bullet.gameObject.SetActive(false);
        }

        public void OnDestroyPooledObject(Bullet bullet)
        {
            Destroy(bullet.gameObject);
        }
    }
}
