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

    private void Update()
    {
        // if there's a conversation going on, don't look for dialogue clicks
        if (DialogueManager.state != DialogueManager.DialogueStates.NONE) { return; }
        // if a panel is open, don't look for dialogue clicks
        if (GameManager.loadPanel) { return; }

        if (Input.GetMouseButtonDown(0))
        {
            // raycast bs
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Ray2D ray = new Ray2D(mousePosition, Vector2.zero);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null)
            {
                if (hit.collider.gameObject == gameObject)
                {
                    StartDialogue();
                }
            }
        }
    }

    // TODO look into, OnMouseDown can't raycast to this collider when the overlay camera is active
    //void OnMouseDown()
    //{
    //    Debug.Log("pressed");
    //    DialogueManager.PlayDialogue(asset, lineIndex, this);
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
}
