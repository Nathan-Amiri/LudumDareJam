using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    // STATIC:
    public static List<Tile> tilesOnGrid = new();

    // PREFAB REFERENCE:
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Sprite lightSprite;
    [SerializeField] private Sprite darkSprite;

    private void Awake()
    {
        tilesOnGrid.Add(this);
    }

    public void ToggleLightDark(bool light)
    {
        sr.sprite = light ? lightSprite : darkSprite;
        tag = light ? "LightTile" : "DarkTile";
    }
}