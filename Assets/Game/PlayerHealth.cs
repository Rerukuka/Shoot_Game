using UnityEngine;
using TMPro;
using System.Collections;

using UnityEngine.SceneManagement;
using UnityEngine.UI; // для Image


public class PlayerHealth : MonoBehaviour
{
    public float maxHP = 100;
    public float currentHP;
    public TextMeshProUGUI hpText;
    public Image victoryFadePanel;
    public TextMeshProUGUI victoryText;
    private bool victoryTriggered = false;

    public TextMeshProUGUI timerText;
    public GameObject deathUI;
    public Image deathFadePanel;
    public Animator animator;

    public TextMeshProUGUI deathText;



    private float timer = 0f;
    private bool isDead = false;

    void Start()
    {
        currentHP = maxHP;
        hpText.text = $"HP: {currentHP}";
        deathUI.SetActive(false); // Скрыта до смерти
        victoryText.gameObject.SetActive(false);
        deathText.gameObject.SetActive(false); // ← ЭТА СТРОКА отключает текст
    }

    void Update()
    {
        if (isDead) return;

        timer += Time.deltaTime;
        timerText.text = $"Time: {timer:F1}";

        if (!victoryTriggered && timer >= 60f)
        {
            victoryTriggered = true;
            StartCoroutine(TriggerVictory());
        }
    }

    public void TakeDamage(float amount)
    {
        currentHP -= amount;
        hpText.text = $"HP: {currentHP}";

        if (currentHP <= 0 && !isDead)
        {
            Die();
        }
    }

    void Die()
    {
        animator.SetFloat("Speed", 0f);
        animator.SetFloat("MoveX", 0f);
        animator.SetFloat("MoveZ", 0f);
        animator.SetBool("IsRunning", false);

        isDead = true;
        Time.timeScale = 0f;
        timerText.color = Color.red;
        animator.SetTrigger("Die");
        deathUI.SetActive(true);
        StartCoroutine(DeathSequence());
        deathText.gameObject.SetActive(true);
        animator.Play("Die");
        GetComponent<PlayerController_NewInput>()?.SetDead();






    }

    IEnumerator DeathSequence()
    {
        Time.timeScale = 0.01f; // почти остановка

        // Показать текст
        deathText.gameObject.SetActive(true);

        float duration = 3f;
        float elapsed = 0f;

        Color startColor = new Color(0, 0, 0, 0);
        Color endColor = new Color(0, 0, 0, 1);

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            deathFadePanel.color = Color.Lerp(startColor, endColor, t);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        deathFadePanel.color = endColor;

        // Переключение сцены (нормальная скорость)
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuScene");

    }
    IEnumerator TriggerVictory()
    {   
        victoryText.gameObject.SetActive(true);

        float duration = 3f;
        float elapsed = 0f;

        Color startColor = new Color(0, 0, 0, 0);
        Color endColor = new Color(0, 0, 0, 1);

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            victoryFadePanel.color = Color.Lerp(startColor, endColor, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        victoryFadePanel.color = endColor;

        // Можно потом вернуться в меню, если хочешь:
        // SceneManager.LoadScene("MenuScene");
    }


}
