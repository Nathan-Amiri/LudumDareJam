using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    // List clips in this comment
    [SerializeField] private List<AudioClip> clipList = new();
    [SerializeField] private List<float> volumeList = new();

    private readonly List<bool> clipPlaying = new();

    private void Awake()
    {
        for (int i = 0; i < clipList.Count; i++)
            clipPlaying.Add(false);
    }

    public IEnumerator PlayClip(int clipNumber)
    {
        if (clipPlaying[clipNumber]) yield break;

        clipPlaying[clipNumber] = true;

        audioSource.PlayOneShot(clipList[clipNumber], volumeList[clipNumber]);

        yield return new WaitForSeconds(clipList[clipNumber].length);

        clipPlaying[clipNumber] = false;
    }
}