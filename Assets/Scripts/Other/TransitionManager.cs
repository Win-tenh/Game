using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance; // Singleton để truy cập dễ dàng
    
    public Animator transition; // Gắn Animator của Transition Canvas
    public float transtionTime = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    //Gọi hiệu ứng fade-out và chuyển scene
    public void TransitionToScene(int scene)
    {
        StartCoroutine(Transition(scene));
    }

    private IEnumerator Transition(int scene)
    {
        // Kích hoạt animation fade-out
        AudioManager.instance.PlaySFX("Leaf");
        transition.SetTrigger("End");

        // Chờ animation hoàn tất
        yield return new WaitForSeconds(transtionTime); // Thời gian khớp với animation

        // Chuyển scene
        SceneManager.LoadScene(scene);

        // Kích hoạt animation fade-in
        AudioManager.instance.PlaySFX("Leaf");
        transition.SetTrigger("Start");

    }
}
