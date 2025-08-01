using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSourcePrefab;

    public static SFXManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public AudioSource PlayRandomSFX(List<AudioClip> clips, float volume = 0.1f, float delay = 0.0f, bool loop = false, float pitch = 1f)
    {
        if (clips == null)
            return null;

        int index = Random.Range(0, clips.Count);

        return PlaySFXAtLocation(clips[index], null, volume, delay, loop, pitch);
    }

    public AudioSource PlayRandomSFXAtLocation(AudioClip[] clips, Transform target, float volume = 0.1f, float delay = 0.0f, bool loop = false, float pitch = 1f)
    {
        int index = Random.Range(0, clips.Length);

        return PlaySFXAtLocation(clips[index], target, volume, delay, loop, pitch);
    }

    public AudioSource PlaySFXAtLocation(AudioClip clip, Transform target, float volume = 0.1f, float delay = 0.0f, bool loop = false, float pitch = 1f)
    {
        Transform parent = target != null ? target : transform;

        AudioSource source = Instantiate(audioSourcePrefab, parent.position, Quaternion.identity, parent);

        source.clip = clip;
        source.volume = volume;
        source.loop = loop;
        source.pitch = pitch + Random.Range(-0.05f, 0.05f);
        if (delay <= 0.0f)
            source.Play();
        else
            source.PlayDelayed(delay);

        if (!loop)
            Destroy(source.gameObject, clip.length + delay);

        return source;
    }

    public AudioSource PlaySFX(AudioClip clip, float volume = 0.1f, float delay = 0.0f, bool loop = false, float pitch = 1f)
    {
        return PlaySFXAtLocation(clip, null, volume, delay, loop, pitch);
    }
    public AudioSource PlaySFXAtLocationNoPitchModifier(AudioClip clip, Transform target, float volume = 0.1f, float delay = 0.0f, bool loop = false)
    {
        Transform parent = target != null ? target : transform;

        AudioSource source = Instantiate(audioSourcePrefab, parent.position, Quaternion.identity, parent);

        source.clip = clip;
        source.volume = volume;
        source.loop = loop;
        if (delay <= 0.0f)
            source.Play();
        else
            source.PlayDelayed(delay);

        if (!loop)
            Destroy(source.gameObject, clip.length + delay);

        return source;
    }
    public AudioSource PlaySFXNoPitchModifier(AudioClip clip, float volume = 0.1f, float delay = 0.0f, bool loop = false)
    {
        return PlaySFXAtLocationNoPitchModifier(clip, null, volume, delay, loop);
    }
}
