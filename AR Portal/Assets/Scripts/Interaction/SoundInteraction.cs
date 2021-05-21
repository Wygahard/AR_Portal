using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundInteraction : MonoBehaviour, Interaction
{
    private AudioSource _audioSource;
    public AudioClip audioClip;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.loop = false;
        _audioSource.playOnAwake = false;

        if (audioClip == null)
            Debug.LogError("Adui clip needs to be referenced in the Inspector", this);

        _audioSource.clip = audioClip;
    }    

    public void Interact()
    {
        _audioSource.Play();
    }

    public void ResetInteraction()
    {
        _audioSource.Stop();
    }

    private void OnDisable()
    {
        ResetInteraction();
    }
}
