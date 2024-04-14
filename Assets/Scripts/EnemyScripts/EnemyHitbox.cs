using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{
    [SerializeField] private Enemy enemy;

    private bool dead;

    private void OnTriggerStay2D(Collider2D col)
    {
        if (!dead && col.CompareTag("DarkTile"))
        {
            dead = true;

            enemy.Die();
        }
    }

    public void ShieldbearerNotDead()
    {
        dead = false;
    }
}