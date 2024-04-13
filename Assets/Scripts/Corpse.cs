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

    public void OnMouseEnter()
    {
        Player.corpseMouseOver = transform;
    }
    public void OnMouseExit()
    {
        if (Player.corpseMouseOver == transform)
            Player.corpseMouseOver = null;
    }
}