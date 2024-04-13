using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    // PREFAB REFERENCE:
    [SerializeField] protected SpriteRenderer sr;

    // CONSTANT:
    [SerializeField] private List<Sprite> facingSprites = new();
    private readonly List<Vector2Int> facingDirections = new()
        { Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down };

    // DYNAMIC:
    private Vector2Int faceDirection;

    public void ChangeFaceDirection(Vector2Int newDirection)
    {
        faceDirection = newDirection;

        for (int i = 0; i < facingDirections.Count; i++)
            if (facingDirections[i] == newDirection)
            {
                sr.sprite = facingSprites[i];
                return;
            }
    }
}