using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public Sprite choiceCharSprite; // the sprite to display when the choice menu is open
    public DialogueAsset tempAsset; // used to store assets with new choice lines
    public DialogueAsset.DialogueLine tempLine; // used to add a line after making a choice
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
    public string[] spriteIndicators; // possible sprite indicators
    
    string textToPlay = "";
    bool lineDone = false;
    bool typing = false; // used for making sure code only executes once during TALKING state
    string invisTag = "<color=#00000000>"; // for line wrapping, moves an invisible character along text
    string clueColorTag = "<color=#F7C6A4>"; // for highlighting clue text
    Coroutine typeCo; // current typing coroutine
    Coroutine endCo; // current end coroutine to move to next line
    Sprite queuedSprite = null; // sprite to transition to on anim

    bool challengeMode = false;
    int challengeAnswer; // to track which choice option is correct during a challenge dialogue

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

        switch (state)
        {
            case DialogueStates.TALKING:
                if (!updated)
                {
                    updated = true;
                    choicePanel.SetActive(false);
                    investigatePanel.SetActive(false);
                    talkPanel.SetActive(true);
                }
                
                // line skipping
                if (typing && Input.GetMouseButtonDown(0))
                {
                    if (skipSound != null) { SoundManager.PlaySound(skipSound); }

                    GetComponent<Animator>().SetTrigger("Skip");

                    if (!lineDone)
                    {
                        // skip to the end of the current line
                        StopCoroutine(typeCo);

                        // remove any sprite indicators before setting text
                        for (int charIndex = 0; charIndex < textToPlay.Length; charIndex++)
                        {
                            foreach (string indicator in spriteIndicators)
                            {
                                if (textToPlay[charIndex].ToString() == indicator)
                                {
                                    textToPlay = textToPlay.Remove(charIndex, 1);
                                    charIndex--;
                                }
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

                    
                    // if this is the sprite's first appearance
                    // TODO I don't like using the animator bool as a reference
                    if (!GetComponent<Animator>().GetBool("CharVis"))
                    {
                        characterSprite.sprite = GetSprite(textToPlay[0].ToString());
                        GetComponent<Animator>().SetTrigger("SpriteAppear");
                    }

                    // start typing
                    typeCo = StartCoroutine(WriteText());

                  
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

                if (curAsset.lines[curLineIndex].character != null)
                {
                    nameText.text = curAsset.lines[curLineIndex].character.charName;
                    characterSprite.enabled = true;
                    GetComponent<Animator>().SetBool("CharVis", true);
                }
                else
                {
                    nameText.text = "";
                    GetComponent<Animator>().SetBool("CharVis", false);
                }
                
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
                        int index = i;
                        TMP_Text choiceText = choiceElements[i].GetComponentInChildren<TMP_Text>();
                        Button choiceButton = choiceElements[i].GetComponentInChildren<Button>();

                        if (choices[i].text[0].ToString() == "!")
                        {
                            challengeAnswer = i;
                            choiceText.text = choices[i].text.Substring(1); // get rid of the answer indicator
                        }
                        else
                        {
                            choiceText.text = choices[i].text;
                        }
                        
                        //choiceButton.onClick.AddListener(() => ChooseOption(i));
                    }
                }
            break;

            case DialogueStates.INVESTIGATING:
                if (!updated)
                {
                    updated = true;

                    talkPanel.SetActive(false);
                    choicePanel.SetActive(false);
                    investigatePanel.SetActive(true);

                    // turn off challenge mode
                    challengeMode = false;

                    // deactivate sprite
                    GetComponent<Animator>().SetBool("CharVis", false);
                }
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
        lineDone = false;
        bodyText.text = "";
        StopCoroutine(typeCo);
        StopCoroutine(endCo);

        // if the current line has choices
        if (curAsset.lines[curLineIndex].choices.Length > 0)
        {
            SwitchState(DialogueStates.CHOOSING);
        }
        else
        {
            typing = false;
            if (curLineIndex < curAsset.lines.Count - 1)
            {
                curLineIndex++;
            }
            else
            {
                queuedSprite = null;
                curAsset = null;
                textToPlay = null;

                // we've reached the end of the conversation, show investigate panel
                SwitchState(DialogueStates.INVESTIGATING);

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
                if (GameManager.suspectClues.Contains(ID)) { continue; }
                else { return; }
            }
            // if we've found every ID, unlock
            unlockableElements[i].SetActive(true);
            unlockableElements[i].GetComponentInChildren<TMP_Text>().text = unlockable.investigatePanelText;
        }

        // do the same for challenge
        foreach (string ID in talkingTo.challenge.unlockIds)
        {
            if (GameManager.suspectClues.Contains(ID)) { continue; }
            else { return; }
        }
        unlockableElements[talkingTo.unlockables.Length + 1].SetActive(true);
    }

    public void ChooseOption(int choice)
    {
        DialogueAsset.DialogueChoice[] choices = curAsset.lines[curLineIndex].choices;
        if (choices.Length > 0)
        {
            textToPlay = null;

            typing = false;
            bodyText.text = "";

            if (challengeMode && choice != challengeAnswer)
            {
                // add the "not right line" as the only line so it goes straight back to INVESTIGATING
                tempAsset.lines.Clear();
                tempLine.dialogue = choices[choice].fullText;
                tempAsset.lines.Insert(0, tempLine);

                curAsset = tempAsset;
                curLineIndex = 0;
                state = DialogueStates.TALKING;
            }
            else
            {
                tempAsset.lines.Clear();
                foreach (DialogueAsset.DialogueLine line in curAsset.lines)
                {
                    tempAsset.lines.Add(line);
                }
                tempLine.dialogue = choices[choice].fullText;
                tempAsset.lines.Insert(curLineIndex + 1, tempLine);
                curAsset = tempAsset;

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
        GetComponent<Animator>().SetBool("CharVis", false);
        characterSprite.enabled = false;

        if (typeCo != null) { StopCoroutine(typeCo); }
        if (endCo != null) { StopCoroutine(endCo); }
        SwitchState(DialogueStates.NONE);
    }

    public void YourJob()
    {
        typing = false;
        textToPlay = null;
        curAsset = talkingTo.yourJobAsset;

        curLineIndex = 0;
        SwitchState(DialogueStates.TALKING);
    }

    public void LesterBeaumont()
    {
        typing = false;
        textToPlay = null;
        curAsset = talkingTo.lesterBeaumontAsset;

        curLineIndex = 0;
        SwitchState(DialogueStates.TALKING);
    }

    public void PlayUnlockable(int index)
    {
        typing = false;
        textToPlay = null; 
        curAsset = talkingTo.unlockables[index].dialogue;

        curLineIndex = 0;
        SwitchState(DialogueStates.TALKING);
    }

    public void PlayChallenege()
    {
        challengeMode = true;

        typing = false;
        textToPlay = null;
        curAsset = talkingTo.challenge.dialogue;

        curLineIndex = 0;
        SwitchState(DialogueStates.TALKING);
    }

    void SwitchState(DialogueStates newState)
    {
        updated = false;
        state = newState;
    }

    IEnumerator WriteText()
    {
        bodyText.text = "";
        //int wordIndex = -1;
        for (int charIndex = 0; charIndex < textToPlay.Length; charIndex++)
        {
            string character = textToPlay[charIndex].ToString();
            switch (character)
            {
                case "#":
                    queuedSprite = GetSprite(character);
                    if (characterSprite.sprite != queuedSprite) { GetComponent<Animator>().SetTrigger("SpriteChange"); }
                    textToPlay = textToPlay.Remove(charIndex, 1);
                    charIndex--;
                break;

                case "$":
                    queuedSprite = GetSprite(character);
                    if (characterSprite.sprite != queuedSprite) { GetComponent<Animator>().SetTrigger("SpriteChange"); }
                    textToPlay = textToPlay.Remove(charIndex, 1);
                    charIndex--;
                break;

                case "%":
                    queuedSprite = GetSprite(character);
                    if (characterSprite.sprite != queuedSprite) { GetComponent<Animator>().SetTrigger("SpriteChange"); }
                    textToPlay = textToPlay.Remove(charIndex, 1);
                    charIndex--;
                break;

                case "&":
                    queuedSprite = GetSprite(character);
                    if (characterSprite.sprite != queuedSprite) { GetComponent<Animator>().SetTrigger("SpriteChange"); }
                    textToPlay = textToPlay.Remove(charIndex, 1);
                    charIndex--;
                break;

                case "*":
                    queuedSprite = GetSprite(character);
                    if (characterSprite.sprite != queuedSprite) { GetComponent<Animator>().SetTrigger("SpriteChange"); }
                    textToPlay = textToPlay.Remove(charIndex, 1);
                    charIndex--;
                break;

                case "(":
                    queuedSprite = GetSprite(character);
                    if (characterSprite.sprite != queuedSprite) { GetComponent<Animator>().SetTrigger("SpriteChange"); }
                    textToPlay = textToPlay.Remove(charIndex, 1);
                    charIndex--;
                break;

                case ")":
                    queuedSprite = GetSprite(character);
                    if (characterSprite.sprite != queuedSprite) { GetComponent<Animator>().SetTrigger("SpriteChange"); }
                    textToPlay = textToPlay.Remove(charIndex, 1);
                    charIndex--;
                break;

                case "+":
                    queuedSprite = GetSprite(character);
                    if (characterSprite.sprite != queuedSprite) { GetComponent<Animator>().SetTrigger("SpriteChange"); }
                    textToPlay = textToPlay.Remove(charIndex, 1);
                    charIndex--;
                break;

                case "/":
                    // messes with the invis tag, fix later
                    //bodyText.text = textToPlay.Substring(0, charIndex) + clueColorTag + textToPlay.Substring(charIndex);
                break;

                default:
                    // if no sprite indicator, just add text
                    //bodyText.text += textToPlay[charIndex].ToString();
                    bodyText.text = textToPlay.Substring(0, charIndex) + invisTag + textToPlay.Substring(charIndex);
                break;
            }
            
            if (charIndex == textToPlay.Length - 1) 
            {
                bodyText.text = textToPlay;
                endCo = StartCoroutine(EndLine()); // end line after last letter
            }  
            yield return new WaitForSeconds(delayTime);
        }
    }

    IEnumerator EndLine()
    {
        lineDone = true;
        yield return new WaitForSeconds(endTime);
        NextLine();
    }

    Sprite GetSprite(string indicator)
    {
        if (indicator == "#") { return curAsset.lines[curLineIndex].character.sprites[0]; }
        else if (indicator == "$") { return curAsset.lines[curLineIndex].character.sprites[1]; }
        else if (indicator == "%") { return curAsset.lines[curLineIndex].character.sprites[2]; }
        else if (indicator == "&") { return curAsset.lines[curLineIndex].character.sprites[3]; }
        else if (indicator == "*") { return curAsset.lines[curLineIndex].character.sprites[4]; }
        else if (indicator == "(") { return curAsset.lines[curLineIndex].character.sprites[5]; }
        else if (indicator == ")") { return curAsset.lines[curLineIndex].character.sprites[6]; }
        else if (indicator == "-") { return curAsset.lines[curLineIndex].character.sprites[7]; }
        else if (indicator == "+") { return curAsset.lines[curLineIndex].character.sprites[8]; }
        else { return curAsset.lines[curLineIndex].character.sprites[0]; }
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
