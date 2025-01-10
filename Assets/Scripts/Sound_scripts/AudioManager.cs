using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void PlayMusic (string name)
    {
        Sound s = System.Array.Find(musicSounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        musicSource.clip = s.clip;
        musicSource.Play();
    }

    public void Pause()
    {
        musicSource.Pause();
    }

    public void UnPause()
    {
        musicSource.UnPause();
    }

    public void PlaySFX(string name)
    {
        Sound s = System.Array.Find(sfxSounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        sfxSource.PlayOneShot(s.clip);
    }

    public void SetVolumeMusic(float volume)
    {
        musicSource.volume = volume;
    }

    public void SetVolumeSFX(float volume)
    {
        sfxSource.volume = volume;
    }

    private void Start()
    {
        float musicVolume = PlayerPrefs.GetFloat("musicVolume", 1);
        float sfxVolume = PlayerPrefs.GetFloat("sfxVolume", 1);
        SetVolumeMusic(musicVolume);
        SetVolumeSFX(sfxVolume);
        PlayMusic("BG");
    }

}
