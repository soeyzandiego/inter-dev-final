using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using JetBrains.Annotations;

public class GameManager : MonoBehaviour
{
    //Receive Game Objects
    [SerializeField] GameObject Panel;

    [SerializeField] GameObject profilePanel;
    [SerializeField] GameObject cluesPanel;
    [SerializeField] GameObject mapPanel;
    [SerializeField] GameObject suspectPanel;
    [SerializeField] DustController dustParticles;
    [SerializeField] GameObject walkModeButton;

    [Header("Room Transition")]
    [SerializeField] public GameObject currentRoom;
    [SerializeField] GameObject fadeToBlack;

    [Header("Suspect Panel Elements")]
    [SerializeField] Image suspectPicture;
    [SerializeField] TMP_Text suspectName;
    [SerializeField] TMP_Text suspectQuote;
    [SerializeField] TMP_Text challengeText;
    [SerializeField] Image challengeIndicator;
    [SerializeField] Sprite[] challengeIndicatorSprites;
    [SerializeField] SuspectClueUI[] clueElements;

    [Header("Audio")]
    [SerializeField] AudioClip unloadPanelSound;
    [SerializeField] AudioClip walkModeSound;
    [SerializeField] AudioClip mapPanelSound;

    [Header("Suspect Profiles")]
    [SerializeField] SuspectFile[] suspects;

    //Initialize variables
    bool loadPanel = false; // public and static so ObjectClick and DialogueClick can check
    bool walkMode = true;
    public static List<Button> walkButtons = new List<Button>(); // list of game objects to be iterated through when walkmode is turned on
    public static List<string> suspectClues = new List<string>(); // holds all the unlocked suspect clues (their ID, not the actual text)
    public static bool clickable = true;
    static List<Clickable> clickables = new List<Clickable>(); 

    [Header("List of Buttons")]
    [HideInInspector] public Button[] buttons;
    [SerializeField] Button[] mapButtons;
    [SerializeField] public GameObject magGlass;
    [SerializeField] GameObject mapSelect;
    [SerializeField] GameObject MainMenu;
    [SerializeField] GameObject UI;
    [SerializeField] GameObject mapButton;

    [Header("GateButton Replacement")]
    [SerializeField] GameObject gatePuzzleManager;
    [SerializeField] Button gateButton;


    //deactivate all walk buttons on game start.
    private void Start()
    {
        Clickable[] clickablesToAdd = FindObjectsOfType<Clickable>();
        foreach (Clickable c in clickablesToAdd) { clickables.Add(c); }
        EnableClickables();

        transform.position = currentRoom.transform.position;

        foreach (Button button in mapButtons) { button.gameObject.SetActive(false); }

        buttons = FindObjectsOfType<Button>(true);

        gateButton.onClick.AddListener(() => { moveToRoom(gateButton.GetComponent<GateButtonSwap>().gateRoom); });

        GameObject[] temp = GameObject.FindGameObjectsWithTag("WalkButton");
        foreach(GameObject g in temp)
        {
            walkButtons.Add(g.GetComponent<Button>());
        }
        mapButton.SetActive(false);

        WalkModeToggle(false);
    }

    //Main Method
    private void Update()
    {
        //Loading Panels
        if (loadPanel)
        {
            Panel.transform.position = Vector3.Lerp(Panel.transform.position, transform.position + new Vector3(0.1f, 0, 0), Time.deltaTime * 5);
        }

        else
        {
            Panel.transform.position = Vector3.Lerp(Panel.transform.position, transform.position + new Vector3(-18, 0, 0), Time.deltaTime * 5);
        }

        if (Input.GetMouseButtonDown(0) && currentRoom.name == "Thank You") { FindObjectOfType<SceneLoader>().QueueScene(0); }
    }

    //Coroutine for screen transition
    //Loops for 2 seconds, lowering opacity, swaps currentRoom object, and makes the new object visible.
    IEnumerator RoomTransition(GameObject room)
    {
        WalkModeToggle();
        DisableClickables();
        SetButtonsActive(false);
        Time.timeScale = 0;

        for (float i = 0; i < 1; i += Time.unscaledDeltaTime * 2)
        {
            fadeToBlack.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, i);
            yield return null;
        }

        currentRoom = room;
        transform.position = currentRoom.transform.position;

        UI.gameObject.SetActive(true);
        switch (currentRoom.name)
        {
            case "Entrance":
                {
                    mapSelect.transform.position = mapButtons[0].transform.position; mapButtons[0].gameObject.SetActive(true);
                    break;
                }
            case "Outside Rest Stop":
                {
                    mapSelect.transform.position = mapButtons[1].transform.position; mapButtons[1].gameObject.SetActive(true);
                    break;
                }
            case "Inside Rest Stop":
                {
                    mapSelect.transform.position = mapButtons[2].transform.position; mapButtons[2].gameObject.SetActive(true);
                    break;
                }
            case "Rita's":
                {
                    mapSelect.transform.position = mapButtons[3].transform.position; mapButtons[3].gameObject.SetActive(true);
                    break;
                }
            case "Outside Temple":
                {
                    mapSelect.transform.position = mapButtons[4].transform.position; mapButtons[4].gameObject.SetActive(true);
                    break;
                }
            case "Inside Temple":
                {
                    mapSelect.transform.position = mapButtons[5].transform.position; mapButtons[5].gameObject.SetActive(true);
                    break;
                }
            case "Main Menu":
                {
                    UI.gameObject.SetActive(false);
                    break;
                }
        }

