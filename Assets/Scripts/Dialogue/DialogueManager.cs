using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public GameObject panel;
    public TMP_Text bodyText;
    public TMP_Text nameText;
    public Image characterSprite;

    public DialogueAsset testAsset;

    static bool dialogueActive = false;
    static DialogueAsset curAsset;
    static int curLineIndex;

    // Start is called before the first frame update
    void Start()
    {
        CloseDialogue();

        //PlayDialogue(testAsset, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueActive)
        {
            panel.SetActive(true);

            bodyText.text = curAsset.lines[curLineIndex].dialogue;
            nameText.text = curAsset.lines[curLineIndex].character.charName;

            characterSprite.sprite = curAsset.lines[curLineIndex].character.sprites[0];
        }
    }

    public static void PlayDialogue(DialogueAsset asset, int index)
    {
        curAsset = asset;
        curLineIndex = index;
        dialogueActive = true;
    }

    public void NextLine()
    {
        if (curLineIndex < curAsset.lines.Length - 1) 
        { 
            // if the next chunk of dialogue hasn't been unlocked yet
            if (curAsset.lines[curLineIndex + 1].locked)
            {
                // activate questioning/challenge panel thingy
            }
            else
            {
                curLineIndex++;
            }
        }
    }

    public void CloseDialogue()
    {
        panel.SetActive(false);
    }
}
