using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public CanvasGroup fadeGroup;
    public float fadeTime = 1f;

    void Awake()
    {
        Instance = this;
        // if (Instance == null)
        // {
        //     Instance = this;
        //     DontDestroyOnLoad(gameObject);
        // }
        // else
        // {
        //     Destroy(gameObject);
        //     return;
        // }
    }

    void Start()
    {
        // Start fully black and fade out
        fadeGroup.alpha = 0;
        // StartCoroutine(FadeOut());
    }

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