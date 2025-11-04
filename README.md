# ClearSky

Unityで開発した 2D 弾幕シューティングゲーム。  
プレイヤー挙動・ステージ進行・アイテムショップなどの
主要システムを自作し、敵の移動や弾幕には
DOTween と uBulletHell を活用しています。
各アセットの挙動を理解し、自作スクリプトと組み合わせて
独自のステージ進行とゲームテンポを実現しました。
スマートフォン向け操作（FixedJoystick）に対応しています。 

UnityRoom上で公開済みです。

---

##  ゲーム概要 

| 項目 | 内容 |
|------|------|
| **ジャンル** | 2D 弾幕シューティング |
| **開発環境** | Unity 2022.3 / C# |
| **対応機種** | スマートフォン（WebGL） |
| **制作期間** | 約8ヶ月 |
| **開発人数** | 1人（個人制作） |

---

##  主な機能

###  プレイヤー関連
- **PlayerManager**：移動・HP管理・ダメージ処理・ジョイスティック入力制御  
- **Status**：シールド展開システム  

###  敵関連
- **EnemyCore**：雑魚敵のHP管理・被弾・ノックバック制御  
- **Enemy_Spine_Anim**：ボス専用のSpineアニメーションと弾幕切替  
- **EnemyFormController**：ボスのHP割合による弾幕パターン変化  
- **EnemyAnimationController**：被弾時の点滅アニメーション制御  

###  アイテムシステム
- **ItemShopManager**：ランダム生成された3種のアイテムをUI表示  
- **ItemPickUI**：アイテム購入UI・購入時のボタン制御  
- **ItemEffectApplier**：取得時の効果適用（スピード・回復・スコア倍率など）  
- **ItemManager**：購入回数に応じた価格上昇システム  

###  ステージ管理
- **StageManager**：敵・ボスの生成、難易度上昇処理  
- **StageProgressManager**：ステージ番号の管理、再生成処理  
- **GameClear / GameOver**：勝利・敗北時の画面遷移、復活処理  
- **GameClearSceneManager**：クリア後の自動進行処理  

###  システム
- **ScoreManager / HighScoreManager**：スコア・ハイスコア管理（PlayerPrefs保存対応）  
- **ScoreUI**：スコアをリアルタイムでUI表示  
- **DestroyBullet / Scroll**：画面外削除処理・背景スクロール制御
- **GameManager**：弾変更やアイテム効果適用を統合管理  

---

##  フォルダ構成
Assets/
├── Scripts/
│ ├── Player/
│ ├── Enemy/
│ ├── Item/
│ ├── Stage/
│ ├── System/
│ └── Utility/
├── Prefabs/
├── Scenes/
└── Sprites/

---

##　UnityRoom
https://unityroom.com/games/takaki_1
---

## 使用アセット 
- **Ubh (UniBulletHell)**：弾幕生成フレームワーク（基礎部分）
- **DOTween**：アニメーション制御用
- **Spine 2D**：ボスキャラクターのアニメーション表現
- **FixedJoystick**：スマートフォン向け入力UI


## 補足 / Notes
- PlayerPrefs によるスコア保存対応  
- ボスの弾幕切替はHP割合ベースで自動制御  
- UIボタン操作は `onClick.RemoveAllListeners()` で重複防止  
- GitHub上のコードには全て日本語コメントを付与し、可読性を重視  
