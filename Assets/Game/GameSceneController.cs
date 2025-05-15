using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameSceneController : MonoBehaviour
{
    public Image fadePanel;
    public float fadeDuration = 2f;

    void Start()
    {
        StartCoroutine(FadeFromBlack());
    }

    public void EndGame()
    {
        StartCoroutine(FadeAndReturnToMenu());
    }

    IEnumerator FadeFromBlack()
    {
        float t = 0;
        Color start = new Color(0, 0, 0, 1);
        Color end = new Color(0, 0, 0, 0);
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadePanel.color = Color.Lerp(start, end, t / fadeDuration);
            yield return null;
        }
        fadePanel.color = end;
    }

    IEnumerator FadeAndReturnToMenu()
    {
        yield return StartCoroutine(FadeToBlack());
        SceneManager.LoadScene("MenuScene");
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
