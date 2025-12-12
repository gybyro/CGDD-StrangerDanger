using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance { get; private set; }

    private const float DEFAULT_VOLUME = 0.8f;
    private const float DEFAULT_SENS   = 1.0f;

    [Range(0f, 1f)] public float masterVolume = DEFAULT_VOLUME;
    [Range(0.1f, 10f)] public float mouseSensitivity = DEFAULT_SENS;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadSettings();
        ApplySettings();
    }

    public void ApplySettings()
    {
        AudioListener.volume = masterVolume;
        Debug.Log($"GameSettings: ApplySettings AudioListener.volume={AudioListener.volume}");
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("volume", masterVolume);
        PlayerPrefs.SetFloat("sens", mouseSensitivity);
        PlayerPrefs.Save();
        Debug.Log($"GameSettings: SaveSettings volume={masterVolume}, sens={mouseSensitivity}");
    }

    private void LoadSettings()
    {
        masterVolume = PlayerPrefs.GetFloat("volume", DEFAULT_VOLUME);
        mouseSensitivity = PlayerPrefs.GetFloat("sens", DEFAULT_SENS);
        Debug.Log($"GameSettings: LoadSettings volume={masterVolume}, sens={mouseSensitivity}");
    }

    public void SetDefaults()
    {
        masterVolume = DEFAULT_VOLUME;
        mouseSensitivity = DEFAULT_SENS;
        ApplySettings();
        Debug.Log("GameSettings: SetDefaults");
    }
}
