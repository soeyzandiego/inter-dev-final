using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueClick : MonoBehaviour
{
    public DialogueAsset asset;

    public DialogueAsset yourJobAsset;
    // challenege conversation to be unlocked
    public DialogueAsset[] toUnlock;

    Dictionary<DialogueAsset, bool> lockedDialogue = new Dictionary<DialogueAsset, bool>();
    int lineIndex = 0;

    public GameObject sidebarButton;

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
        if(sidebarButton.activeSelf)
        {
            sidebarButton.SetActive(false);
        }
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

    public void Unlock(DialogueAsset assetToUnlock)
    {
        if (lockedDialogue.ContainsKey(assetToUnlock))
        {
            lockedDialogue[assetToUnlock] = true;
        }
    }
}
