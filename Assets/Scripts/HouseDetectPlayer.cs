using UnityEngine;

public class TriggerLoadScene : MonoBehaviour
{
    public SceneTransition sceneTransition;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            sceneTransition.LoadScene("HouseScene");
        }
    }
}
