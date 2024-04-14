using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    // STATIC:
    public static List<Vector2Int> directions = new()
        { Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down };

    // PREFAB REFERENCE:
    [SerializeField] protected SpriteRenderer sr;
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected CircleCollider2D col;

    [SerializeField] protected CircleCollider2D auraCol;

    [SerializeField] bool lightAura;

    [SerializeField] private float defaultMoveSpeed;

    // CONSTANT:
    [SerializeField] private List<Sprite> facingSprites = new();

    // DYNAMIC:
    protected Vector2 faceDirection;

    protected float moveSpeed;

    public void ChangeFaceDirectionFromVector(Vector2 normalizedDirection)
    {
        int closestDirectionIndex = 0;
        float closestDirectionDifference = 0;
        for (int i = 0; i < directions.Count; i++)
        {
            float newDirectionDifference = Vector2.Distance(normalizedDirection, directions[i]);

            if (i == 0 || newDirectionDifference < closestDirectionDifference)
            {
                closestDirectionIndex = i;
                closestDirectionDifference = newDirectionDifference;
            }
        }

        ChangeFaceDirection(closestDirectionIndex);
    }

    public void ChangeFaceDirection(int facingDirectionIndex)
    {
        sr.sprite = facingSprites[facingDirectionIndex];

        faceDirection = directions[facingDirectionIndex];
    }

    public void SetMoveSpeed()
    {
        moveSpeed = defaultMoveSpeed;
        if (PhaseManager.LightningPhase)
            moveSpeed *= 1 + PhaseManager.LightningPhaseIncrease;
    }

    public virtual void DestroyEntity()
    {
        Destroy(gameObject);
    }

    public virtual void AuraTrigger(Collider2D col) // Called by Aura
    {
        if (col.CompareTag(lightAura ? "DarkTile" : "LightTile"))
            col.GetComponent<Tile>().ToggleLightDark(lightAura);
    }
}