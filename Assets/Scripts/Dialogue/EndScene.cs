using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScene : MonoBehaviour, ICutscenePlayer
{
    public DialogueAsset asset;
    public GameObject endRoom;
    public AudioClip clickSound;
    DialogueManager.OnLastLine onLastLine;

    // Update is called once per frame
    void Update()
    {
        // if there's a conversation going on, don't look for dialogue clicks
        if (DialogueManager.state != DialogueManager.DialogueStates.NONE) { return; }
        // if a panel is open, don't look for dialogue clicks
        if (GameManager.loadPanel) { return; }
        // if walk mode is active, don't look for dialogue clicks
        if (GameManager.walkMode) { return; }

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

    void StartDialogue()
    {
        Debug.Log(asset.name);
        onLastLine = EndAction;
        DialogueManager.PlayDialogue(asset, 0, onLastLine);
        FindObjectOfType<DialogueManager>().ToggleEndChallengeMode(true); // TODO um different way to do this plz
    }

    public void EndAction()
    {
        FindObjectOfType<GameManager>().moveToRoom(endRoom);
    }
}
