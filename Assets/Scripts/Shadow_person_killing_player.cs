using UnityEngine;

public class Kill_Player : MonoBehaviour
{
    public GameObject Game_Over_Panel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D (Collider2D other) 
    {
       if (other.CompareTag("Player")) 
       {
            Game_Over_Panel.SetActive(true);
       } 
    }

}
