using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New SuspectFile", menuName = "New SuspectFile")]
public class SuspectFile : ScriptableObject
{
    [System.Serializable]
    public class SuspectClue
    {
        public string unlockId;
        [TextArea(3, 5)] public string text;
        public Sprite picture;
    }

    public string suspectName;
    public Sprite sprite;
    [TextArea(3,5)] public string quote;
    public SuspectClue[] clues;

    [Header("Challenge Information")]
    public string challengeUnlockID;
    public string challengeSolvedID;
    [TextArea(4,4)] public string challengeUnlocked;
    [TextArea(4, 4)] public string challengeSolved;


    // if restructuring so DialogueClick points to a SuspectFile
    //[Header("Dialogue")]
    //public DialogueAsset defaultAsset;
    //public DialogueAsset yourJobAsset;
}
