using UnityEngine;

public class DoorBGPallet : MonoBehaviour
{
    [Header("Door BG objects / SpriteRenderers")]
    public SpriteRenderer doorSR;
    public SpriteRenderer outerWallSR;
    public SpriteRenderer innerWallSR;
    public SpriteRenderer floorSR;

    [Header("Color Sets")]
    public SpriteColorSet[] colorSets;

    public void ApplyColorSet(string setName)
    {
        foreach (var set in colorSets)
        {
            if (set.setName == setName)
            {
                doorSR.sprite = set.doorS;
                outerWallSR.sprite = set.outerWallS;
                innerWallSR.sprite = set.innerWallS;
                floorSR.sprite = set.floorS;
                return;
            }
        }
        Debug.LogWarning("No color set found with name: " + setName);
    }
}