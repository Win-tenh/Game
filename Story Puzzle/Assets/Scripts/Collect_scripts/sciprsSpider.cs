using UnityEngine;

public class sciprsSpider : MonoBehaviour
{
    GameController m_gc;
    private void Start()
    {
        m_gc = FindAnyObjectByType<GameController>();

    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Qua bong da va cham voi gia do");
            m_gc.DecrementHealth();
            Destroy(gameObject);

        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("DeathZone"))
        {
            //Debug.Log("Qua bong da va cham voi deathzone, Gameover");
            Destroy(gameObject);
        }
    }
}
