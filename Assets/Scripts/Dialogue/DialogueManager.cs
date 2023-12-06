using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public Sprite choiceCharSprite; // the sprite to display when the choice menu is open
    [Space(10)]

    [Header("Audio")]
    public AudioClip skipSound;
    public AudioClip buttonSound; 

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
    public GameObject[] unlockableElements;

    [Header("Text Writing")]
    public float delayTime = 0.1f;
    public float endTime = 1.2f;
    public int maxCharacters = 25; // max characters per line
    string textToPlay = "";
    List<int> spacePositions = new List<int>(); // will store the positions of each space to find the starts/ends of words
    bool typing = false;
    Coroutine typeCo; // current typing coroutine
    Coroutine endCo; // current end coroutine to move to next line
    Sprite queuedSprite = null;

    public enum DialogueStates
    {
        NONE,
        TALKING,
        CHOOSING,
        INVESTIGATING
    };

    bool updated = false; // so certain lines will only run once at the beginning of a state switch (similar to bool typing)

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

        GetComponent<Animator>().SetBool("DialogueOpen", state == DialogueStates.TALKING || state == DialogueStates.CHOOSING);

        switch (state)
        {
            case DialogueStates.TALKING:

                choicePanel.SetActive(false);
                investigatePanel.SetActive(false);
                talkPanel.SetActive(true);

                if (typing && Input.GetMouseButtonDown(0))
                {
                    if (skipSound != null) { SoundManager.PlaySound(skipSound); }

                    if (bodyText.text.Length < textToPlay.Length)
                    {
                        // Skip to the end of the current line
                        StopCoroutine(typeCo);

                        // remove any sprite indicators before setting text
                        for (int charIndex = 0; charIndex < textToPlay.Length; charIndex++)
                        {
                            switch (textToPlay[charIndex].ToString())
                            {
                                case "#":
                                    textToPlay = textToPlay.Remove(charIndex, 1);
                                    charIndex--;
                                break;
                                
                                case "$":
                                    textToPlay = textToPlay.Remove(charIndex, 1);
                                    charIndex--;
                                break;

                                case "%":
                                    textToPlay = textToPlay.Remove(charIndex, 1);
                                    charIndex--;
                                break;

                                case "&":
                                    textToPlay = textToPlay.Remove(charIndex, 1);
                                    charIndex--;
                                break;
                            }
                        }

                        bodyText.text = textToPlay;

                        // start counting to move to next line
                        endCo = StartCoroutine(EndLine());
                    }
                    else
                    {
                        // Play next line
                        StopCoroutine(typeCo);
                        StopCoroutine(endCo);
                        NextLine();
                    }
                }

                if (!typing)
                {
                    typing = true;
                    if (curAsset == null) { return; }
                    textToPlay = curAsset.lines[curLineIndex].dialogue;
                    SeparateWords();
                    typeCo = StartCoroutine(WriteText());

                    // set sprite based on current line's character
                    characterSprite.enabled = true;
                    // if this is the sprite's first appearance
                    if (queuedSprite == null)
                    {
                        characterSprite.sprite = curAsset.lines[curLineIndex].character.sprites[0];
                        GetComponent<Animator>().SetTrigger("SpriteAppear");
                    }
                    else
                    {
                        //queuedSprite = curAsset.lines[curLineIndex].character.sprites[0];
                    }
                    

                    // If there is an unlock 
                    if (curAsset.lines[curLineIndex].unlockID != "")
                    {
                        bool unlocked = GameManager.UnlockClue(curAsset.lines[curLineIndex].unlockID);

                        // if this is new information
                        if (!unlocked)
                        {
                            // update case file indicator
                            GetComponent<Animator>().SetTrigger("SuspectUpdated");
                        }
                    }
                }

                nameText.text = curAsset.lines[curLineIndex].character.charName;
            break;

            case DialogueStates.CHOOSING:
                talkPanel.SetActive(false);
                investigatePanel.SetActive(false);
                choicePanel.SetActive(true);

                if (!updated)
                {
                    updated = true;
                    DialogueAsset.DialogueChoice[] choices = curAsset.lines[curLineIndex].choices;

                    queuedSprite = choiceCharSprite;
                    GetComponent<Animator>().SetTrigger("SpriteChange");

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
                    }
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
        bodyText.text = "";
        StopCoroutine(typeCo);
        StopCoroutine(endCo);

        // if the current line has choices
        if (curAsset.lines[curLineIndex].choices.Length > 0)
        {
            state = DialogueStates.CHOOSING;
        }
        else
        {
            typing = false;
            if (curLineIndex < curAsset.lines.Length - 1)
            {
                curLineIndex++;
            }
            else
            {
                queuedSprite = null;
                curAsset = null;
                textToPlay = null;

                // we've reached the end of the conversation, show investigate panel
                state = DialogueStates.INVESTIGATING;

                //GetComponent<Animator>().SetTrigger("ForceClose");
                CheckUnlockables();
            }
        }
    }

    void CheckUnlockables()
    {
        // reference talkingTo's unlockable IDs to GameManager IDs
        for (int i = 0; i < talkingTo.unlockables.Length; i++)
        {
            DialogueClick.DialogueUnlockable unlockable = talkingTo.unlockables[i];
            foreach (string ID in unlockable.unlockIds)
            {
                if (GameManager.suspectClues.Contains(ID))
                {
                    continue;
                }
                else
                {
                    return;
                }
            }
            // if we've found every ID, unlock
            unlockableElements[i].SetActive(true);
            unlockableElements[i].GetComponentInChildren<TMP_Text>().text = unlockable.investigatePanelText;
        }
    }

    public void ChooseOption(int choice)
    {
        DialogueAsset.DialogueChoice[] choices = curAsset.lines[curLineIndex].choices;
        if (choices.Length > 0)
        {
            if (choices[choice].nextSection != null)
            {
                curAsset = choices[choice].nextSection;
                curLineIndex = 0;
                state = DialogueStates.TALKING;
            }
            else // if not branching
            {
                textToPlay = null;

                typing = false;
                bodyText.text = "";
                curLineIndex++;
                state = DialogueStates.TALKING;
            }
        }
        updated = false;
    }

    public void CloseDialogue()
    {
        dialoguePanel.SetActive(false);
        typing = false;
        talkingTo = null;
        queuedSprite = null;
        curAsset = null;
        textToPlay = null;
        if (typeCo != null) { StopCoroutine(typeCo); }
        if (endCo != null) { StopCoroutine(endCo); }
        state = DialogueStates.NONE;
    }

    public void YourJob()
    {
        typing = false;
        textToPlay = null;
        curAsset = talkingTo.yourJobAsset;

        curLineIndex = 0;
        state = DialogueStates.TALKING;
    }

    public void LesterBeaumont()
    {
        typing = false;
        textToPlay = null;
        curAsset = talkingTo.lesterBeaumontAsset;

        curLineIndex = 0;
        state = DialogueStates.TALKING;
    }

    public void PlayUnlockable(int index)
    {
        typing = false;
        textToPlay = null; 
        curAsset = talkingTo.unlockables[index].dialogue;

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
        //int wordIndex = -1;
        for (int charIndex = 0; charIndex < textToPlay.Length; charIndex++)
        {
            switch (textToPlay[charIndex].ToString())
            {
                case "#":
                    GetComponent<Animator>().SetTrigger("SpriteChange");
                    queuedSprite = curAsset.lines[curLineIndex].character.sprites[0];
                    textToPlay = textToPlay.Remove(charIndex, 1);
                    charIndex--;
                break;

                case "$":
                    GetComponent<Animator>().SetTrigger("SpriteChange");
                    queuedSprite = curAsset.lines[curLineIndex].character.sprites[1];
                    textToPlay = textToPlay.Remove(charIndex, 1);
                    charIndex--;
                break;

                case "%":
                    GetComponent<Animator>().SetTrigger("SpriteChange");
                    queuedSprite = curAsset.lines[curLineIndex].character.sprites[2];
                    textToPlay = textToPlay.Remove(charIndex, 1);
                    charIndex--;
                break;

                case "&":
                    GetComponent<Animator>().SetTrigger("SpriteChange");
                    queuedSprite = curAsset.lines[curLineIndex].character.sprites[3];
                    textToPlay = textToPlay.Remove(charIndex, 1);
                    charIndex--;
                break;

                default:
                    // if there's no sprite indicator, just add to the text
                    bodyText.text += textToPlay[charIndex];
                break;
            }
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
            if (charIndex == textToPlay.Length - 1) { endCo = StartCoroutine(EndLine()); }  // end line after last letter
            yield return new WaitForSecondsRealtime(delayTime);
        }
    }

    IEnumerator EndLine()
    {
        yield return new WaitForSecondsRealtime(endTime);
        NextLine();
    }


    // used by an animation event for SpriteChange
    public void ChangeSprite()
    {
        characterSprite.sprite = queuedSprite;
    }

    public void ButtonSound()
    {
        if (buttonSound != null) { SoundManager.PlaySound(buttonSound); }
    }
}
