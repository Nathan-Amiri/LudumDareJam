using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : Entity
{
    // CONSTANT:
    private readonly float bouncePadTransparency = .8f;

    private void Start()
    {
        ToggleReady(false);
    }

    private void ToggleReady(bool ready)
    {
        sr.color = ready ? Color.white : new Color(1, 1, 1, bouncePadTransparency);
        auraCol.enabled = ready;
    }

    public virtual void OnActivate()
    {
        ToggleReady(true);

        rb.velocity = faceDirection * moveSpeed;
    }
}