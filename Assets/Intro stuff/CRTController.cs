using UnityEngine;
using UnityEngine.Rendering;
using RetroShadersPro.URP;
using System.Collections;



public class CRTController : MonoBehaviour
{
    public Volume volume;  // Assign Global Volume here
    private CRTSettings crt;  // The override from Retro Shaders Pro
    private CRTPreset currentPreset;

    void Start()
    {
        // Get your CRT override from the volume profile
        volume.profile.TryGet(out crt);
    }

    public void ToggleCRT(bool on)
    {
        if (on) crt.active = true;  // turn on
        else crt.active = false;    // turn off
    }

    public void FadePixelSize() {}

    public CRTPreset lowPreset = new CRTPreset
    {
        pixelSize = 1,
        distortionStrength = 0,
        distortionSmoothing = 0,
        rgbStrength = 0,
        scanlineStrength = 0,
        scanlineSize = 0,
        randomWear = 0,
        aberrationStrength = 0,
        trackingJitter = 0,
        trackingStrength = 0,
        trackingColorDamage = 0,
        contrast = 1,
        brightness = 1
    };

    public CRTPreset highPreset = new CRTPreset
    {
        pixelSize = 108,
        distortionStrength = 1,
        distortionSmoothing = 0.02f,
        rgbStrength = 1,
        scanlineStrength = 1,
        scanlineSize = 1,
        randomWear = 5,
        aberrationStrength = 10,
        trackingJitter = 0.1f,
        trackingStrength = 50,
        trackingColorDamage = 1,
        contrast = 1,
        brightness = 1
    };

    public CRTPreset goodPreset = new CRTPreset
    {
        pixelSize = 2,
        distortionStrength = 1,
        distortionSmoothing = 0.01f,
        rgbStrength = 0.25f,
        scanlineStrength = 0.2f,
        scanlineSize = 8,
        randomWear = 3,
        aberrationStrength = 10,
        trackingJitter = 0.001f,
        trackingStrength = 10,
        trackingColorDamage = 1,
        contrast = 0,
        brightness = 2
    };

    public IEnumerator FadeCRT(CRTPreset from, CRTPreset to, float duration)
    {
        float t = 0;

        while (t < duration)
        {
            t += Time.deltaTime;
            float lerp = t / duration;

            crt.pixelSize.value = (int)Mathf.Lerp(from.pixelSize, to.pixelSize, lerp);
            crt.distortionStrength.value = Mathf.Lerp(from.distortionStrength, to.distortionStrength, lerp);
            crt.distortionSmoothing.value = Mathf.Lerp(from.distortionSmoothing, to.distortionSmoothing, lerp);
            crt.rgbStrength.value = Mathf.Lerp(from.rgbStrength, to.rgbStrength, lerp);
            crt.scanlineStrength.value = Mathf.Lerp(from.scanlineStrength, to.scanlineStrength, lerp);
            crt.scanlineSize.value = (int)Mathf.Lerp(from.scanlineSize, to.scanlineSize, lerp);
            crt.randomWear.value = Mathf.Lerp(from.randomWear, to.randomWear, lerp);
            crt.aberrationStrength.value = Mathf.Lerp(from.aberrationStrength, to.aberrationStrength, lerp);
            crt.trackingJitter.value = Mathf.Lerp(from.trackingJitter, to.trackingJitter, lerp);
            crt.trackingStrength.value = Mathf.Lerp(from.trackingStrength, to.trackingStrength, lerp);
            crt.trackingColorDamage.value = Mathf.Lerp(from.trackingColorDamage, to.trackingColorDamage, lerp);
            crt.contrast.value = Mathf.Lerp(from.contrast, to.contrast, lerp);
            crt.brightness.value = Mathf.Lerp(from.brightness, to.brightness, lerp);

            yield return null;
        }
        currentPreset = to;
    }

    public Coroutine FadeInCRT(float duration) { return StartCoroutine(FadeCRT(lowPreset, highPreset, duration)); }
    public Coroutine FadeOutCRT(float duration) { return StartCoroutine(FadeCRT(highPreset, lowPreset, duration)); }
    public Coroutine FadeLowToGood(float duration) { return StartCoroutine(FadeCRT(lowPreset, goodPreset, duration)); }
    public Coroutine FadeGoodToHigh(float duration) { return StartCoroutine(FadeCRT(goodPreset, highPreset, duration)); }
    

}