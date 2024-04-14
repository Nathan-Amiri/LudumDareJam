using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PhaseManager : MonoBehaviour
{
    // STATIC:
    public static float CurrentPhase;

    public static float LightningPhaseIncrease;
    public static bool LightningPhase;

    // SCENE REFERENCE:
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private Player player;

    [SerializeField] private GameObject menuScreen;
    [SerializeField] private GameObject gameUI;
    [SerializeField] private TMP_Text currentScoreText;
    [SerializeField] private TMP_Text clockText;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private TMP_Text endScoreText;

    [SerializeField] private float phaseDuration;

    // CONSTANT:
    [SerializeField] private float lightningPhaseIncrease;

    // DYNAMIC:
    private float timeRemaining;

    private void Awake()
    {
        LightningPhaseIncrease = lightningPhaseIncrease;
    }
    private void Update()
    {
        if (gameUI.activeSelf)
        {
            currentScoreText.text = "Current Score:\n" + GetScore().ToString() + "%";

            timeRemaining -= Time.deltaTime;
            clockText.text = "New Phase in:\n" + Mathf.FloorToInt(timeRemaining).ToString();
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

    public void SelectStartGame()
    {
        StartCoroutine(PhaseRoutine());
    }

    private IEnumerator PhaseRoutine()
    {
        CurrentPhase = 1;

        menuScreen.SetActive(false);
        gameUI.SetActive(true);

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

        gameUI.SetActive(false);
        endScoreText.text = "Final Score:\n" + GetScore() + "%";
        gameOverScreen.SetActive(true);
    }

    private void NewPhase()
    {
        timeRemaining = phaseDuration;

        player.SetMoveSpeed();

        enemySpawner.ClearEnemies();
        enemySpawner.NewEnemyPool(false);

        int newMode = Random.Range(0, 8);

        if (newMode < 4)
        {
            //enemySpawner.NewEnemyPool(true, newMode + 1);
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
}