using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueClick : Clickable
{
    [SerializeField] Sprite hoverSprite;
    Sprite defaultSprite;

    [Header("Dialogue")]
    [SerializeField] public DialogueAsset asset;
    [SerializeField] public DialogueAsset yourJobAsset;
    [SerializeField] public DialogueAsset lesterBeaumontAsset;
    [SerializeField] public DialogueUnlockable[] unlockables;
    [SerializeField] public DialogueUnlockable challenge;

    [Header("Audio")]
    [SerializeField] AudioClip clickSound;

    [System.Serializable]
    public class DialogueUnlockable
    {
        public string investigatePanelText;
        public DialogueAsset dialogue;
        public string[] unlockIds;
    }

    [Header("UI Elements")]
    [SerializeField] GameObject sidebarButton;
    [SerializeField] GameObject profileButton;

    int lineIndex = 0;
    bool finished = false;

    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        if (profileButton != null) { profileButton.SetActive(false); }
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        defaultSprite = spriteRenderer.sprite;
    }

    private void Update()
    {
        if (HoveringThis())
        {
            // outline sprite
            spriteRenderer.sprite = hoverSprite;

            if (Input.GetMouseButtonDown(0))
            {
                if (clickSound != null) { SoundManager.PlaySound(clickSound); }
                spriteRenderer.sprite = defaultSprite;
                StartDialogue();
            }
        }
        else
        {
            spriteRenderer.sprite = defaultSprite;
        }
    }

    // TODO look into, OnMouse functions can't raycast to this collider when the overlay camera is active
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
