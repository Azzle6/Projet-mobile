using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public SoundsListSO soundsList;
    public static AudioManager instance;
    private int unavailableAudioS;

    private AudioSource[] AudioSources;

    private void Awake()
    {
        InstantiateAudioSources();
        if (instance != null) return;
        instance = this;
    }

    private void InstantiateAudioSources()
    {
        GameObject AudioSourcesGO = new GameObject("AudioSources");
        
        for (int i = 0; i < 8; i++)
        {
            AudioSourcesGO.gameObject.AddComponent<AudioSource>();
        }
        AudioSources = AudioSourcesGO.GetComponents<AudioSource>();
        foreach (var AudioS in AudioSources)
        {
            AudioS.playOnAwake = false;
        }
    }

    public void PlaySound(string soundName, float delay = 0) =>
        instance.StartCoroutine(instance.PlayAudio(soundName, delay));

    public void PlayRandomSound(string categoryName, float delay = 0)
    {
        
        instance.StartCoroutine(instance.PlayAudio(categoryName, delay, true));

    }

    public IEnumerator PlayAudio(string soundName, float delay, bool randomSound = false)
    {

        SoundInfo sound;
        sound = !randomSound ? soundsList.FindSound(soundName) : soundsList.FindRandomSound(soundName);
            
        yield return new WaitForSeconds(delay);

        unavailableAudioS = 0;
        foreach (AudioSource AudioS in AudioSources)
        {
            if (!AudioS.isPlaying)
            {
                AudioS.clip = sound.clip;
                AudioS.volume = sound.clipVolume;
                AudioS.loop = false;
                AudioS.outputAudioMixerGroup = sound.audioMixer;
                AudioS.Play();
                //Debug.Log(sound.clipName + " is playing");
                break;
            }
            else unavailableAudioS++;
        }
        if(unavailableAudioS == AudioSources.Length) Debug.Log("Not enough AudioSources to play " + sound.clipName);
    }
}
