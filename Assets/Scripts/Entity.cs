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

    [SerializeField] private float defaultMoveSpeed;

    // CONSTANT:
    [SerializeField] private List<Sprite> facingSprites = new();

    // DYNAMIC:
    protected Vector2 faceDirection;

    protected float moveSpeed;

    private bool walking;

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

    public void OnSpawn()
    {
        walking = true;
    }

    protected virtual void Update()
    {
        moveSpeed = defaultMoveSpeed;
        if (PhaseManager.LightningPhase)
            moveSpeed *= 1 + PhaseManager.LightningPhaseIncrease;
    }

    protected virtual void FixedUpdate()
    {
        rb.velocity = walking ? faceDirection * moveSpeed : Vector2.zero;
    }
}