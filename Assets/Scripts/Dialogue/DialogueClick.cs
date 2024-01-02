using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueClick : MonoBehaviour
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
        if (LogicPuzzleManager.logicOpen) { return; }
        // if there's a conversation going on, don't look for dialogue clicks
        if (DialogueManager.state != DialogueManager.DialogueStates.NONE) { return; }
        // if a panel is open, don't look for dialogue clicks
        if (GameManager.loadPanel) { return; }
        // if walk mode is active, don't look for dialogue clicks
        if (GameManager.walkMode) { return; }

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Ray2D ray = new Ray2D(mousePosition, Vector2.zero);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider != null)
        {
            if (hit.collider.gameObject == gameObject) 
            {
                // hovering
                spriteRenderer.sprite = hoverSprite;

                if (Input.GetMouseButtonDown(0))
                {
                    if (clickSound != null) { SoundManager.PlaySound(clickSound); }
                    spriteRenderer.sprite = defaultSprite;
                    StartDialogue();
                }
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
