using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // SCENE REFERENCE:
    [SerializeField] private List<Enemy> enemyPrefs = new();

        // Read by Enemy
    public Player player;
    public AudioManager audioManager;

    [SerializeField] private Transform enemyParent;

    [SerializeField] private float spawnDistance;
    [SerializeField] private float spawnWidth;

    [SerializeField] private float averageSpawnDelay;
    [SerializeField] private float spawnDelayVariance;

    [SerializeField] private List<int> spawnChanceByType = new();

    //[SerializeField] private int enemyModeIncrease;
    //[SerializeField] private int enemyModeDecrease;

    // CONSTANT:
    private readonly List<int> enemyTypePool = new();

        // Accessed by Enemy
    [NonSerialized] public readonly List<Enemy> activeEnemies = new();

    // DYNAMIC:
    private Coroutine spawnRoutine;

    //public void NewEnemyPool(bool boost, int boostedType = 0)
    //{
    //    for (int i = 0; i < spawnChanceByType.Count; i++)
    //    {
    //        int chance = spawnChanceByType[i];

    //        if (boost)
    //            chance += i == boostedType ? enemyModeIncrease : -enemyModeDecrease;

    //        for (int j = 0; j < chance; j++)
    //            enemyTypePool.Add(i);
    //    }
    //}

    public void StartStopSpawning(bool start) // Called by PhaseManager
    {
        if (start)
        {
            for (int i = 0; i < spawnChanceByType.Count; i++)
            {
                int chance = spawnChanceByType[i];

                for (int j = 0; j < chance; j++)
                    enemyTypePool.Add(i);
            }

            if (spawnRoutine != null)
                Debug.LogError("Spawn routine already running!");

            spawnRoutine = StartCoroutine(SpawnRoutine());
        }
        else
        {
            StopCoroutine(spawnRoutine);
            spawnRoutine = null;

            ClearEnemies();
        }
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            PrepareToSpawnEnemy();

            float delay = averageSpawnDelay + UnityEngine.Random.Range(-spawnDelayVariance / 2, spawnDelayVariance / 2);

            //if (PhaseManager.LightningPhase)
            //    delay *= 1 - PhaseManager.LightningPhaseIncrease;

            yield return new WaitForSeconds(delay);
        }
    }

    private void PrepareToSpawnEnemy()
    {
        int enemyType = enemyTypePool[UnityEngine.Random.Range(0, enemyTypePool.Count)];

        Vector2 spawnDirection = Entity.directions[UnityEngine.Random.Range(0, 4)];
        float offsetDistance = UnityEngine.Random.Range(-spawnWidth / 2, spawnWidth / 2);
        Vector2 spawnOffset = Player.GetPositivePerpendicular(spawnDirection) * offsetDistance;
        Vector2 spawnPosition = Vector2Int.RoundToInt(spawnDirection * spawnDistance + spawnOffset);

        SpawnEnemy(enemyType, spawnPosition, -spawnDirection);
    }

    public void SpawnEnemy(int enemyType, Vector2 spawnPosition, Vector2 faceDirection) // Called by CartEnemy
    {
        Enemy enemy = Instantiate(enemyPrefs[enemyType], spawnPosition, Quaternion.identity, enemyParent);
        activeEnemies.Add(enemy);

        enemy.ChangeFaceDirectionFromVector(faceDirection);
        //enemy.SetMoveSpeed();
        enemy.OnSpawn(this);
        StartCoroutine(EnemyDespawn(enemy));
    }

    private IEnumerator EnemyDespawn(Enemy enemy)
    {
        yield return new WaitForSeconds(25);

        if (enemy == null)
            yield break;

        activeEnemies.Remove(enemy);
        enemy.DestroyEntity();
    }

    public void ClearEnemies()
    {
        // Enemies remove themselves from activeEnemies automatically
        List<Enemy> enemiesToDestroy = new();
        foreach (Enemy enemy in activeEnemies)
            enemiesToDestroy.Add(enemy);
        foreach (Enemy enemy in enemiesToDestroy)
            enemy.DestroyEntity();
    }
}