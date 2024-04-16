using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PhaseManager : MonoBehaviour
{
    // SCENE REFERENCE:
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private Player player;
    [SerializeField] private AudioManager audioManager;

    [SerializeField] private GameObject menuScreen;
    [SerializeField] private GameObject gameUI;
    [SerializeField] private TMP_Text currentScoreText;
    [SerializeField] private TMP_Text clockText;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private TMP_Text endScoreText;

    private readonly float gameDuration = 308;

    // CONSTANT:
    [SerializeField] private float lightningPhaseIncrease;

    // DYNAMIC:
    private float timeRemaining = 308;

    private void Update()
    {
        if (gameUI.activeSelf)
        {
            currentScoreText.text = "Current Score:\n" + GetScore().ToString() + "%";

            timeRemaining -= Time.deltaTime;
            clockText.text = "Time remaining:\n" + Mathf.CeilToInt(timeRemaining).ToString();
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

    public void SelectNewGame()
    {
        Tile.tilesOnGrid.Clear();
        SceneManager.LoadScene(0);
    }

    private IEnumerator PhaseRoutine()
    {
        menuScreen.SetActive(false);
        gameUI.SetActive(true);

        player.GameStartEnd(true);
        enemySpawner.StartStopSpawning(true);

        audioManager.PlayClip(0);

        yield return new WaitForSeconds(gameDuration);

        enemySpawner.StartStopSpawning(false);
        player.GameStartEnd(false);

        gameUI.SetActive(false);
        endScoreText.text = "Final Score:\n" + GetScore() + "%";
        gameOverScreen.SetActive(true);
    }
}