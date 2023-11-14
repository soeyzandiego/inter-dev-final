using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueClick : MonoBehaviour
{
    public DialogueAsset asset;

    int lineIndex = 0;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(StartDialogue);
    }

    // TODO look into, OnMouseDown can't raycast to this collider when the overlay camera is active
    //void OnMouseDown()
    //{
    //    Debug.Log("pressed");
    //    DialogueManager.PlayDialogue(asset, lineIndex);
    //}

    void StartDialogue()
    {
        DialogueManager.PlayDialogue(asset, lineIndex);
    }

    public void ExitDialogue(int newIndex)
    {
        lineIndex = newIndex;
    }
}
