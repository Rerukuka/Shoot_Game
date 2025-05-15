using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private static MusicPlayer instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Не уничтожать между сценами
        }
        else
        {
            Destroy(gameObject); // Удалить дубликаты
        }
    }
}
