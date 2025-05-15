using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class CadSceneController : MonoBehaviour
{
    public Image fadePanel;
    public float fadeDuration = 2f;

    void Start()
    {
        StartCoroutine(CadSequence());
    }

    IEnumerator CadSequence()
    {
        // Ждём 1 сек и убираем чёрный
        yield return new WaitForSeconds(0f);
        yield return StartCoroutine(FadeFromBlack());

        // Показать кад-сцену 10 сек
        yield return new WaitForSeconds(4f);

        // Снова затемнение
        yield return StartCoroutine(FadeToBlack());

        // Переход в игровую сцену
        SceneManager.LoadScene("DemoScene01");
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
}
