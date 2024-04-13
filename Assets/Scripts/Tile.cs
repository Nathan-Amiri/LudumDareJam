using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    // STATIC:
    public static Dictionary<Vector2Int, Tile> gridIndex = new();

    // PREFAB REFERENCE:
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Sprite lightSprite;
    [SerializeField] private Sprite darkSprite;

    private void Start()
    {
        gridIndex.Add(Vector2Int.RoundToInt(transform.position), this);
    }

    public void ToggleLightDark(bool light)
    {
        sr.sprite = light ? lightSprite : darkSprite;
    }
}