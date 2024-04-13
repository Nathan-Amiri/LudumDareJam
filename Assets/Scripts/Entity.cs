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
    public Vector2Int faceDirection;

    public void ChangeFaceDirectionFromVector(Vector2 normalizedDirection)
    {
        int closestDirectionIndex = 0;
        float closestDirectionDifference = 0;
        for (int i = 0; i < facingDirections.Count; i++)
        {
            float newDirectionDifference = Vector2.Distance(normalizedDirection, facingDirections[i]);

            if (i == 0 || newDirectionDifference < closestDirectionDifference)
            {
                closestDirectionIndex = i;
                closestDirectionDifference = newDirectionDifference;
            }
        }

        sr.sprite = facingSprites[closestDirectionIndex];

        faceDirection = facingDirections[closestDirectionIndex];
    }
}