        for (float i = 1; i >= 0; i -= Time.unscaledDeltaTime * 2)
        {
            fadeToBlack.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, i);
            yield return null;
        }


        Time.timeScale = 1;
        EnableClickables();
        SetButtonsActive(true);
    }

    //Method to move from room to room
    public void moveToRoom(GameObject room)
    {
        if (room.layer == LayerMask.NameToLayer("NoDust")) { dustParticles.StopDust(); }
        else { dustParticles.StartDust(); }

        UnloadPanel(); 
        StartCoroutine(RoomTransition(room)); 
    }

    //Method to toggle walking mode. Runs through for loop to set buttons to active, and deactivates them when walk mode is toggled again.
    public void WalkModeToggle(bool sound = true)
    {
        if (walkModeSound != null && sound) { SoundManager.PlaySound(walkModeSound); }

        walkMode = !walkMode;

        //Debug.Log("toggled to " + walkMode);

        if (walkMode) { DisableClickables(); }
        else { EnableClickables(); }

        foreach (Button button in buttons)
        {
            button.interactable = !walkMode;
        }
        foreach (Button button in walkButtons)
        {
            button.gameObject.SetActive(walkMode);
            button.interactable = walkMode;
        }
        walkModeButton.GetComponent<Button>().interactable = true;
    }

    // Method to swap out the gate button after puzzle has been completed.
    public void PuzzleComplete()
    {
        gateButton.onClick.RemoveAllListeners();
        gateButton.onClick.AddListener(() => { moveToRoom(gateButton.GetComponent<GateButtonSwap>().roomSwap); });
    }

    //Method to Load the UI Panels
    public void UnloadPanel()
    {
        EnableClickables();

        loadPanel = false;
        profilePanel.SetActive(false);
        cluesPanel.SetActive(false);
        mapPanel.SetActive(false);
        suspectPanel.SetActive(false);

        if (unloadPanelSound != null) { SoundManager.PlaySound(unloadPanelSound); }
    }

    public void LoadProfilePanel()
    {
        DisableClickables();

        loadPanel = true;
        profilePanel.SetActive(true);
        cluesPanel.SetActive(false);
        mapPanel.SetActive(false);
        suspectPanel.SetActive(false);
    }

    public void LoadCluesPanel()
    {
        DisableClickables();

        loadPanel = true;
        cluesPanel.SetActive(true);
        mapPanel.SetActive(false);
        profilePanel.SetActive(false);
        suspectPanel.SetActive(false);
    }

    public void LoadMapPanel()
    {
        DisableClickables();

        if (mapPanelSound != null) { SoundManager.PlaySound(mapPanelSound); }
        loadPanel = true;
        mapPanel.SetActive(true);
        cluesPanel.SetActive(false);
        profilePanel.SetActive(false);
        suspectPanel.SetActive(false);
    }

    public void LoadSuspectPanel(int suspectIndex)
    {
        DisableClickables();

        loadPanel = true;
        suspectPanel.SetActive(true);
        mapPanel.SetActive(false);
        cluesPanel.SetActive(false);
        profilePanel.SetActive(false);

        SuspectFile curSuspect = suspects[suspectIndex];
        suspectPicture.sprite = curSuspect.sprite;
        suspectName.text = curSuspect.suspectName;
        suspectQuote.text = curSuspect.quote;
        
        for (int i = 0; i < clueElements.Length; i++)
        {
            // if the GameManager has unlocked the ID for the clue, show the clue
            if (curSuspect.clues.Length > i && suspectClues.Contains(curSuspect.clues[i].unlockId))
            {
                clueElements[i].SetClue(curSuspect.clues[i].picture, curSuspect.clues[i].text);
            }
            else
            {
                clueElements[i].HideClue();
            }
        }

        // check challenge
        if (suspectClues.Contains(curSuspect.challengeSolvedID))
        {
            challengeText.enabled = true;
            challengeText.text = curSuspect.challengeSolved;
            challengeIndicator.sprite = challengeIndicatorSprites[0];
        }
        else if (suspectClues.Contains(curSuspect.challengeUnlockID))
        {
            challengeText.enabled = true;
            challengeText.text = curSuspect.challengeUnlocked;
            challengeIndicator.sprite = challengeIndicatorSprites[1];
        }
        else
        {
            challengeText.enabled = false;
            challengeIndicator.sprite = challengeIndicatorSprites[2];
        }
    }

    public static bool UnlockClue(string _id)
    {
        bool wasUnlocked = suspectClues.Contains(_id);
        if (!wasUnlocked)
        {
            suspectClues.Add(_id);
        }

        EventManager.CheckFlag(_id);

        return wasUnlocked;
    }

    public void UnlockMap()
    {
        mapButton.SetActive(true);
    }

    public void SetWalkModeButtonActive(bool active)
    {
        walkModeButton.SetActive(active);
    }

    public void SetButtonsActive(bool active)
    {
        foreach (Button button in buttons)
        {
            button.interactable = active;
        }
    }

    static public void EnableClickables() { foreach (Clickable clickable in clickables) { clickable.SetClickable(true); } clickable = true; Debug.Log("enabled clickables"); }
    static public void DisableClickables() { foreach (Clickable clickable in clickables) { clickable.SetClickable(false); } clickable = false; Debug.Log("disabled clickables"); }

    static public void ResetGame()
    {
        clickables.Clear();
        suspectClues.Clear();
        walkButtons.Clear();
    }
}
