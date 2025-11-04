using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathEffect : MonoBehaviour
{
    [SerializeField] private GameObject dieEffectPrefab;

    public void SpawnEffect(Vector3 position)
    {
        if (dieEffectPrefab == null) return;

        var effect = Instantiate(dieEffectPrefab, position, Quaternion.identity);

    }
}
