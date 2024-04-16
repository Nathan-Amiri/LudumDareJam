using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : Entity
{
    [SerializeField] private GameObject spawnExplosion;
    [SerializeField] private float spawnExplosionDuration;

    protected Player player;

    public void OnSpawn(Player newPlayer) // Called by Player
    {
        player = newPlayer;

        StartCoroutine(SpawnExplosionEnd());
        StartCoroutine(DespawnMinion());
    }

    public virtual void OnActivate() // Called by Player
    {
        rb.velocity = faceDirection * moveSpeed;
    }

    private IEnumerator SpawnExplosionEnd()
    {
        yield return new WaitForSeconds(spawnExplosionDuration);

        spawnExplosion.SetActive(false);
    }

    private IEnumerator DespawnMinion()
    {
        yield return new WaitForSeconds(25);
        DestroyEntity();
    }
}