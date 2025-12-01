using UnityEngine;

[System.Serializable]
public class CharacterScript : MonoBehaviour
{
    public string characterName;
    [TextArea] public string text;
    public Sprite portrait;
}

[CreateAssetMenu(menuName = "VN/Dialogue Sequence")]
public class DialogueSequence : ScriptableObject
{
    public CharacterScript[] lines;
}