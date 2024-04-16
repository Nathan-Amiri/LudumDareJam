using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldbearerEnemy : Enemy
{
    [SerializeField] private EnemyHitbox enemyHitbox;
    [SerializeField] private GameObject aura;

    [SerializeField] private float stunDuration;

    private int health = 3;

    private bool isStunned;

    public override void Die()
    {
        if (isStunned)
            return;

        health -= 1;

        if (health == 0)
        {
            base.Die();

            return;
        }

        enemySpawner.audioManager.PlayClip(2);

        StartCoroutine(Stun());
    }

    private IEnumerator Stun()
    {
        aura.SetActive(false);
        isStunned = true;
        rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(stunDuration);

        aura.SetActive(true);
        isStunned = false;
        rb.velocity = faceDirection * moveSpeed;
    }
}