using System;
using UnityEngine;

public class MessageLogger : MonoBehaviour
{
    public static event Action<GameObject> EnemyHit;

    public static void ReportEnemy(GameObject enemy)
    {
        EnemyHit?.Invoke(enemy);
    }

    void OnEnable()
    {
        EnemyHit += LogEnemy;
    }

    void OnDisable()
    {
        EnemyHit -= LogEnemy;
    }

    void LogEnemy(GameObject enemy)
    {
        Debug.Log(enemy.name, enemy);
    }
}
