using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{

    public void StartGame()
    {
        SceneManager.LoadScene("Packing"); // Укажите имя или индекс игровой сцены
    }

    public void OpenSettings()
    {
        SceneManager.LoadScene("SettingsMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ToMiniGame()
    {
        SceneManager.LoadScene("Town");
    }
}
