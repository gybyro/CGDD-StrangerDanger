using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Slider sensitivitySlider;

    private bool isInitializing;

    private void Awake()
    {
        // Remove old inspector hooks and wire in code
        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.RemoveAllListeners();
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        }

        if (sensitivitySlider != null)
        {
            sensitivitySlider.onValueChanged.RemoveAllListeners();
            sensitivitySlider.onValueChanged.AddListener(OnSensitivityChanged);
        }
    }

    private void OnEnable()
    {
        isInitializing = true;

        volumeSlider.SetValueWithoutNotify(GameSettings.Instance.masterVolume);
        sensitivitySlider.SetValueWithoutNotify(GameSettings.Instance.mouseSensitivity);

        isInitializing = false;

        Debug.Log($"SettingsMenu: OnEnable set sliders to vol={volumeSlider.value}, sens={sensitivitySlider.value}");
    }

    public void OnVolumeChanged(float value)
    {
        if (isInitializing) return;

        Debug.Log("SettingsMenu: OnVolumeChanged -> " + value);

        GameSettings.Instance.masterVolume = value;
        GameSettings.Instance.ApplySettings(); // live update
    }

    public void OnSensitivityChanged(float value)
    {
        if (isInitializing) return;

        Debug.Log("SettingsMenu: OnSensitivityChanged -> " + value);

        GameSettings.Instance.mouseSensitivity = value;
        // apply sensitivity wherever you use it
    }
}
