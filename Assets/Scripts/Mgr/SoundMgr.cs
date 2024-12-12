using UnityEngine;

public class SoundMgr : SingletonMono<SoundMgr>
{
    public AudioClip[] soundEffects;
    public AudioSource soundSource;

    public AudioClip[] musics;
    public AudioSource musicSource;
    bool isMusicMuted = false;

    public AudioSource ambientSource;
    public AudioClip[] ambientEffects;

    protected override void Awake()
    {
        base.Awake();
        musicSource.loop = true;
        musicSource.volume = 1f;
        musicSource.playOnAwake = false;
    }

    private void Start()
    {
        PlayMusic("music_intro");
        PlayAmbient("ocean_ambience");
    }

    public void PlaySound(string soundName)
    {
        AudioClip clip = System.Array.Find(soundEffects, s => s.name == soundName);
        if (clip == null)
        {
            Debug.Log("Sound Not Fount Warning: " + soundName);
            return;
        }
        soundSource.PlayOneShot(clip);
        Debug.Log("Play sound: " + soundName);
    }

    public void PlayMusic(string musicName)
    {
        AudioClip clip = System.Array.Find(musics, s => s.name == musicName);
        if (clip == null)
        {
            Debug.Log("Music Not Fount Warning: " + musicName);
            return;
        }
        musicSource.clip = clip;
        musicSource.mute = isMusicMuted;
        musicSource.Play();
        Debug.Log("Play music: " + musicName);
    }

    public void PlayAmbient(string ambientName)
    {
        AudioClip clip = System.Array.Find(ambientEffects, s => s.name == ambientName);
        if (clip == null)
        {
            Debug.Log("Sound Not Fount Warning: " + ambientName);
            return;
        }
        ambientSource.clip = clip;
        ambientSource.Play();
        Debug.Log("Play sound: " + ambientName);
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void ChangeMute()
    {
        isMusicMuted = !isMusicMuted;
        musicSource.mute = isMusicMuted;
    }
}

