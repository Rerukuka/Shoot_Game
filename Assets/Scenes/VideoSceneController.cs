using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.Collections;

public class VideoSceneController : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer.loopPointReached += OnVideoFinished;
        StartCoroutine(SkipAfterTime(27f)); // если вдруг видео не вызовет завершение
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        LoadCadScene();
    }

    IEnumerator SkipAfterTime(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        LoadCadScene();
    }

    void LoadCadScene()
    {
        SceneManager.LoadScene("CadScene");
    }
}
