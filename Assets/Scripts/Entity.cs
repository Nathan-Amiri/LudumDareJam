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

    [SerializeField] protected CircleCollider2D auraCol;

    [SerializeField] protected float moveSpeed;

    // DYNAMIC:
    protected Vector2 faceDirection;

    protected virtual void Update()
    {
        float rotation = rb.velocity.x >= 0 ? 0 : 180;
        transform.rotation = Quaternion.Euler(0, rotation, 0);
    }

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
        faceDirection = directions[facingDirectionIndex];
    }

    public virtual void DestroyEntity()
    {
        Destroy(gameObject);
    }
}