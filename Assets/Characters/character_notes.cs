

// character named "You" thats the player speaking

// dialouge data can contain:
    // id - name of dialouge portion
    // speaker - who is speaking
    // text - a single string
    // portrait - full body character Sprite - image? not sprite renderer
    // type - can be "random" or "choice"

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
// next - the next dialouge data after pressing space
// "finished": true  -  if this is the last dialouge data in the json file.


// =================================================================================================
// Dialogue Data Classes for the DialogueManager to parse the json

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
    public string text;
    public string portrait;
    public string sound;
    public string next;
    public bool finished;

    public string type;      // "choice", "random", or null
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
