using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageMinion : Minion
{
    [SerializeField] private GameObject mageExplosion;
    [SerializeField] private float explodeDelay;
    [SerializeField] private float mageExplosionDuration;

    public override void OnActivate(Player player)
    {
        base.OnActivate(player);

        StartCoroutine(MageExplosion());
    }

    private IEnumerator MageExplosion()
    {
        yield return new WaitForSeconds(explodeDelay);

        mageExplosion.SetActive(true);

        yield return new WaitForSeconds(mageExplosionDuration);

        DestroyEntity();
    }
}