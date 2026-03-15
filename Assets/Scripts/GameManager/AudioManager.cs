using System;
using Unity.AppUI.UI;
using UnityEngine;
using static GameEvent;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] EnemyhitSound;
    public float pitchMin = 0.5f;
    public float pitchMax = 1.5f;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnEnable()
    {
        GameEvent.PlaySFX += PlayAudio;
    }
    private void OnDisable()
    {
        GameEvent.PlaySFX-=PlayAudio;
    }
    public void PlayAudio(GameEvent.SFXType sFXType)
    {
        switch(sFXType)
        {
            case GameEvent.SFXType.EnemyHit:
                PlayRandomSound(EnemyhitSound);
                break;
            case GameEvent.SFXType.PlayerHit:

                break;
            case GameEvent.SFXType.SwordSwing:

                break;
        }
    }
    private void PlayRandomSound(AudioClip[] audioClips)
    {
        if(audioClips==null||audioClips.Length==0)
        {
            return;
        }
        audioSource.pitch = Random.Range(pitchMin, pitchMax);
        int randomIndex = Random.Range(0, audioClips.Length);
        AudioClip clipToPlay = audioClips[randomIndex];
        audioSource.PlayOneShot(clipToPlay);
    }
}
