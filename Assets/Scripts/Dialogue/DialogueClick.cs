using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueClick : MonoBehaviour
{
    public DialogueAsset asset;

    public DialogueAsset yourJobAsset;
    public DialogueAsset lesterBeaumontAsset;
    public DialogueUnlockable[] unlockables;

    [System.Serializable]
    public class DialogueUnlockable
    {
        public string investigatePanelText;
        public DialogueAsset dialogue;
        public string[] unlockIds;
        public bool challenge = false;
    }

    int lineIndex = 0;

    public GameObject sidebarButton;

    void Start()
    {

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
