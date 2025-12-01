using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Camera cam;

    void Start()
    {
        if (cam == null)
            cam = Camera.main;
    }

    void LateUpdate()
    {
        Vector3 direction = cam.transform.position - transform.position;
        direction.y = 0; // Lock Y-axis
        transform.rotation = Quaternion.LookRotation(direction);
    }
}
