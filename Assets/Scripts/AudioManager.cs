using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    // 0 = music, 1 = explosion, 2 = metal, 3 = die, 4 = summon
    [SerializeField] private List<AudioClip> clipList = new();
    [SerializeField] private List<float> volumeList = new();

    private bool diePlaying;

    public void PlayClip(int clipNumber)
    {
        StartCoroutine(ClipRoutine(clipNumber));
    }

    private IEnumerator ClipRoutine(int clipNumber)
    {
        if (clipNumber == 3)
        {
            if (diePlaying)
                yield break;

            diePlaying = true;
        }

        audioSource.PlayOneShot(clipList[clipNumber], volumeList[clipNumber]);

        yield return new WaitForSeconds(clipList[clipNumber].length);

        if (clipNumber == 3)
            diePlaying = false;
    }
}