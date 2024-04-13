using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aura : MonoBehaviour
{
    [SerializeField] private Entity entity;

    private void OnTriggerEnter2D(Collider2D col)
    {
        entity.AuraTrigger(col);
    }
}