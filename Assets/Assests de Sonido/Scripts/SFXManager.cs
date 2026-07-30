using UnityEngine;

public class SFXManager : MonoBehaviour
{
    [SerializeField] private AudioSource[] audioSources;

    public void Play(AudioClip clip)
    {
        GetAvailableSource().PlayOneShot(clip);
    }

    private AudioSource GetAvailableSource()
    {
        foreach (AudioSource source in audioSources)
        {
            if (!source.isPlaying)
                return source;
        }

        return audioSources[0];
    }
}
