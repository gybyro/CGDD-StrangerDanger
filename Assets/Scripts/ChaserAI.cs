using UnityEngine;

public class ChaserAI : MonoBehaviour
{
    public Transform player; 
    public float speed = 3f;
    public float chaseDistance = 20f;
    public AudioSource audioSource; // plays when player hits chaseDistance

    private float lockedY; // the Y position we want to keep

    void Start()
    {
        lockedY = transform.position.y;  // save starting height
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);

        // Only chase if within range
        if (dist < chaseDistance)
        {
            // sound
            audioSource.PlayOneShot(audioSource.clip);


            // Move toward player
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0;

            transform.position += direction * speed * Time.deltaTime;

            // Lock Y axis
            transform.position = new Vector3(
                transform.position.x,
                lockedY,
                transform.position.z
            );
        }
    }
}