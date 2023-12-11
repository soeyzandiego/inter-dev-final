using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroScene : MonoBehaviour, ICutscenePlayer
{
    public DialogueAsset asset;
    DialogueManager.OnLastLine onLastLine;

    float countdown = 1.5f;
    bool played = false;

    void Update()
    {
        if (played) { return; }

        if (countdown > 0)
        {
            countdown -= Time.deltaTime;
        }
        else
        {
            onLastLine = EndAction;
            DialogueManager.PlayDialogue(asset, 0, onLastLine);
            played = true;
        }
    }

    public void EndAction()
    {
        FindObjectOfType<SceneLoader>().QueueScene(2);
    }
}
