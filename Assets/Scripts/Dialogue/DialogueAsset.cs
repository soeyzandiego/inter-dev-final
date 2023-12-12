using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New DialogueAsset", menuName = "New DialogueAsset")]
// an asset that stores a conversation
public class DialogueAsset : ScriptableObject
{
    // an asset that stores a unique ID for the line, who says it, and if it's locked 
    [System.Serializable]
    public class DialogueLine
    {
        public DialogueCharacter character;
        [TextArea(6, 6)] public string dialogue;
        public DialogueChoice[] choices; // can be empty
        public string unlockID; // can be empty
    }

    [System.Serializable]
    public class DialogueChoice
    {
        public string text;
        public string fullText;
    }

    // if a logic puzzle will play at the end of this dialogue
    public GameObject puzzle;

    //public DialogueLine[] lines;
    public List<DialogueLine> lines;
}
