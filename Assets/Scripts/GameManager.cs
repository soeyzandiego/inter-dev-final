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
    [SerializeField] TMP_Text challengeIndicator;
    [SerializeField] SuspectClueUI[] clueElements;

    [Header("Audio")]
    [SerializeField] AudioClip unloadPanelSound;
    [SerializeField] AudioClip walkModeSound;
    [SerializeField] AudioClip mapPanelSound;

    [Header("Suspect Profiles")]
    [SerializeField] SuspectFile[] suspects;

    //Initialize variables
    public static bool loadPanel = false; // public and static so ObjectClick and DialogueClick can check
    public static bool walkMode = false;
    public static List<Button> walkButtons = new List<Button>(); //List of game objects to be iterated through when walkmode is turned on
    public static List<string> suspectClues = new List<string>(); // holds all the unlocked suspect clues (their ID, not the actual text)

    [Header("List of Buttons")]
    [HideInInspector] public Button[] buttons;
    [SerializeField] Button[] mapButtons;
    public List<GameObject> clicker;
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
        transform.position = currentRoom.transform.position;

        foreach (Button button in mapButtons) { button.gameObject.SetActive(false); }

        buttons = FindObjectsOfType<Button>();
        GameObject[] temp = GameObject.FindGameObjectsWithTag("WalkButton");
        foreach(GameObject g in temp)
        {
            walkButtons.Add(g.GetComponent<Button>());
        }
        mapButton.SetActive(false);
    }

    //Main Method
    private void Update()
    {
        //Loading Panels
        if (loadPanel)
        {
            Panel.transform.position = Vector3.Lerp(Panel.transform.position, transform.position + new Vector3(0.1f, 0, 0), Time.deltaTime * 5);
            SetWalkModeButtonActive(false);
        }

        else
        {
            Panel.transform.position = Vector3.Lerp(Panel.transform.position, transform.position + new Vector3(-18, 0, 0), Time.deltaTime * 5);
        }

        if (Input.GetMouseButtonDown(0) && currentRoom.name == "Thank You") { moveToRoom(MainMenu); }
    }


    //Coroutine for screen transition
    //Loops for 2 seconds, lowering opacity, swaps currentRoom object, and makes the new object visible.
    IEnumerator RoomTransition(GameObject room)
    {
        WalkModeToggle();
        foreach (Button button in buttons)
        {
            button.interactable = false;
        }
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
        foreach (Button button in buttons)
        {
            button.interactable = true;
        }
    }

    //Method to move from room to room
    public void moveToRoom(GameObject room)
    {
        if (room.layer == LayerMask.NameToLayer("NoDust")) { dustParticles.StopDust(); }
        else { dustParticles.StartDust(); }

        StartCoroutine(RoomTransition(room)); UnloadPanel(); 
    }

    //Method to toggle walking mode. Runs through for loop to set buttons to active, and deactivates them when walk mode is toggled again.
    public void WalkModeToggle()
    {
        if (walkModeSound != null) { SoundManager.PlaySound(walkModeSound); }

        walkMode = !walkMode;
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
        gateButton.onClick.AddListener(() => { moveToRoom(gateButton.GetComponent<GateButtonSwap>().room); });
    }

    //Method to Load the UI Panels
    public void UnloadPanel()
    {
        loadPanel = false;
        profilePanel.SetActive(false);
        cluesPanel.SetActive(false);
        mapPanel.SetActive(false);
        suspectPanel.SetActive(false);
        SetWalkModeButtonActive(true);

        if (unloadPanelSound != null) { SoundManager.PlaySound(unloadPanelSound); }
    }

    public void LoadProfilePanel()
    {
        loadPanel = true;
        profilePanel.SetActive(true);
        cluesPanel.SetActive(false);
        mapPanel.SetActive(false);
        suspectPanel.SetActive(false);
    }

    public void LoadCluesPanel()
    {
        loadPanel = true;
        cluesPanel.SetActive(true);
        mapPanel.SetActive(false);
        profilePanel.SetActive(false);
        suspectPanel.SetActive(false);
    }

    public void LoadMapPanel()
    {
        if (mapPanelSound != null) { SoundManager.PlaySound(mapPanelSound); }
        loadPanel = true;
        mapPanel.SetActive(true);
        cluesPanel.SetActive(false);
        profilePanel.SetActive(false);
        suspectPanel.SetActive(false);
    }

    public void LoadSuspectPanel(int suspectIndex)
    {
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
            challengeIndicator.text = "SOLVED!";
        }
        else if (suspectClues.Contains(curSuspect.challengeUnlockID))
        {
            challengeText.enabled = true;
            challengeText.text = curSuspect.challengeUnlocked;
            challengeIndicator.text = "Unlocked";
        }
        else
        {
            challengeText.enabled = false;
            challengeIndicator.text = "???";
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
}
