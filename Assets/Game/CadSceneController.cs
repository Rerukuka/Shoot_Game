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
        // ��� 1 ��� � ������� ������
        yield return new WaitForSeconds(0f);
        yield return StartCoroutine(FadeFromBlack());

        // �������� ���-����� 10 ���
        yield return new WaitForSeconds(4f);

        // ����� ����������
        yield return StartCoroutine(FadeToBlack());

        // ������� � ������� �����
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
