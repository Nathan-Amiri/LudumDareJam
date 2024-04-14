using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : Entity
{
    [SerializeField] private List<SpriteRenderer> auraRenderers = new();
    [SerializeField] private List<CircleCollider2D> auraColliders = new();

    [SerializeField] private GameObject spawnExplosion;
    [SerializeField] private float spawnExplosionDuration;

    public virtual void OnActivate()
    {
        sr.color = Color.white;
        foreach (SpriteRenderer auraRenderer in auraRenderers)
            auraRenderer.color = Color.white;

        foreach (CircleCollider2D auraCollider in auraColliders)
            auraCollider.enabled = true;

        SetMoveSpeed();
        rb.velocity = faceDirection * moveSpeed;

        StartCoroutine(SpawnExplosionEnd());
    }

    private IEnumerator SpawnExplosionEnd()
    {
        yield return new WaitForSeconds(spawnExplosionDuration);

        spawnExplosion.SetActive(false);
    }
}