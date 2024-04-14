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

        else if (col.CompareTag("Corpse") && entity is Player player)
        {
            Corpse corpse = col.GetComponent<Corpse>();

            player.corpseQueue.Add(corpse.corpseType);

            Destroy(corpse.gameObject);
        }
    }
    private void OnTriggerStay2D(Collider2D col)
    {
        if (!lightAura && col.CompareTag("LightTile"))
            col.GetComponent<Tile>().ToggleLightDark(false);
    }
}