using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    public string sceneNameToLoad = "MainMenu";
    public CanvasGroup fadeGroup;
    public float fadeTime = 1f;

    private bool isTransitioning;
    
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(
            string.IsNullOrEmpty(sceneName) ? sceneNameToLoad : sceneName
        );
    }

    public void LoadSceneWithFade(string sceneName) 
    {
        if (isTransitioning) return;

        string target = string.IsNullOrEmpty(sceneName)
            ? sceneNameToLoad
            : sceneName;

        FadeToScene(target);
    }

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
        isTransitioning = true;
        fadeGroup.blocksRaycasts = true;

        float t = 0;

        while (t < fadeTime)
        {
            t += Time.deltaTime;
            fadeGroup.alpha = t / fadeTime;
            yield return null;
        }
        fadeGroup.alpha = 1f;

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
            fadeGroup.alpha = 1f - (t / fadeTime);
            yield return null;
        }
        fadeGroup.alpha = 0f;
        fadeGroup.blocksRaycasts = false;
        isTransitioning = false;
    }
}
