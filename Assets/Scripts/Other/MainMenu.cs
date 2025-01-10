using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject panelSettingPanel;
    public GameObject panelChapters;

    public void ShowSetting()
    {
        AudioManager.instance.PlaySFX("Click");
        panelSettingPanel.SetActive(true);
    }

    public void HideSetting()
    {
        AudioManager.instance.PlaySFX("Close");
        panelSettingPanel.SetActive(false);
    }

    public void ChooseChapter()
    {
        AudioManager.instance.PlaySFX("Click");
        mainMenu.SetActive(false);
        panelChapters.SetActive(true);
    }

    public void QuitGame()
    {
        AudioManager.instance.PlaySFX("Close");
        //Debug.Log("QUIT!");
        Application.Quit();

    }

    public void BackToMenu()
    {
        AudioManager.instance.PlaySFX("Close");
        panelChapters.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void GoToGame()
    {
        AudioManager.instance.Pause();
        AudioManager.instance.PlaySFX("Click");
        PlayerPrefs.Save();
        TransitionManager.Instance.TransitionToScene(2);
    }

    public void PlayChapter1()
    {
        PlayerPrefs.SetInt("ChapterSelected", 1);
        GoToGame();
    }

    public void PlayChapter2()
    {
        PlayerPrefs.SetInt("ChapterSelected", 2);
        GoToGame();
    }

    public void PlayChapter3()
    {
        PlayerPrefs.SetInt("ChapterSelected", 3);
        GoToGame();
    }

    private void Start()
    {
        AudioManager.instance.UnPause();
    }

}
