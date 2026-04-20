using System;   
using UnityEngine;
using static GameEvent;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    [Tooltip("用于 PlayOneShot 等音效（原 audioSource）")]
    public AudioSource audioSource;
    [Tooltip("背景音乐，可选；可在 Inspector 指定 Loop 与 Clip")]
    public AudioSource bgmSource;
    public AudioClip[] EnemyBehitSound;
    public AudioClip[] PlayerDefenceBeHit;
    public AudioClip[] PlayerBeHit;
    public AudioClip[] clickUI;
    public float pitchMin = 0.5f;
    public float pitchMax = 1.5f;
    private void Awake()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        float bgm = PlayerPrefs.GetFloat(SettingsPrefs.BgmVolume, 1f);
        float sfx = PlayerPrefs.GetFloat(SettingsPrefs.SfxVolume, 1f);
        SetBgmVolume(bgm);
        SetSfxVolume(sfx);
    }

    private void Start()
    {
        if (bgmSource != null && bgmSource.clip != null && !bgmSource.isPlaying)
            bgmSource.Play();
    }

    public void SetBgmVolume(float linear01)
    {
        linear01 = Mathf.Clamp01(linear01);
        if (bgmSource != null)
            bgmSource.volume = linear01;
    }

    public void SetSfxVolume(float linear01)
    {
        linear01 = Mathf.Clamp01(linear01);
        if (audioSource != null)
            audioSource.volume = linear01;
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
            case GameEvent.SFXType.ClickUI:
                PlayRandomSound(clickUI);
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
