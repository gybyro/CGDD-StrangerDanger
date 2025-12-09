using UnityEngine;

[CreateAssetMenu(menuName = "VN/Character Database")]
public class CharacterDatabase : ScriptableObject
{
    [Header("Character Prefabs")]
    public GameObject[] characterPrefabs;   // drag prefabs here, NOT scene objects

    public Character GetCharacter(string id)
    {
        foreach (var prefab in characterPrefabs)
        {
            if (prefab == null) continue;

            Character c = prefab.GetComponent<Character>();

            if (c != null && c.characterID == id)
                return c;
        }

        Debug.LogWarning("Character not found in database: " + id);
        return null;
    }

    public int GetIndex(Character character)
    {
        if (character == null) return -1;

        for (int i = 0; i < characterPrefabs.Length; i++)
        {
            if (characterPrefabs[i] == character)
                return i;
        }

        return -1; // not found
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