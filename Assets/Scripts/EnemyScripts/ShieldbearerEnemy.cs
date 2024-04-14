using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldbearerEnemy : Enemy
{
    [SerializeField] private EnemyHitbox enemyHitbox;
    [SerializeField] private GameObject aura;

    [SerializeField] private float stunDuration;

    [SerializeField] private List<Sprite> mediumArmorSprites = new();
    [SerializeField] private List<Sprite> lowArmorSprites = new();

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

        facingSprites = health == 2 ? mediumArmorSprites : lowArmorSprites;
        // Update sprite
        ChangeFaceDirectionFromVector(faceDirection);

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

        // Wait to declare not dead until Aura has been flickered so that below tiles turn light again
        enemyHitbox.ShieldbearerNotDead();
    }
}