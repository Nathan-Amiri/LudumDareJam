using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] private int enemyType;

    protected EnemySpawner enemySpawner;

    public virtual void OnSpawn(EnemySpawner newEnemySpawner) // Called by EnemySpawner
    {
        enemySpawner = newEnemySpawner;

        rb.velocity = faceDirection * moveSpeed;
    }

    public virtual void Die() // Called by Aura
    {
        enemySpawner.audioManager.PlayClip(3);

        enemySpawner.player.AddCorpse(enemyType);
        DestroyEntity();
    }

    public override void DestroyEntity() // Called by EnemySpawner
    {
        enemySpawner.activeEnemies.Remove(this);

        base.DestroyEntity();
    }
}