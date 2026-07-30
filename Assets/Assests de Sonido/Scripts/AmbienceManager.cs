using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class AmbienceManager : MonoBehaviour
{


    [Header("Audio Source")]
    [SerializeField] private AudioSource ambienceSource;

    [Header("Ambient Sounds")]
    [SerializeField] private AudioClip[] ambienceClips;

    [Header("Time Between Sounds")]
    [SerializeField] private float minTime = 3f;
    [SerializeField] private float maxTime = 8f;

    [Header("Random Volume")]
    [Range(0f, 1f)]
    [SerializeField] private float minVolume = 0.6f;

    [Range(0f, 1f)]
    [SerializeField] private float maxVolume = 1f;

    [Header("Random Pitch")]
    [SerializeField] private float minPitch = 0.9f;
    [SerializeField] private float maxPitch = 1.1f;

    private int lastClip = -1;

    private void Start()
    {
        StartCoroutine(PlayRandomAmbience());
    }

    private IEnumerator PlayRandomAmbience()
    {
        while (true)
        {
            // Waits to play a new sound
            yield return new WaitForSeconds(Random.Range(minTime, maxTime));

            // this is to avoid repeating the previous sound.
            int clipIndex;

            do
            {
                clipIndex = Random.Range(0, ambienceClips.Length);
            }
            while (clipIndex == lastClip && ambienceClips.Length > 1);

            lastClip = clipIndex;

            // Random Pitch and volume
            ambienceSource.pitch = Random.Range(minPitch, maxPitch);
            ambienceSource.volume = Random.Range(minVolume, maxVolume);

            // Plays sound
            ambienceSource.PlayOneShot(ambienceClips[clipIndex]);
        }
    }

}
