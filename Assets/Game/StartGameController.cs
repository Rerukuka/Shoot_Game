using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartGameController : MonoBehaviour
{
    public Image fadePanel;
    public float fadeDuration = 2f;

    public void OnStartButtonPressed()
    {
        // 🛑 Удаляем старую музыку
        GameObject musicPlayer = GameObject.Find("MusicPlayer");
        if (musicPlayer != null)
        {
            AudioSource source = musicPlayer.GetComponent<AudioSource>();
            if (source != null)
            {
                StartCoroutine(FadeOutAndDestroy(source, 2f)); // плавно за 2 сек
            }
            else
            {
                Destroy(musicPlayer); // fallback
            }
        }

        // 🔁 Переход с затемнением
        StartCoroutine(FadeAndLoadVideoScene());
    }
    IEnumerator FadeOutAndDestroy(AudioSource audioSource, float duration)
    {
        float startVolume = audioSource.volume;

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, t / duration);
            yield return null;
        }

        audioSource.volume = 0f;
        audioSource.Stop();

        Destroy(audioSource.gameObject); // или gameObject с музыкой
    }



    IEnumerator FadeAndLoadVideoScene()
    {
        yield return StartCoroutine(FadeToBlack());
        SceneManager.LoadScene("VideoScene");
    }

    IEnumerator FadeToBlack()
    {
        float t = 0;
        Color start = new Color(0, 0, 0, 0);
        Color end = new Color(0, 0, 0, 1);
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadePanel.color = Color.Lerp(start, end, t / fadeDuration);
            yield return null;
        }
        fadePanel.color = end;
    }
}
