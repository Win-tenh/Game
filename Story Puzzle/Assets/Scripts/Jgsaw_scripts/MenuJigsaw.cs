using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuJigsaw : MonoBehaviour
{
    public delegate void DelegateOnClick();
    public DelegateOnClick btnPlayOnClick;

    public GameObject panelTopPanel;
    public GameObject panelBottomPanel;
    public GameObject panelGameComplete;
    public GameObject panelPausePanel;

    public Text textTime;
    public Text textTimeComplete;
    public Text textTotalTiles;
    public Text textTilesInPlace;

    private void Start()
    {
        AudioManager.instance.UnPause();
    }

    public void SetEnableBotPanel(bool flag)
    {
        panelBottomPanel.SetActive(flag);
    }

    public void SetEnableTopPanel(bool flag)
    {
        panelTopPanel.SetActive(flag);
    }

    public void SetEnablePausePanel(bool flag)
    {
        if (flag)
            AudioManager.instance.PlaySFX("Click");
        else
            AudioManager.instance.PlaySFX("Close");
        panelPausePanel.SetActive(flag);
    }

    public void SetEnableGameComplete(bool flag)
    {
        AudioManager.instance.Pause();
        AudioManager.instance.PlaySFX("Complete");
        textTimeComplete.text = textTime.text;

        panelGameComplete.SetActive(flag);
    }

    public void SetTimeInSeconds(double tt)
    {
        System.TimeSpan t = System.TimeSpan.FromSeconds(tt);
        string time = string.Format(
            "{0:D2} : {1:D2} : {2:D2}",
            t.Hours,
            t.Minutes,
            t.Seconds);
        textTime.text = time;
    }

    public void SetTotalTiles(int count)
    {
        textTotalTiles.text = count.ToString();
    }

    public void SetTilesInPlace(int count)
    {
        textTilesInPlace.text = count.ToString();
    }

    public void OnClickExit()
    {
        AudioManager.instance.PlaySFX("Click");
        panelGameComplete.SetActive(false);
        TransitionManager.Instance.TransitionToScene(3);
    }

    public void OnClickPLayAgain()
    {
        AudioManager.instance.PlaySFX("Click");
        panelGameComplete.SetActive(false);
        TransitionManager.Instance.TransitionToScene(1);
    }

    public void OnClickHome()
    {
        AudioManager.instance.PlaySFX("Click");
        panelPausePanel.SetActive(false);
        panelTopPanel.SetActive(false);
        TransitionManager.Instance.TransitionToScene(0);
    }

    public void OnClickPlay()
    {
        AudioManager.instance.PlaySFX("Click");
        btnPlayOnClick?.Invoke();
    }

}
