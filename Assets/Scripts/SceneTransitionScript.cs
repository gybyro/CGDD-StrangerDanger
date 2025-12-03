using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string sceneNameToLoad;
    
    public void LoadScene()
    {
        SceneManager.LoadScene(sceneNameToLoad);
        Debug.Log($"Scene {sceneNameToLoad} loaded");
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the thing entering is the player
        if (other.CompareTag("Player"))
        {
            //GameManager.Instance.FadeToScene(sceneNameToLoad);
            SceneManager.LoadScene(sceneNameToLoad);
        }
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit called! (This won't close the editor)");
    }
}
