using UnityEngine;

public class Moving_forward_event : MonoBehaviour
{
    public float speed = 5f;
    Vector3 curr_position;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        curr_position = transform.position;
    }


    public void Button_click_right () {
        curr_position = transform.position;
        curr_position.x = curr_position.x + speed * Time.deltaTime;
        transform.position = curr_position;
    }
}

