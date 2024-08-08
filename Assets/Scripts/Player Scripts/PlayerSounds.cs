using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    private float collisionSoundEffect = 1f;
    public float audioFootVolume = 1f;
    public float soundEffectRandomness = 0.05f;

    public AudioClip genericFootSound;
    public AudioClip metalFootSound;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // to randomly play the audio & set different clips too
    void FootSound()
    {
        audioSource.volume = collisionSoundEffect * audioFootVolume;
        audioSource.pitch = Random.Range(1.0f - soundEffectRandomness, 1.0f + soundEffectRandomness);

        if(Random.Range(0, 2) > 0)
        {
            audioSource.clip = genericFootSound;
        } else
        {
            audioSource.clip = metalFootSound;
        }
        audioSource.Play();
    }
}
