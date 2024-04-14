using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] private Corpse corpsePref;
    [SerializeField] private int enemyType;

    // Set by EnemySpawner
    [NonSerialized] public EnemySpawner enemySpawner;

    private bool dead;

    public void OnSpawn()
    {
        rb.velocity = faceDirection * moveSpeed;
    }

    public override void DestroyEntity() // Called by EnemySpawner
    {
        enemySpawner.activeEnemies.Remove(this);

        base.DestroyEntity();
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (!dead && col.CompareTag("DarkTile"))
        {
            dead = true;

            SpawnCorpse();
            DestroyEntity();
        }
    }

    private void SpawnCorpse()
    {
        Vector2Int gridPosition = Vector2Int.RoundToInt(transform.position);
        if (gridPosition.magnitude > 18)
            return;

        Corpse corpse = Instantiate(corpsePref, (Vector2)gridPosition, Quaternion.identity);
        corpse.OnSpawn(enemyType);
    }
}