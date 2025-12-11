using UnityEngine;

public class ColdShiverCamera : MonoBehaviour
{
    public float shiverIntensity = 0.02f;   // how strong the shiver is
    public float shiverSpeed = 30f;         // how fast the shiver oscillates

    private Vector3 originalPos;

    void Start()
    {
        originalPos = transform.localPosition;
    }

    void Update()
    {
        float x = Mathf.Sin(Time.time * shiverSpeed) * shiverIntensity;
        float y = Mathf.Cos(Time.time * shiverSpeed * 1.3f) * (shiverIntensity * 0.5f);

        transform.localPosition = originalPos + new Vector3(x, y, 0);
    }
}
