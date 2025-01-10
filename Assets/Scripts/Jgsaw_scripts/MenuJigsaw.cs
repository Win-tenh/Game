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

    IEnumerator FadeInUI(GameObject panel, float fadeInDuration = 2.0f)
    {
        Graphic[] graphics = panel.GetComponentsInChildren<Graphic>();
        foreach (Graphic g in graphics)
        {
            g.color = new Color(
                g.color.r,
                g.color.g,
                g.color.b,
                0.0f);
        }

        float timer = 0.0f;
        while (timer < fadeInDuration)
        {
            timer += Time.deltaTime;
            float normailisedTime = timer / fadeInDuration;
            foreach (Graphic g in graphics)
            {
                g.color = new Color(
                    g.color.r,
                    g.color.g,
                    g.color.b,
                    normailisedTime);
            }
            yield return null;
        }
        foreach (Graphic g in graphics)
        {
            g.color = new Color(
                g.color.r,
                g.color.g,
                g.color.b,
                1.0f);
        }
    }

    public void SetEnableBotPanel(bool flag)
    {
        panelBottomPanel.SetActive(flag);
        if (flag)
            FadeInUI(panelBottomPanel);
    }

    public void SetEnableTopPanel(bool flag)
    {
        panelTopPanel.SetActive(flag);
        if (flag)
            FadeInUI(panelTopPanel);
    }

    public void SetEnablePausePanel(bool flag)
    {
        AudioManager.instance.PlaySFX("Click");
        panelPausePanel.SetActive(flag);
        if (flag)
            FadeInUI(panelPausePanel);
    }

    public void SetEnableGameComplete(bool flag)
    {
        AudioManager.instance.Pause();
        AudioManager.instance.PlaySFX("Complete");
        textTimeComplete.text = textTime.text;

        panelGameComplete.SetActive(flag);
        if (flag)
            FadeInUI(panelGameComplete);
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
