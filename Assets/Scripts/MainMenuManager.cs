using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public string toLoad;
    
    public void LoadScene()
    {
        SceneManager.LoadScene(toLoad);
        Debug.Log($"Scene {toLoad} loaded");
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit called! (This won't close the editor)");
    }
}
