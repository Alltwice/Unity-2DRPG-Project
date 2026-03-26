using System;   
using UnityEngine;
using static GameEvent;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] EnemyBehitSound;
    public AudioClip[] PlayerDefenceBeHit;
    public AudioClip[] PlayerBeHit;
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
            case GameEvent.SFXType.EnemyBeHit:
                PlayRandomSound(EnemyBehitSound);
                break;
            case GameEvent.SFXType.PlayerBeHit:
                PlayRandomSound(PlayerBeHit);
                break;
            case GameEvent.SFXType.SwordSwing:

                break;
            case GameEvent.SFXType.PlayerDefenceBeHit:
                PlayRandomSound(PlayerDefenceBeHit);
                break;
        }
    }
    private void PlayRandomSound(AudioClip[] audioClips)
    {
        if(audioClips==null||audioClips.Length==0)
        {
            return;
        }
        int randomIndex = Random.Range(0, audioClips.Length);
        AudioClip clipToPlay = audioClips[randomIndex];
        audioSource.PlayOneShot(clipToPlay);
    }
}
