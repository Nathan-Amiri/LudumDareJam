using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseManager : MonoBehaviour
{
    // STATIC:
    public static float CurrentPhase;

    public static float LightningPhaseIncrease;
    public static bool LightningPhase;

    // SCENE REFERENCE:
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private Player player;

    [SerializeField] private float phaseDuration;

    // CONSTANT:
    [SerializeField] private float lightningPhaseIncrease;

    private void Awake()
    {
        LightningPhaseIncrease = lightningPhaseIncrease;
    }

    public void SelectStartGame()
    {
        StartCoroutine(PhaseRoutine());
    }

    private IEnumerator PhaseRoutine()
    {
        CurrentPhase = 1;

        player.GameStartEnd(true);
        enemySpawner.StartStopSpawning(true);

        while (CurrentPhase < 4)
        {
            NewPhase();

            yield return new WaitForSeconds(phaseDuration);

            CurrentPhase += 1;
        }

        enemySpawner.StartStopSpawning(false);
        player.GameStartEnd(false);
    }

    private void NewPhase()
    {
        enemySpawner.ClearEnemies();
        enemySpawner.NewEnemyPool(false);

        int newMode = Random.Range(0, 8);

        if (newMode < 4)
        {
            enemySpawner.NewEnemyPool(true, newMode + 1);
            return;
        }

        switch (newMode)
        {
            case 4:

                break;
            case 5:

                break;
            case 6:

                break;
            case 7:

                break;
        }
    }

    private int GetScore()
    {
        int darkTiles = 0;
        foreach (Tile tile in Tile.tilesOnGrid)
            if (tile.CompareTag("DarkTile"))
                darkTiles += 1;

        float darkPercentage = (float)darkTiles / Tile.tilesOnGrid.Count;
        return Mathf.RoundToInt(darkPercentage * 100);
    }
}