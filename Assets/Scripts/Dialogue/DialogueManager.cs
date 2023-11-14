using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject dialoguePanel;
    public Image characterSprite;

    [Header("Talk Panel")]
    public GameObject talkPanel;
    public TMP_Text bodyText;
    public TMP_Text nameText;

    [Header("Choice Panel")]
    public GameObject choicePanel;
    public GameObject[] choiceElements;

    public enum DialogueStates
    {
        NONE,
        TALKING,
        CHOOSING,
        INVESTIGATING
    };

    public static DialogueStates state = DialogueStates.NONE;

    static DialogueAsset curAsset;
    static int curLineIndex;
    static DialogueClick talkingTo; // current character we're in conversation with

    // Start is called before the first frame update
    void Start()
    {
        CloseDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        // turns on the object containing all the panels
        if (state != DialogueStates.NONE) { dialoguePanel.SetActive(true); }

        switch (state)
        {
            case DialogueStates.TALKING:
                choicePanel.SetActive(false);
                talkPanel.SetActive(true);

                bodyText.text = curAsset.lines[curLineIndex].dialogue;
                nameText.text = curAsset.lines[curLineIndex].character.charName;

                // set sprite based on current line's character
                characterSprite.sprite = curAsset.lines[curLineIndex].character.sprites[0];

                if (Input.GetMouseButtonDown(0)) { NextLine(); }
            break;

            case DialogueStates.CHOOSING:
                talkPanel.SetActive(false);
                choicePanel.SetActive(true);

                DialogueAsset.DialogueChoice[] choices = curAsset.lines[curLineIndex].choices;

                // TODO lambda expression doesn't work with local variable from for loop, so... figure that out
                Button but1 = choiceElements[0].GetComponentInChildren<Button>();
                but1.onClick.AddListener(() => ChooseOption(0));
                Button but2 = choiceElements[1].GetComponentInChildren<Button>();
                but2.onClick.AddListener(() => ChooseOption(1));
                Button but3 = choiceElements[2].GetComponentInChildren<Button>();
                but3.onClick.AddListener(() => ChooseOption(2));

                for (int i = 0; i < choiceElements.Length; i++)
                {
                    TMP_Text choiceText = choiceElements[i].GetComponentInChildren<TMP_Text>();
                    Button choiceButton = choiceElements[i].GetComponentInChildren<Button>();

                    choiceText.text = choices[i].text;
                    //choiceButton.onClick.AddListener(() => { ChooseOption(i); });

                    // TODO set character sprite to Grimoire

                }
            break;

            case DialogueStates.INVESTIGATING:
                talkPanel.SetActive(false);
                choicePanel.SetActive(false);

                // deactivate sprite
                characterSprite.sprite = null;

            break;
        }
    }

    public static void PlayDialogue(DialogueAsset asset, int index, DialogueClick clicked)
    {
        curAsset = asset;
        curLineIndex = index;
        state = DialogueStates.TALKING;

        talkingTo = clicked;
    }

    void NextLine()
    {
        if (curLineIndex < curAsset.lines.Length - 1) 
        { 
            // if the current line has choices
            if (curAsset.lines[curLineIndex].choices.Length > 0)
            {
                state = DialogueStates.CHOOSING;
            }
            else
            {
                curLineIndex++;
            }
        }
    }

    public void ChooseOption(int choice)
    {
        DialogueAsset.DialogueChoice[] choices = curAsset.lines[curLineIndex].choices;
        curAsset = choices[choice].nextSection;

        curLineIndex = 0;
        state = DialogueStates.TALKING;
    }

    public void CloseDialogue()
    {
        dialoguePanel.SetActive(false);
        talkingTo = null;
    }
}
