using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseManager : MonoBehaviour
{
    // STATIC:
    public static float LightningPhaseIncrease;
    public static bool LightningPhase;

    // CONSTANT:
    [SerializeField] private float lightningPhaseIncrease;

    private void Awake()
    {
        LightningPhaseIncrease = lightningPhaseIncrease;
    }
}