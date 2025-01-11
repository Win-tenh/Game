using UnityEngine;
using UnityEngine.UI;

public class SettingController : MonoBehaviour
{
    public Slider musicSlider, sfxSlider;

    public GameObject btnMusic, btnSFX;
    public Sprite musicOn, musicOff, sfxOn, sfxOff;

    public void ToggleMusic()
    {
        if (PlayerPrefs.GetFloat("musicVolume", 1f) > 0)
        {
            AudioManager.instance.SetVolumeMusic(0f);
            musicSlider.value = 0f;
            PlayerPrefs.SetFloat("musicVolume", 0f);
        }
        else
        {
            AudioManager.instance.SetVolumeMusic(1f);
            musicSlider.value = 1f;
            PlayerPrefs.SetFloat("musicVolume", 1f);
        }
        AudioManager.instance.PlaySFX("Click");
        SetSpriteMusic();
    }

    public void ToggleSFX()
    {
        if (PlayerPrefs.GetFloat("sfxVolume", 1f) > 0)
        {
            AudioManager.instance.SetVolumeSFX(0f);
            sfxSlider.value = 0f;
            PlayerPrefs.GetFloat("sfxVolume", 0f);
        }
        else
        {
            AudioManager.instance.SetVolumeSFX(1f);
            sfxSlider.value = 1f;
            PlayerPrefs.SetFloat("sfxVolume", 1f);
        }
        AudioManager.instance.PlaySFX("Click");
        SetSpriteSFX();
    }

    public void SetMusicVolume()
    {
        AudioManager.instance.SetVolumeMusic(musicSlider.value);
        PlayerPrefs.SetFloat("musicVolume", musicSlider.value);
        SetSpriteMusic();
    }

    public void SetSFXVolume()
    {
        AudioManager.instance.SetVolumeSFX(sfxSlider.value);
        PlayerPrefs.SetFloat("sfxVolume", sfxSlider.value);
        SetSpriteSFX();
    }

    // set Sprite for Btn
    public void SetSpriteMusic()
    {
        if (PlayerPrefs.GetFloat("musicVolume", 1f) > 0)
        {
            btnMusic.GetComponent<Image>().sprite = musicOn;
        }
        else
        {
            btnMusic.GetComponent<Image>().sprite = musicOff;
        }
    }

    public void SetSpriteSFX()
    {
        if (PlayerPrefs.GetFloat("sfxVolume", 1f) > 0)
        {
            btnSFX.GetComponent<Image>().sprite = sfxOn;
        }
        else
        {
            btnSFX.GetComponent<Image>().sprite = sfxOff;
        }
    }


    void Start()
    {
        // kiểm tra các GO có tồn tại không
        if (btnMusic == null || 
            btnSFX == null || 
            musicSlider == null || 
            sfxSlider == null)
        {
            return;
        }
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume", 1); ;
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume", 1);
        SetSpriteMusic();
        SetSpriteSFX();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
