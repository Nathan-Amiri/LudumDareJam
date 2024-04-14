using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : Entity
{
    [SerializeField] private List<SpriteRenderer> spawnAuraRenderers = new();
    [SerializeField] private List<CircleCollider2D> spawnAuraColliders = new();

    [SerializeField] private GameObject spawnExplosion;
    [SerializeField] private float spawnExplosionDuration;

    protected Player player;

    public virtual void OnActivate(Player newPlayer) // Called by Player
    {
        player = newPlayer;

        sr.color = Color.white;
        foreach (SpriteRenderer auraRenderer in spawnAuraRenderers)
            auraRenderer.color = Color.white;

        foreach (CircleCollider2D auraCollider in spawnAuraColliders)
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