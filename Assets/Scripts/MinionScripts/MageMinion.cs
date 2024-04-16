using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageMinion : Minion
{
    [SerializeField] private GameObject mageExplosion;
    [SerializeField] private float explodeDelay;
    [SerializeField] private float mageExplosionDuration;

    public override void OnActivate()
    {
        base.OnActivate();

        StartCoroutine(MageExplosion());
        StartCoroutine(Despawn());
    }

    private IEnumerator MageExplosion()
    {
        yield return new WaitForSeconds(explodeDelay);

        StartCoroutine(player.audioManager.PlayClip(1));
        mageExplosion.SetActive(true);

        yield return new WaitForSeconds(mageExplosionDuration);

        DestroyEntity();
    }

    private IEnumerator Despawn()
    {
        yield return new WaitForSeconds(25);
        DestroyEntity();
    }
}