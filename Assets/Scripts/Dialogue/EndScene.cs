using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScene : Clickable, ICutscenePlayer
{
    [SerializeField] DialogueAsset asset;
    [SerializeField] GameObject endRoom;
    [SerializeField] AudioClip clickSound;
    DialogueManager.OnLastLine onLastLine;

    // Update is called once per frame
    void Update()
    {
        if (!clickable) { return; }

        if (Input.GetMouseButtonDown(0))
        {
            if (HoveringThis())
            {
                if (clickSound != null) { SoundManager.PlaySound(clickSound); }
                StartDialogue();
            }
        }
    }

    void StartDialogue()
    {
        onLastLine = EndAction;
        FindObjectOfType<DialogueManager>().ToggleEndChallengeMode(true, RemoveIndicators()); // TODO um different way to do this plz
        DialogueManager.PlayDialogue(asset, 0, onLastLine);
    }

    string RemoveIndicators()
    {
        string lastLineText = asset.lines[asset.lines.Count - 1].dialogue;
        return lastLineText.Substring(1);
    }

    public void EndAction()
    {
        FindObjectOfType<GameManager>().moveToRoom(endRoom);
    }

    public override void SetClickable(bool _clickable)
    {
        clickable = _clickable;
    }
}
