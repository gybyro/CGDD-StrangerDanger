using UnityEngine;

[CreateAssetMenu(menuName = "VN/Character Database")]
public class CharacterDatabase : ScriptableObject
{
    public Character[] characters;

    public Character GetCharacter(string id)
    {
        foreach (var c in characters)
            if (c.characterID == id)
                return c;

        return null;
    }
}