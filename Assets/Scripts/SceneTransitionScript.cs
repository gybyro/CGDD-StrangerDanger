using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    public string sceneNameToLoad;
    public CanvasGroup fadeGroup;
    public float fadeTime = 1f;
    
    public void LoadScene()
    {
        SceneManager.LoadScene(sceneNameToLoad);
        Debug.Log($"Scene {sceneNameToLoad} loaded");
    }

    public void LoadSceneWithFade() 
    {
        FadeToScene(sceneNameToLoad);
    }


    // private void OnTriggerEnter(Collider other)
    // {
    //     // Check if the thing entering is the player
    //     if (other.CompareTag("Player"))
    //     {
    //         FadeToScene(sceneNameToLoad);
    //         // SceneManager.LoadScene(sceneNameToLoad);
    //     }
    // }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit called! (This won't close the editor)");
    }




    // ========================== Scene Fade =====================================
    public void FadeToScene(string sceneName)
    {
        StartCoroutine(FadeIn(sceneName));
    }

    IEnumerator FadeIn(string sceneName)
    {
        float t = 0;

        while (t < fadeTime)
        {
            t += Time.deltaTime;
            fadeGroup.alpha = t / fadeTime;
            yield return null;
        }

        SceneManager.LoadScene(sceneName);

        // Now fade out after the scene loads
        yield return StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        float t = 0;

        while (t < fadeTime)
        {
            t += Time.deltaTime;
            fadeGroup.alpha = 1 - (t / fadeTime);
            yield return null;
        }
    }
}
