
using UnityEngine;

public class Ball : MonoBehaviour
{
    GameController m_gc;
    private void Start()
    {
        m_gc = FindAnyObjectByType<GameController> ();
   
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            // va cham
            //Debug.Log("Qua bong da va cham voi gia do");
            m_gc.IncrementScore();
            Destroy(gameObject);

        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("DeathZone"))
        {
            //Debug.Log("Qua bong da va cham voi deathzone, Gameover");
            m_gc.DecrementHealth();
            Destroy(gameObject);
        }
    }
}
