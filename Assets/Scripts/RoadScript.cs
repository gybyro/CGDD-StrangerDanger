using UnityEngine;

public class RoadPieceLooper : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 10f;         // How fast the road moves on the Z axis

    [Header("Loop Settings")]
    public float resetZ = -20f;       // When Z position is less than this -> teleport
    public float startZ = 40f;        // New Z position after teleport

    void Update()
    {
        // Move backwards along Z axis
        transform.Translate(Vector3.back * speed * Time.deltaTime);

        // Check if the road has passed the reset point
        if (transform.position.z <= resetZ)
        {
            Vector3 pos = transform.position;
            pos.z = startZ;           // Teleport forward
            transform.position = pos;
        }
    }
}
