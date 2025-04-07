using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public void GoMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void GoStartGame()
    {
        SceneManager.LoadScene("MainScene");
    }
}
