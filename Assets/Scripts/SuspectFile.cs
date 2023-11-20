using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuspectFile : MonoBehaviour
{
    [System.Serializable]
    public class SuspectClue
    {
        public string unlockId;
        public string text;
        public Sprite image;
    }

    public string suspectName;
    public string quote;
    public SuspectClue[] clues;
}
