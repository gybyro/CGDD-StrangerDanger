using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string toLoad;

    public void LoadScene()
    {
        SceneManager.LoadScene(toLoad);
        Debug.Log($"Scene {toLoad} loaded");
    }
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        Debug.Log($"Scene {sceneName} loaded");
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit called! (This won't close the editor)");
    }
}
