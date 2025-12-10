using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class RetroToggle : MonoBehaviour
{
    public Volume volume;
    private VolumeComponent retroEffect;

    void Start()
    {
        volume.profile.TryGet(out retroEffect);
    }

    public void SetRetro(bool enabled)
    {
        retroEffect.active = enabled;
    }
}


// TOGGLE FROM GAMEMANAGER
// public bool retroEnabled;

// void OnSceneLoaded(Scene scene, LoadSceneMode mode)
// {
//     retroToggle.SetRetro(retroEnabled);
// }