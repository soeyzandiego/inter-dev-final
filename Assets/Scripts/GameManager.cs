using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class GameManager : MonoBehaviour
{
    //Receive Game Objects
    public GameObject Panel;

    public GameObject profilePanel;
    public GameObject cluesPanel;
    public GameObject mapPanel;
    public GameObject suspectPanel;

    public GameObject currentRoom;

    [Header("Suspect Panel Elements")]
    public Image suspectPicture;
    public TMP_Text suspectName;
    public TMP_Text suspectQuote;
    public SuspectClueUI[] clueElements;


    [Header("Suspect Profiles")]
    public SuspectFile[] suspects;

    //Initialize variables
    public static bool loadPanel = false; // public and static so ObjectClick and DialogueClick can check
    bool walkMode = true;
    List<Button> walkButtons = new List<Button>(); //List of game objects to be iterated through when walkmode is turned on
    public static List<string> suspectClues = new List<string>(); // holds all the unlocked suspect clues (their ID, not the actual text)

    [Header("List of Buttons")]
    Button[] buttons;


    //deactivate all walk buttons on game start.
    private void Start()
    {
        buttons = FindObjectsOfType<Button>();
        GameObject[] temp = GameObject.FindGameObjectsWithTag("WalkButton");
        foreach(GameObject g in temp)
        {
            walkButtons.Add(g.GetComponent<Button>());
        }
        walkModeToggle();
    }

    //Main Method
    private void Update()
    {
        transform.position = currentRoom.transform.position;
        
        //Loading Panels
        if (loadPanel)
        {
            Panel.transform.position = Vector3.Lerp(Panel.transform.position, transform.position + new Vector3(0.1f, 0, 0), Time.deltaTime * 5);
        }

        else
        {
            Panel.transform.position = Vector3.Lerp(Panel.transform.position, transform.position + new Vector3(-18, 0, 0), Time.deltaTime * 5);
        }
    }


    //Coroutine for screen transition
    //Loops for 2 seconds, lowering opacity, swaps currentRoom object, and makes the new object visible.
    IEnumerator RoomTransition(GameObject room)
    {
        walkModeToggle();
        foreach (Button button in buttons)
        {
            button.interactable = false;
        }
        Time.timeScale = 0;
        for (float i = 1; i >= 0; i -= Time.unscaledDeltaTime*2)
        {
            currentRoom.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, i);
            yield return null;
        }
        currentRoom = room;
        for (float i = 0; i < 1; i += Time.unscaledDeltaTime/3*2)
        {
            currentRoom.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, i);
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
        StartCoroutine(RoomTransition(room));
    }

    //Method to toggle walking mode. Runs through for loop to set buttons to active, and deactivates them when walk mode is toggled again.
    public void walkModeToggle()
    {
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
        GameObject.FindGameObjectWithTag("WalkModeToggle").GetComponent<Button>().interactable = true;
    }

    //Method to Load the UI Panels
    public void UnloadPanel()
    {
        loadPanel = false;
        profilePanel.SetActive(false);
        cluesPanel.SetActive(false);
        mapPanel.SetActive(false);
        suspectPanel.SetActive(false);
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
        
        // if the GameManager has unlocked the ID for the first clue
        if (suspectClues.Contains(curSuspect.clues[0].unlockId))
        {
            clueElements[0].SetClue(curSuspect.clues[0].picture, curSuspect.clues[0].text);
        }
        else
        {
            clueElements[0].HideClue();
        }
        
        // if the GameManager has unlocked the ID for the second clue
        if (suspectClues.Contains(curSuspect.clues[1].unlockId))
        {
            clueElements[1].SetClue(curSuspect.clues[1].picture, curSuspect.clues[1].text);
        }
        else
        {
            clueElements[1].HideClue();
        }
    }

    public static bool UnlockClue(string _id)
    {
        bool wasUnlocked = suspectClues.Contains(_id);
        if (!wasUnlocked)
        {
            suspectClues.Add(_id);
        }

        return wasUnlocked;
    }
}
