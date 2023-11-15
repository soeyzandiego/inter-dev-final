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


    //Initialize variables
    bool LoadPanel = false;
    bool walkMode = true;
    public List<GameObject> walkButtons = new List<GameObject>(); //List of game objects to be iterated through when walkmode is turned on


    //deactivate all walk buttons on game start.
    private void Start()
    {
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
        foreach (GameObject button in walkButtons)
        {
            button.SetActive(walkMode);
        }
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
