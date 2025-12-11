using UnityEngine;

public class CarPhaseManager : MonoBehaviour
{
    public GameObject[] phaseObjects;  // phase 0, 1, 2, ...

    private void Start()
    {
        int phase = 0;
        if (GameManager.Instance != null)
        {
            phase = GameManager.Instance.carPhase;
        }

        if (phaseObjects == null || phaseObjects.Length == 0)
            return;

        phase = Mathf.Clamp(phase, 0, phaseObjects.Length - 1);

        for (int i = 0; i < phaseObjects.Length; i++)
        {
            if (phaseObjects[i] != null)
                phaseObjects[i].SetActive(i == phase);
        }

        Debug.Log("[CarPhaseController] Enabled object index: " + phase);
    }
}
