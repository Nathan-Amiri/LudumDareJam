using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseManager : MonoBehaviour
{
    // STATIC:
    public static float LightningPhaseIncrease;
    public static bool LightningPhase;

    // SCENE REFERENCE:
    [SerializeField] private EnemySpawner enemySpawner;

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
        enemySpawner.StartStopSpawning(true);

        yield return new WaitForSeconds(phaseDuration);

        enemySpawner.StartStopSpawning(false);
    }
}