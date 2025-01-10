using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public Text ScoreText;
    public Text HealthText;
    public GameObject gameOverPanel;
    public GameObject winerPanel;


    public void SetScoreText(int txt, int total)
    {
        if(ScoreText)
            ScoreText.text = 
                "Score : " + 
                txt + 
                " / " +
                total;
    }

    public void SetHealthText(string txt)
    {
        if (HealthText)
            HealthText.text = txt;
    }
    public void ShowGameOver(bool isShow)
    {
        if(gameOverPanel)
        {
            AudioManager.instance.Pause();
            AudioManager.instance.PlaySFX("Lose");
            gameOverPanel.SetActive(isShow);
        }
    }
    public void ShowGameWiner(bool isShow)
    {
        if (winerPanel)
        {
            AudioManager.instance.Pause();
            AudioManager.instance.PlaySFX("Complete");
            winerPanel.SetActive(isShow);
        }
    }

    public void OnClickLoadScene(int i)
    {
        AudioManager.instance.PlaySFX("Click");
        TransitionManager.Instance.TransitionToScene(i);
    }

}
