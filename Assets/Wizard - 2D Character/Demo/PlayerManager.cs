using UnityEngine;
using UnityEngine.UI;

namespace ClearSky
{
    // プレイヤーの基本的な挙動(操作・HP管理・被ダメージ処理・アニメーションなど)を統括するクラス
    public class PlayerManager : MonoBehaviour, IDamaged
    {
        private Rigidbody2D rb;
        private Animator anim;

        // --- 状態管理　---
        private int direction = 1;　//左右向き判定
        private bool isKnockback = false; // ノックバック中かどうか
        private bool alive = true;


        public FixedJoystick joystick;　　//移動用
        public FixedJoystick inputMove;   //弾の挙動操作用


       public  float moveSpeed = 10.0f; // 移動速度
        float xLimit = 17.5f; // X軸移動制限
        float yLimit = 5.9f;  // Y軸移動制限


        [SerializeField] private GameObject dieEffect; // 死亡時のエフェクト
        public int maxHP = 7000;
        public int currentHP;
        public Slider hpBar; 


        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            maxHP = 7000;
            currentHP = maxHP;
            hpBar.maxValue = maxHP;
        }

        private void Update()
        {
            // HPバー更新
            hpBar.value = currentHP;

     
            // スマホ用  ジョイスティックでの移動
            transform.position += Vector3.right * inputMove.Horizontal * moveSpeed * Time.deltaTime;
            transform.position += Vector3.up * inputMove.Vertical * moveSpeed * Time.deltaTime;

            // 移動範囲制限
            Vector3 currentPos = transform.position;
            currentPos.x = Mathf.Clamp(currentPos.x, -xLimit, xLimit);
            currentPos.y = Mathf.Clamp(currentPos.y, -yLimit * 1.8f, yLimit);
            transform.position = currentPos;
        }


        // =================================
        // ダメージ処理（HP減少とノックバック）
        // =================================
        public void Damaged(int damage)
        {
            if (!alive) return;

            if (GameClear.instance != null && GameClear.instance.IsGameCleared) //ゲームクリア状態ならダメージ受けない
            {
                return;
            }

            currentHP -= damage;
            Debug.Log("playerがダメージを受けた！残りHP: " + currentHP);

            if (currentHP > 0)
            {
                Hurt();
            }
            else
            {
                Die();
            }
        }


        // =================================
        // 被弾時のアニメーション・ノックバック・無敵時間処理
        // =================================
        void Hurt()
        {
            if (isKnockback) return;

            anim.SetTrigger("hurt");

            //向きに応じてノックバック方向を反転
            Vector2 knockback = (direction == 1) ? new Vector2(-5f, 1f) : new Vector2(5f, 1f);
            rb.AddForce(knockback, ForceMode2D.Impulse);

            rb.velocity = Vector2.zero;
            isKnockback = true;

            //1秒後にノックバック解除
            Invoke("ResetKnockback", 1.0f);
            Debug.Log("むてき");
        }

        // ===============================
        // ノックバック解除
        // ===============================
        void ResetKnockback()
        {
            rb.velocity = Vector2.zero;
            isKnockback = false;
            Debug.Log("無敵おわり");
        }

        // ===============================
        // 死亡処理
        // ===============================
        private void Die()
        {
            if (!alive) return;

            anim.SetTrigger("die");
            DieEffect();
            alive = false;
        }

        //アニメーション終了時呼び出し用
        public void OnAnimationEnd()
        {
            Destroy(gameObject);
        }

        // 死亡エフェクト生成
        public void DieEffect()
        {
            Vector3 position = transform.position;
            GameObject effect = Instantiate(dieEffect, position, Quaternion.identity);
            Destroy(effect, 2f);
        }



        // ===============================
        // アイテム効果適用
        // ===============================
        public void IncreaseSpeed(float amount)
        {
            moveSpeed += amount;
        }

        public void Heal(int amount)
        {
            currentHP = Mathf.Min(currentHP + amount, maxHP);
        }
    }
}
