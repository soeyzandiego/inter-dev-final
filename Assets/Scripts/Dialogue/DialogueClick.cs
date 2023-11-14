using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueClick : MonoBehaviour
{
    public DialogueAsset asset;

    public DialogueAsset yourJobDialogue;
    // challenege conversation to be unlocked
    public DialogueAsset[] toUnlock;
    
    // I made this a Dictionary bc I thought there would be multiple locked but I THINK we're
    // just doing the challenge??? idk sorry im lowkey out of the loop -zoey
    Dictionary<DialogueAsset, bool> lockedDialogue = new Dictionary<DialogueAsset, bool>();
    int lineIndex = 0;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(StartDialogue);
        foreach (DialogueAsset section in toUnlock)
        {
            lockedDialogue.Add(section, false);
        }
    }

    // TODO look into, OnMouseDown can't raycast to this collider when the overlay camera is active
    //void OnMouseDown()
    //{
    //    Debug.Log("pressed");
    //    DialogueManager.PlayDialogue(asset, lineIndex);
    //}

    void StartDialogue()
    {
        DialogueManager.PlayDialogue(asset, lineIndex, this);
    }

    public void ExitDialogue(int newIndex)
    {
        lineIndex = newIndex;
    }

    public void SetAsset(DialogueAsset newAsset)
    {
        asset = newAsset;
    }
}
