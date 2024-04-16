using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartMinion : Minion
{
    [SerializeField] private float spawnRate;

    private int spawnDistance = 1;

    public override void OnActivate()
    {
        base.OnActivate();

        StartCoroutine(SpawnGrunts());
    }

    private IEnumerator SpawnGrunts()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnRate);

            Vector2 spawnDirection = Player.GetPositivePerpendicular(faceDirection);
            Vector2 spawnPosition1 = (Vector2)transform.position + (spawnDirection * spawnDistance);
            Vector2 spawnPosition2 = (Vector2)transform.position + (spawnDirection * -spawnDistance);

            player.SummonMinionFromCart(spawnPosition1, faceDirection);
            player.SummonMinionFromCart(spawnPosition2, faceDirection);

            spawnDistance += 1;
        }
    }
}