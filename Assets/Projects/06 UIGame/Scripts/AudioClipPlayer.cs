using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioClipPlayer : MonoBehaviour{

    [SerializeField] AudioClip[] clips;
    [SerializeField] AudioSource[] audioSources;


    public void PlayRdmSound() {
        foreach (AudioSource audioSource in audioSources) {
            if (audioSource.isPlaying) continue;
            audioSource.clip = clips[Random.Range(0, clips.Length)];
            audioSource.Play();
        }
    }
}
