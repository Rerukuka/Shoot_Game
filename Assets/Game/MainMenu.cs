using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("DemoScene01"); // Название игровой сцены
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
