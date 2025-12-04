using UnityEngine;

public class FogSystem : MonoBehaviour
{
    public float fogStrength = 0f;  // 0 = no fog, 1 = full white

    

    private void OnTriggerStay(Collider other)
    {
        FogSystem fog = other.GetComponent<FogSystem>();
        if (fog != null)
        {
            fogStrength = Mathf.MoveTowards(fogStrength, 1f, Time.deltaTime * 0.5f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        FogSystem fog = other.GetComponent<FogSystem>();
        if (fog != null)
        {
            fogStrength = 0f;
        }
    }
}
