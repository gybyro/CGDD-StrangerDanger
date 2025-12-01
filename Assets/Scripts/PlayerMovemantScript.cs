using UnityEngine;

public class PlayerMovemantScript : MonoBehaviour
{
    public float PlayerMovemantSpeed;
    public bool PlayerAtHouse = false;
    void Start()
    {
        
    }

    void Update()
    {
        if (!PlayerAtHouse)
        {
            transform.Translate(Vector3.forward * PlayerMovemantSpeed * Time.deltaTime);

        }


    }

    private void onTriggerEnter(Collider other)
    {
        if (other.CompareTag("HouseTrigger"))
        {
            PlayerAtHouse = true;
        }
    }
}
