using UnityEngine;
using UnityEngine.UI;
using TMPro; // Если ты используешь TextMeshPro

public class HellButtonEffect : MonoBehaviour
{
    public Image buttonImage;
    public Color normalColor = Color.red;
    public Color hoverColor = new Color(1f, 0.3f, 0.1f);
    public AudioSource demonSound;
    public TextMeshProUGUI buttonText; // Ссылка на текст
    public Vector3 normalScale = Vector3.one;
    public Vector3 hoverScale = new Vector3(1.2f, 1.2f, 1f);

    void Start()
    {
        buttonImage.color = normalColor;
        buttonText.rectTransform.localScale = normalScale;
    }

    public void OnHoverEnter()
    {
        buttonImage.color = hoverColor;
        buttonText.rectTransform.localScale = hoverScale;
        demonSound?.Play();
    }

    public void OnHoverExit()
    {
        buttonImage.color = normalColor;
        buttonText.rectTransform.localScale = normalScale;
    }
}
