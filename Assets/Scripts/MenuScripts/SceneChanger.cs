using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    // Change the scene based on the name
    public void ChangeScene(string newSceneName)
    {
        try
        {
            SceneManager.LoadScene(newSceneName);
        }
        catch(System.Exception exception)
        {
            Debug.LogError("Couldn't load scene: " + exception.Message);
        }
    }

    // Quit the game
    public void QuitGame()
    {
        Application.Quit();
    }
}
