using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueClick : MonoBehaviour
{
    [Header("Dialogue")]
    public DialogueAsset asset;
    public DialogueAsset yourJobAsset;
    public DialogueAsset lesterBeaumontAsset;
    public DialogueUnlockable[] unlockables;
    public DialogueUnlockable challenge;

    [Header("Audio")]
    public AudioClip clickSound;

    [System.Serializable]
    public class DialogueUnlockable
    {
        public string investigatePanelText;
        public DialogueAsset dialogue;
        public string[] unlockIds;
        [HideInInspector] public bool unlocked = false;
    }

    int lineIndex = 0;

    [Header("UI Elements")]
    public GameObject sidebarButton;
    public GameObject profileButton;

    bool finished = false;

    private void Awake()
    {
        if (profileButton != null) { profileButton.SetActive(false); }
    }

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
                    if (clickSound != null) { SoundManager.PlaySound(clickSound); }
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
        if (sidebarButton != null && profileButton != null)
        {
            if (sidebarButton.activeSelf)
            {
                sidebarButton.SetActive(false);
            }
            profileButton.SetActive(true);
        }
        DialogueManager.PlayDialogue(asset, lineIndex, this);
    }

    public void FinishDialogue()
    {
        finished = true; 
    }

    public void SetAsset(DialogueAsset newAsset)
    {
        asset = newAsset;
    }

    public bool IsFinished()
    {
        return finished;
    }
}
