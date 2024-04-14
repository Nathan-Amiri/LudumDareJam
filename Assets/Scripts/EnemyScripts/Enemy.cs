using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] private Corpse corpsePref;
    [SerializeField] private int enemyType;

    protected EnemySpawner enemySpawner;

    public virtual void OnSpawn(EnemySpawner newEnemySpawner) // Called by EnemySpawner
    {
        enemySpawner = newEnemySpawner;

        rb.velocity = faceDirection * moveSpeed;
    }

    public virtual void Die() // Called by EnemyHitbox
    {
        SpawnCorpse();
        DestroyEntity();
    }

    public override void DestroyEntity() // Called by EnemySpawner
    {
        enemySpawner.activeEnemies.Remove(this);

        base.DestroyEntity();
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