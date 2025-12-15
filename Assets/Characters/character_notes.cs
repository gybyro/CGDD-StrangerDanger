using UnityEngine;
using System;

// [Serializable]
// public class DialogueFile  // json file - called "dial_..."
// {
//     public string scene;

//     // ✅ Added: used by DialogueManager.cs (house background palette)
//     public string houseBgPallet;

//     public DialogueLine[] lines;
// }

// [Serializable]
// public class DialogueLine
// {
//     public string id;
//     public string speaker;
//     public string color;
//     public string text;
//     public string portrait;
//     public string sound;
//     public string trigger;
//     public string animationTriggerDoor;

//     // You use string "true"/"false" in JSON
//     public string showChar;

//     public float waitSeconds;
//     public float typeSpeed;
//     public string next;

//     // Money/tips system
//     public int tips;

//     public string nextDialFile;
//     public bool finished;

//     // ✅ Added: sanity system support (fixes DialogueManager.cs sanity errors)
//     public int sanity;

//     // Countdown system
//     public bool timerStart;
//     public float countDownSeconds;   // e.g. 10
//     public string timeOutNext;       // default path when time ends
//     public string escapeNext;        // optional: if you want a separate escape path
//     public string escapeText;        // optional UI label

//     // Branching types
//     public string type;      // "choice", "random", "countdown"
//     public string prompt;

//     public ChoiceOption[] options;
//     public RandomChoice[] choices;
// }

// [Serializable]
// public class ChoiceOption
// {
//     public string text;
//     public string next;
// }

// [Serializable]
// public class RandomChoice
// {
//     public int weight;
//     public string next;
// }