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

[CreateAssetMenu(menuName = "VN/Character Sprite Database")]
public class CharacterSpriteDatabase : ScriptableObject
{
    public CharacterPortraitData[] characters;

    public Sprite GetDefaultPortrait(string characterID)
    {
        foreach (var c in characters)
        {
            if (c.characterID == characterID)
                return c.defaultPortrait;
        }
        Debug.LogWarning("No default portrait found for: " + characterID);
        return null;
    }

    public Sprite GetPortrait(string characterID, string emotion)
    {
        foreach (var c in characters)
        {
            if (c.characterID == characterID)
            {
                foreach (var e in c.expressions)
                {
                    if (e.emotion == emotion)
                        return e.portrait;
                }

                Debug.LogWarning($"No portrait for emotion '{emotion}' on character '{characterID}'");
                return c.defaultPortrait;
            }
        }

        Debug.LogWarning("Character not found in database: " + characterID);
        return null;
    }
}

[System.Serializable]
public class CharacterPortraitData
{
    public string characterID; // "john"
    public Sprite defaultPortrait;
    public CharacterExpressionData[] expressions;
}

[System.Serializable]
public class CharacterExpressionData
{
    public string emotion; // "happy", "sad", "angry"
    public Sprite portrait;
}