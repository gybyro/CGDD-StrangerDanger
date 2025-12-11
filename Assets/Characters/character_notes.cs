

// character named "You" thats the player speaking

// dialouge data can contain:
    // id - name of dialouge portion
    // speaker - who is speaking
    // text - a single string
    // color - the text color in hex - unless specified, text color=white, prompt can also be colored but choices and options are not
    // portrait - full body character Sprite - image? not sprite renderer
    // type - can be "random" or "choice"
    // waitSeconds - time the box must wait?
    // animationTriggerDoor - the animation trigger for the door
    // point - character points

// "choices": [
    //     { "weight": 1, "next": "john_intro" },
    //     { "weight": 1, "next": "mary_intro" },
    //     { "weight": 1, "next": "leo_intro" }
    //   ] - this is type "random"?

// "options": [
    // {"text": "Ask politely", "next": "ask_politely"},
    // {"text": "Demand directions","next": "ask_rude"}
    // this was "type": "choice",    "prompt": "How do you respond?",

// sound
// next - the next dialouge line/data after pressing space
// "finished": true  -  if this is the last dialouge data in the scene.
// nextScene - determines your next scene....

// trigger - optional if we want to get rid of the pizza box....
    // hidePizzabox
    // showPizzabox



// =================================================================================================
// Dialogue Data Classes for the DialogueManager to parse the json


// pizza proxy likes:
// a large mexican style?

// proxy sprite sheet:
// openDoor1
// openDoor2
// pizzaUp
// pizzaSide1
// pizzaSide2
// happy
// visitorOpen
// visitorUp
// visitor1
// visitor2
// visitor3

using UnityEngine;
using System;

[Serializable]
public class DialogueFile  // json file - called "dial_..."
{
    public string scene;
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
    public string showChar;  // character.Show() or character.Hide()
    public float waitSeconds;
    public float typeSpeed;
    public string next;
    public int tips;
    public string nextDialFile;
    public bool finished;

    public string type;      // "choice", "random", or null
    public string prompt;
    public ChoiceOption[] options;
    public RandomChoice[] choices;
}
[Serializable]
public class ChoiceOption {
    public string text;
    public string next;
}
[Serializable]
public class RandomChoice {
    public int weight;
    public string next;
}


//// for door bg
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
    public DayInWeek monday;
    public DayInWeek tuesday;
    public DayInWeek wednsday;
    public DayInWeek thursday;
    public DayInWeek friday;
    public DayInWeek saturday;
    public DayInWeek sunday;
}

[Serializable]
public class DayInWeek
{
    public TimeOfDay morning;
    public TimeOfDay eve;
    public TimeOfDay dusk;
    public TimeOfDay midnight;
    public TimeOfDay deep;
}

[Serializable]
public class TimeOfDay
{
    public PlayerDialogueLine[] currentLinesToPlay;
    // more to be added maybe
}

[Serializable]
public class PlayerDialogueLine
{
    public string id;
    public Color color = Color.white;
    [TextArea(2, 5)]
    public string text;
    public AudioClip sound;
    public float waitSeconds;
    public float typeSpeed;
    public bool next;
    public bool end;
}

