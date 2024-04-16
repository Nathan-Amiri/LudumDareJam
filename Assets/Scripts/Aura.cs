using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aura : MonoBehaviour
{
    [SerializeField] private Entity entity;

    [SerializeField] private bool lightAura;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (lightAura && col.CompareTag("DarkTile"))
            col.GetComponent<Tile>().ToggleLightDark(true);
    }
    private void OnTriggerStay2D(Collider2D col)
    {
        if (!lightAura && col.CompareTag("LightTile"))
            col.GetComponent<Tile>().ToggleLightDark(false);

        else if (!lightAura && col.CompareTag("EnemyHitbox"))
            col.GetComponent<EnemyHitbox>().enemy.Die();
    }
}