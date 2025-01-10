using UnityEngine;

public class GameController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created 
    private static System.Random random = new System.Random();
    public GameObject ball;
    public GameObject health;
    public GameObject spider;
    public float SpawnTime;
    float m_spawnTime;
    int m_score;
    bool m_isWinter;
    int m_health;
    UI m_ui;
    int b;
    int h;
    int s;
    int target;
    private bool isEnd;

    void Start()
    {
        target = (PlayerPrefs.GetInt("ChapterSelected", 1) + 1) * 3; 
        h = 2;
        b = target+3+h;
        s = 9;
        SpawnTime = 0.5f;
        m_health = 3;
        m_spawnTime = 0;
        m_score = 0;
        m_isWinter = false;
        m_ui = FindAnyObjectByType<UI>();
        m_ui = FindAnyObjectByType<UI>();
        m_ui.SetScoreText(m_score, target);
        m_ui.SetHealthText("Health: " + m_health);

        isEnd = false;
        AudioManager.instance.UnPause();
    }

    // Update is called once per frame
    void Update()
    {
        int ob=0;            
        m_spawnTime -= Time.deltaTime;
        if (m_health==0)
        {
            m_spawnTime = 0;
            if (!isEnd)
                m_ui.ShowGameOver(true);
            isEnd = true;
            return;
        }
        if (m_isWinter)
        {
            m_spawnTime = 0;
            if (!isEnd)
                m_ui.ShowGameWiner(true);
            isEnd = true;
            return;
        }
        if (m_spawnTime <= 0)
        {
            m_spawnTime = SpawnTime;
            if (b > 0)
            {
                ob = GetRandomObject(b, s, h);
            }
            if(m_score == target)
            {
                m_isWinter = true;
            }
            if (ob == 0)
            {
                SpawnBall();
                b -= 1;
            }
            if (ob == 1)
            {
                SpawnSpider();
                SpawnSpider();
                SpawnSpider();
                SpawnSpider();
                SpawnSpider();
                s -= 1;
            }
            if(ob ==2)
            {
                SpawnHealth();
                h -= 1;
            }
            
        }
    }

    public void SetScore(int value)
    {
        m_score= value;
    }
    public int GetScore() { return m_score; }
    public int GetHealth() { return m_health; }

    public bool Getwinter() { return m_isWinter;}

    public void IncrementScore()
    {
        if (m_health > 0 && !m_isWinter)
        {
            AudioManager.instance.PlaySFX("CorrectTile");
            m_score += 1;
            m_ui.SetScoreText(m_score, target);
        }
    }
    public void IncrementHealth()
    {
        if (m_health > 0 && !m_isWinter)
        {
            AudioManager.instance.PlaySFX("Heal");
            m_health += 1;
            m_ui.SetHealthText("Health: " + m_health);
        }
    }

    public void DecrementHealth()
    {
        if (m_health > 0 && !m_isWinter)
        {
            AudioManager.instance.PlaySFX("Wrong");
            m_health -= 1;
            m_ui.SetHealthText("Health: " + m_health);
        }

    }

    public void SpawnBall()
    {
        Vector2 spawnPos = new Vector2(Random.Range(-7, 7), 6);
        if (ball)
        {
            Instantiate(ball, spawnPos, Quaternion.identity);
        }
    }

    public int GetRandomObject(int count0, int count1, int count2)
    {
        int total = count0 + count1 + count2;
        double randomValue = random.NextDouble() * total; 

        if (randomValue < count0)
            return 0; 
        else if (randomValue < count0 + count1)
            return 1;
        else
            return 2; 
    }

    public void SpawnHealth()
    {
        Vector2 spawnPos = new Vector2(Random.Range(-7, 7), 6);
        if (ball)
        {
            Instantiate(health, spawnPos, Quaternion.identity);

        }
    }

    public void SpawnSpider()
    {
        Vector2 spawnPos = new Vector2(Random.Range(-7, 7), 6);
        if (ball)
        {
            Instantiate(spider, spawnPos, Quaternion.identity);

        }
    }

    // lose
    public void Replay()
    {
        //Debug.Log("Replay button clicked!");
        m_ui.OnClickLoadScene(2);
    }

    // complete
    public void Winter()
    {
        //Debug.Log("you win clicked!");
        AudioManager.instance.PlaySFX("Click");
        m_ui.OnClickLoadScene(1);
    }

    // home
    public void Home()
    {
        //Debug.Log("Home button clicked!");
        m_ui.OnClickLoadScene(0);
    }

}
