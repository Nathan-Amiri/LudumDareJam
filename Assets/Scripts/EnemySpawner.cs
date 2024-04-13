using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // SCENE REFERENCE:
    [SerializeField] private List<Enemy> enemyPrefs = new();

    [SerializeField] private Transform enemyParent;

    [SerializeField] private float spawnDistance;
    [SerializeField] private float spawnWidth;

    [SerializeField] private float averageSpawnDelay;
    [SerializeField] private float spawnDelayVariance;

    [SerializeField] private float gruntSpawnChance;
    [SerializeField] private float mageSpawnChance;
    [SerializeField] private float priestSpawnChance;
    [SerializeField] private float cartSpawnChance;
    [SerializeField] private float shieldbearerSpawnChance;

    [SerializeField] private float enemyModeIncrease;
    [SerializeField] private float enemyModeDecrease;

    // CONSTANT:
    private readonly List<int> enemyPool = new();

    // DYNAMIC:
    private Coroutine spawnRoutine;

    private void Start()
    {
        for (int i = 0; i < gruntSpawnChance; i++)
            enemyPool.Add(0);
        for (int i = 0; i < mageSpawnChance; i++)
            enemyPool.Add(1);
        for (int i = 0; i < priestSpawnChance; i++)
            enemyPool.Add(2);
        for (int i = 0; i < cartSpawnChance; i++)
            enemyPool.Add(3);
        for (int i = 0; i < shieldbearerSpawnChance; i++)
            enemyPool.Add(4);

        StartStopSpawning(true);
    }

    public void StartStopSpawning(bool start)
    {
        if (start)
        {
            if (spawnRoutine != null)
                Debug.LogError("Spawn routine already running!");

            spawnRoutine = StartCoroutine(SpawnRoutine());
        }
        else
        {
            StopCoroutine(SpawnRoutine());
            spawnRoutine = null;
        }
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            SpawnEnemy();

            float delay = averageSpawnDelay + Random.Range(-spawnDelayVariance / 2, spawnDelayVariance / 2);

            if (PhaseManager.LightningPhase)
                delay *= 1 - PhaseManager.LightningPhaseIncrease;

            yield return new WaitForSeconds(delay);
        }
    }

    public void SpawnEnemy()
    {
        int enemyType = enemyPool[Random.Range(0, enemyPool.Count)];

        Vector2 spawnDirection = Entity.directions[Random.Range(0, 4)];
        float offsetDistance = Random.Range(-spawnWidth / 2, spawnWidth / 2);
        Vector2 spawnOffset = Vector2.Perpendicular(spawnDirection) * offsetDistance;
        Vector2 spawnPosition = Vector2Int.RoundToInt(spawnDirection * spawnDistance + spawnOffset);

        Enemy enemy = Instantiate(enemyPrefs[enemyType], spawnPosition, Quaternion.identity, enemyParent);
        enemy.ChangeFaceDirectionFromVector(-spawnDirection);
        enemy.OnSpawn();
    }
}