using System;
using UnityEngine;

[Serializable]
public class DialogueFile
{
    public string scene;
    public string houseBgPallet;
    public DialogueLine[] lines;
}

[Serializable]
public class DialogueLine
{
    public string id;
    public string speaker;
    public string color;
    public string text;
    public string portrait;
    public string sound;
    public string trigger;
    public string animationTriggerDoor;
    public string showChar;
    public float waitSeconds;
    public float typeSpeed;
    public string next;
    public int tips;
    public string nextDialFile;
    public bool finished;

    public int sanity;

    public bool timerStart;
    public float countDownSeconds;
    public string timeOutNext;
    public string escapeNext;
    public string escapeText;

    public string type;
    public string prompt;
    public ChoiceOption[] options;
    public RandomChoice[] choices;
}

[Serializable]
public class ChoiceOption
{
    public string text;
    public string next;
}

[Serializable]
public class RandomChoice
{
    public int weight;
    public string next;
}

[Serializable]
public class SpriteColorSet
{
    public string setName;
    public Sprite doorS;
    public Sprite outerWallS;
    public Sprite innerWallS;
    public Sprite floorS;
}

[Serializable]
public class InGameWeek
{
    public DayInWeek[] dayInWeek;
}

[Serializable]
public class DayInWeek
{
    public TimeSlot[] timeSlots;
}

[Serializable]
public class TimeSlot
{
    public PlayerDialogueLine[] currentLinesToPlay;
}

[Serializable]
public class PlayerDialogueLine
{
    public Color color = Color.white;

    [TextArea(2, 5)]
    public string text;

    public AudioClip sound;
    public float waitSeconds;
    public float typeSpeed;
}

[Serializable]
public struct CRTPreset
{
    public float pixelSize;
    public float distortionStrength;
    public float distortionSmoothing;
    public float rgbStrength;
    public float scanlineStrength;
    public float scanlineSize;
    public float randomWear;
    public float aberrationStrength;
    public float trackingJitter;
    public float trackingStrength;
    public float trackingColorDamage;
    public float contrast;
    public float brightness;
}