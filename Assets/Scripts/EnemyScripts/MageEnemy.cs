using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageEnemy : Enemy
{
    [SerializeField] private CircleCollider2D hitbox;

    [SerializeField] private int boardWidth;

    [SerializeField] private GameObject mageExplosion;
    [SerializeField] private float mageExplosionDuration;

    private Vector2Int targetExplosionPosition;

    public override void OnSpawn(EnemySpawner enemySpawner)
    {
        base.OnSpawn(enemySpawner);

        float random = Random.Range((float)-boardWidth / 2, (float)boardWidth / 2);

        Vector2 targetPositionFloat;
        if (Player.GetPositivePerpendicular(faceDirection) == Vector2.up)
            targetPositionFloat = new(random, transform.position.y);
        else
            targetPositionFloat = new(transform.position.x, random);

        targetExplosionPosition = Vector2Int.RoundToInt(targetPositionFloat);
    }

    private void Update()
    {
        if (Vector2Int.RoundToInt(transform.position) == targetExplosionPosition)
            StartCoroutine(Explode());
    }

    private IEnumerator Explode()
    {
        hitbox.enabled = false;
        mageExplosion.SetActive(true);

        yield return new WaitForSeconds(mageExplosionDuration);

        DestroyEntity();
    }
}