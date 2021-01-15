using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : Singleton<SoundManager> 
{
    public List<SoundFXDefinition> SoundFX;
    private AudioSource _soundFXSource;
    public AudioSource SoundFXSource { get { return _soundFXSource; } }

    void Start()
    {
        _soundFXSource = GetComponent<AudioSource>();
    }

    public void PlaySoundEffect(SoundEffect soundEffect)
    {
        AudioClip effect = SoundFX.Find(sfx => sfx.Effect == soundEffect).Clip;
        _soundFXSource.PlayOneShot(effect);
    }
}
