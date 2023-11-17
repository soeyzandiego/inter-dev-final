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

    [Header("Investigate Panel")]
    public GameObject investigatePanel;

    [Header("Text Writing")]
    public float delayTime = 0.1f;
    public float endTime = 1.2f;
    public int maxCharacters = 25; // max characters per line
    string textToPlay = "";
    List<int> spacePositions = new List<int>(); // will store the positions of each space to find the starts/ends of words
    bool typing = false;
    Coroutine typeCo; // current typing coroutine
    Coroutine endCo; // current end coroutine to move to next line

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
                investigatePanel.SetActive(false);
                talkPanel.SetActive(true);

                if (!typing)
                {
                    typing = true;
                    textToPlay = curAsset.lines[curLineIndex].dialogue;
                    SeparateWords();
                    typeCo = StartCoroutine(WriteText());
                }

                nameText.text = curAsset.lines[curLineIndex].character.charName;

                // set sprite based on current line's character
                characterSprite.enabled = true;
                characterSprite.sprite = curAsset.lines[curLineIndex].character.sprites[0];

                if (Input.GetMouseButtonDown(0)) 
                {
                    if (bodyText.text != textToPlay)
                    {
                        Debug.Log("Skipped");
                        StopCoroutine(typeCo);
                        endCo = StartCoroutine(EndLine());
                        bodyText.text = textToPlay;
                    }
                    else
                    {
                        StopCoroutine(endCo);
                        NextLine();
                    }
                    
                }
            break;

            case DialogueStates.CHOOSING:
                talkPanel.SetActive(false);
                investigatePanel.SetActive(false);
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
                    //choiceButton.onClick.AddListener(() => ChooseOption(i));

                    // TODO set character sprite to Grimoire

                }
            break;

            case DialogueStates.INVESTIGATING:
                talkPanel.SetActive(false);
                choicePanel.SetActive(false);
                investigatePanel.SetActive(true);

                // deactivate sprite
                characterSprite.enabled = false;

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
        typing = false;
        bodyText.text = "";

        // if the current line has choices
        if (curAsset.lines[curLineIndex].choices.Length > 0)
        {
            state = DialogueStates.CHOOSING;
        }
        else
        {
            if (curLineIndex < curAsset.lines.Length - 1)
            {
                curLineIndex++;
            }
            else
            {
                // we've reached the end of the conversation, show investigate panel
                state = DialogueStates.INVESTIGATING;
                CheckUnlockables();
            }
        }
    }

    void CheckUnlockables()
    {
        // reference talkingTo's unlockable IDs to GameManager IDs

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

    public void YourJob()
    {
        curAsset = talkingTo.yourJobAsset;

        curLineIndex = 0;
        state = DialogueStates.TALKING;
    }

    public void LesterBeaumont()
    {
        curAsset = talkingTo.lesterBeaumontAsset;

        curLineIndex = 0;
        state = DialogueStates.TALKING;
    }

    void SeparateWords()
    {
        spacePositions.Clear();
        for (int charIndex = 0; charIndex < textToPlay.Length; charIndex++)
        {
            if (textToPlay[charIndex].ToString() == " ") { spacePositions.Add(charIndex); }
        }
    }

    IEnumerator WriteText()
    {
        bodyText.text = "";
        int wordIndex = -1;
        for (int charIndex = 0; charIndex < textToPlay.Length; charIndex++)
        {
            bodyText.text += textToPlay[charIndex];
            // this was all text wrapping stuff that is... not working but it's not a huge deal
            //if (textToPlay[charIndex].ToString() == " ")
            //{
            //    wordIndex++;
            //    // if it's the last word, use the total text length bc there's no space at the end to check
            //    if (wordIndex == spacePositions.Count - 1)
            //    {
            //        int wordLength = textToPlay.Length - spacePositions[wordIndex];
            //        // TODO fix... right now it just clears the line and starts writing the next part
            //        // if the word isn't going to fit, start a new line
            //        //if (bodyText.text.Length + wordLength > maxCharacters) { bodyText.text += "\n"; }
            //        //else { bodyText.text += textToPlay[charIndex]; }
            //        bodyText.text += textToPlay[charIndex];
            //    }
            //    else
            //    {
            //        int wordLength = spacePositions[wordIndex + 1] - spacePositions[wordIndex];
            //        // TODO fix... right now it just clears the line and starts writing the next part
            //        // if the word isn't going to fit, start a new line
            //        //if (bodyText.text.Length + wordLength > maxCharacters) { bodyText.text += "\n"; }
            //        //else { bodyText.text += textToPlay[charIndex]; }
            //        bodyText.text += textToPlay[charIndex];
            //    }
            //}
            //else
            //{
            //    if (bodyText.text.Length < maxCharacters) { bodyText.text += textToPlay[charIndex]; }
            //    //else { bodyText.text = textToPlay[charIndex].ToString(); }
            //}
            if (charIndex == textToPlay.Length - 1) { StartCoroutine(EndLine()); }  // close text after last letter
            yield return new WaitForSecondsRealtime(delayTime);
        }
    }

    IEnumerator EndLine()
    {
        yield return new WaitForSecondsRealtime(endTime);
        if (curAsset.lines[curLineIndex].unlockID != null)
        {
            GameManager.AddSuspectClue(curAsset.lines[curLineIndex].unlockID);
        }
        NextLine();
    }
}
