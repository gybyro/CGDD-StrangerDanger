using UnityEngine;

public class InsanityCameraShake : MonoBehaviour
{
    public float duration = 0f;
    public float intensity = 0.2f;
    public float rotationIntensity = 15f;
    public float decay = 1f;

    private Vector3 originalPos;
    private Quaternion originalRot;

    void Start()
    {
        originalPos = transform.localPosition;
        originalRot = transform.localRotation;
    }

    void Update()
    {
        if (duration > 0)
        {
            float shake = Mathf.PerlinNoise(Time.time * 10, 0) * intensity;

            // Position shake
            transform.localPosition = originalPos + new Vector3(
                Random.Range(-shake, shake),
                Random.Range(-shake, shake),
                0
            );

            // Slight rotation shake (feels more psychological)
            transform.localRotation = Quaternion.Euler(
                originalRot.eulerAngles.x + Random.Range(-rotationIntensity, rotationIntensity),
                originalRot.eulerAngles.y + Random.Range(-rotationIntensity, rotationIntensity),
                originalRot.eulerAngles.z
            );

            duration -= Time.deltaTime * decay;
        }
        else
        {
            duration = 0;
            transform.localPosition = originalPos;
            transform.localRotation = originalRot;
        }
    }

    public void Trigger(float time)
    {
        duration = time;
    }
}
