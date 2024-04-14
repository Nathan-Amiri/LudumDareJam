using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corpse : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private List<Sprite> corpseSprites = new();

        // Read by Player
    [NonSerialized] public int corpseType;

    public void OnSpawn(int newCorpseType)
    {
        corpseType = newCorpseType;
        sr.sprite = corpseSprites[corpseType];
    }

    private void Update()
    {
        if (Vector2Int.RoundToInt(Player.mousePosition) == Vector2Int.RoundToInt(transform.position))
            Player.corpseMouseOver = transform;
        else if (Player.corpseMouseOver == transform)
            Player.corpseMouseOver = null;
    }
}