using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    private Enemy enemy;
    private void Awake()
    {
        GameObject newEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        enemy = newEnemy.GetComponent<Enemy>();
        enemy.InitEnemy();
    }
}
