using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New SuspectFile", menuName = "New SuspectFile")]
public class SuspectFile : ScriptableObject
{
    [System.Serializable]
    public class SuspectClue
    {
        [SerializeField] public string unlockId;
        [TextArea(3, 5)] public string text;
        [SerializeField] public Sprite picture;
    }

    [SerializeField] public string suspectName;
    [SerializeField] public Sprite sprite;
    [TextArea(3,5)] [SerializeField] public string quote;
    [SerializeField] public SuspectClue[] clues;

    [Header("Challenge Information")]
    [SerializeField] public string challengeUnlockID;
    [SerializeField] public string challengeSolvedID;
    [TextArea(4,4)] [SerializeField] public string challengeUnlocked;
    [TextArea(4, 4)] [SerializeField] public string challengeSolved;


    // if restructuring so DialogueClick points to a SuspectFile
    //[Header("Dialogue")]
    //public DialogueAsset defaultAsset;
    //public DialogueAsset yourJobAsset;
}
