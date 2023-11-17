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

    public GameObject ProfilePanel;
    public GameObject CluesPanel;
    public GameObject MapPanel;
    public GameObject SallyPanel;
    public GameObject ResearcherPanel;

    public GameObject currentRoom;

    [Header("Suspect Profiles")]
    public SuspectFile[] suspects;

    //Initialize variables
    bool LoadPanel = false;
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
        if (LoadPanel)
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
    IEnumerator roomTransition(GameObject room)
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
        StartCoroutine(roomTransition(room));
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
        LoadPanel = false;
        ProfilePanel.SetActive(false);
        CluesPanel.SetActive(false);
        MapPanel.SetActive(false);
        SallyPanel.SetActive(false);
        ResearcherPanel.SetActive(false);
    }

    public void LoadProfilePanel()
    {
        LoadPanel = true;
        ProfilePanel.SetActive(true);
        CluesPanel.SetActive(false);
        MapPanel.SetActive(false);
        SallyPanel.SetActive(false);
        ResearcherPanel.SetActive(false);

    }

    public void LoadCluesPanel()
    {
        LoadPanel = true;
        CluesPanel.SetActive(true);
        MapPanel.SetActive(false);
        ProfilePanel.SetActive(false);
        SallyPanel.SetActive(false);
        ResearcherPanel.SetActive(false);

    }

    public void LoadMapPanel()
    {
        LoadPanel = true;
        MapPanel.SetActive(true);
        CluesPanel.SetActive(false);
        ProfilePanel.SetActive(false);
        SallyPanel.SetActive(false);
        ResearcherPanel.SetActive(false);

    }

    public void LoadSallyPanel()
    {
        LoadPanel = true;
        SallyPanel.SetActive(true);
        MapPanel.SetActive(false);
        CluesPanel.SetActive(false);
        ProfilePanel.SetActive(false);
        ResearcherPanel.SetActive(false);

    }

    public void LoadResearcherPanel()
    {
        LoadPanel = true;
        ResearcherPanel.SetActive(true);
        MapPanel.SetActive(false);
        CluesPanel.SetActive(false);
        ProfilePanel.SetActive(false);
        SallyPanel.SetActive (false);

    }
}
