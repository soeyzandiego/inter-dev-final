using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScene : Clickable, ICutscenePlayer
{
    [SerializeField] DialogueAsset asset;
    [SerializeField] GameObject endRoom;
    [SerializeField] AudioClip clickSound;
    [SerializeField] GameObject bert;
    DialogueManager.OnLastLine onLastLine;

    void Start()
    {
        GetComponent<Collider2D>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
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
        //Debug.Log("can't click end scene");
        GameManager.DisableClickables();
        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject);
        Destroy(bert);
        FindObjectOfType<GameManager>().moveToRoom(endRoom);
    }
}
