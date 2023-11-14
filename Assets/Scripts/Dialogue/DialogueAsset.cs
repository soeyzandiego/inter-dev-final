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
        public string lineID;
        [Space(5)]
        public DialogueCharacter character;
        [TextArea(6, 6)] public string dialogue;
        public bool locked = false;
    }

    public DialogueLine[] lines;
}
