using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartEnemy : Enemy
{
    [SerializeField] private float spawnRate;

    public override void OnSpawn(EnemySpawner enemySpawner)
    {
        base.OnSpawn(enemySpawner);

        StartCoroutine(SpawnGrunts());
    }

    private IEnumerator SpawnGrunts()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnRate);

            Vector2 spawnDirection = Player.GetPositivePerpendicular(faceDirection);
            Vector2 spawnPosition1 = (Vector2)transform.position + (spawnDirection * 1);
            Vector2 spawnPosition2 = (Vector2)transform.position + (spawnDirection * -1);

            enemySpawner.SpawnEnemy(0, spawnPosition1, spawnDirection);
            enemySpawner.SpawnEnemy(0, spawnPosition2, -spawnDirection);
        }
    }
}