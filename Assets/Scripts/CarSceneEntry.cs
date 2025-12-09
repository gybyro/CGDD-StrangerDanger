using UnityEngine;

public class CarSceneEntry : MonoBehaviour
{
    private void Start()
    {
        if (GameManager.Instance != null)
        {
            if(GameManager.Instance.carPhase > 0)
            {
                GameManager.Instance.AdvanceCarPhase();
            }
            
        }
        else
        {
            Debug.LogWarning("[CarSceneEntry] GameManager.Instance is NULL!");
        }
    }
}
