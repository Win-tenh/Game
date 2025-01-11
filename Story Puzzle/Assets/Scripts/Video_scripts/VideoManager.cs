using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    public static VideoManager instance;

    public Video[] videos;
    public VideoPlayer videoPlayer;
    public RawImage videoDisplay; // Thêm RawImage để hiển thị video

    public GameObject endPanel;
    public Text endText;

    private int index;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void PlayVideo(string name)
    {
        Video v = System.Array.Find(videos, video => video.nameVideo == name);
        if (v == null)
        {
            Debug.LogWarning("Video: " + name + " not found!");
            return;
        }

        // Set video clip và play
        videoPlayer.clip = v.videoClip;
        videoPlayer.Play();

        // Hiển thị video trên RawImage
        if (videoDisplay != null)
        {
            videoDisplay.texture = videoPlayer.targetTexture;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        index = PlayerPrefs.GetInt("ChapterSelected", 1);
        float volumeVideo = PlayerPrefs.GetFloat("musicVolume", 1);

        PlayVideo("Chapter" + index);

        // set volume for video
        videoPlayer.SetDirectAudioVolume(0, volumeVideo);
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    public void SkipVideo()
    {
        AudioManager.instance.PlaySFX("Click");
        StartCoroutine(SkipVideoCoroutine());
    }

    private IEnumerator SkipVideoCoroutine()
    {
        // nếu video chưa phát xong thì dừng video
        if (videoPlayer.isPlaying)
        {
            videoPlayer.Stop();
        }

        // Hiển thị endPanel và cập nhật endText
        endPanel.SetActive(true);
        endText.text = "Kết thúc chương " + index;

        // Tạm dừng 1 giây
        yield return new WaitForSeconds(1f);

        // Chuyển scene
        TransitionManager.Instance.TransitionToScene(0);
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        // Chuyển sang scene menu khi video phát xong
        StartCoroutine(SkipVideoCoroutine());
    }

}
