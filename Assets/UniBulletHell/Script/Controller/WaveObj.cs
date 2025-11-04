using UnityEngine;

/// <summary>
/// Wave（敵出現管理）の個別オブジェクトを定義する基底クラス
/// 
/// Wave → WaveObj の仕組みにより、ステージ構成を柔軟にコントロールできる
/// </summary>
public class WaveObj : MonoBehaviour
{
    public virtual bool IsEnd()
    {
        // 基本実装：非アクティブになったら終了とみなす
        return !gameObject.activeInHierarchy;
    }
}
