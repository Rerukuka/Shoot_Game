using UnityEngine;

public class KillstreakManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip kill3Clip;
    public AudioClip kill4Clip;
    public AudioClip kill5Clip;
    public AudioClip kill10Clip;

    private int currentStreak = 0;
    private float lastKillTime = -10f;
    private float maxDelay = 4f; // секунда между убийствами

    public void OnEnemyKilled()
    {
        float now = Time.time;

        if (now - lastKillTime <= maxDelay)
        {
            currentStreak++;
        }
        else
        {
            currentStreak = 1; // новое комбо
        }

        lastKillTime = now;

        // Воспроизводим нужный звук
        switch (currentStreak)
        {
            case 3:
                PlayClip(kill3Clip);
                break;
            case 4:
                PlayClip(kill4Clip);
                break;
            case 5:
                PlayClip(kill5Clip);
                break;
            case 10:
                PlayClip(kill10Clip);
                break;
        }
    }

    void PlayClip(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
