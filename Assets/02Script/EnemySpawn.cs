using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] private Vector3 rotation;
    [SerializeField] private GameObject enemyPrefab;
    private Enemy enemy;
    private EnemyAI enemyAI;
    private void Awake()
    {
        GameObject newEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.Euler(rotation));
        enemy = newEnemy.GetComponent<Enemy>();
        enemy.InitEnemy();
    }
}
