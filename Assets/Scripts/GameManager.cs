using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class GameManager : MonoBehaviour
{

    //Receive Game Objects
    public GameObject Panel;
    public GameObject ProfilePanel;
    public GameObject CluesPanel;
    public GameObject MapPanel;

    //Initialize variables
    bool LoadPanel = false;


    //Main Method
    private void Update()
    {
        if (LoadPanel)
        {
            Panel.transform.position = Vector3.Lerp(Panel.transform.position, new Vector3(0, 0, 0), Time.deltaTime * 5);
        }

        else
        {
            Panel.transform.position = Vector3.Lerp(Panel.transform.position, new Vector3(-18, 0, 0), Time.deltaTime * 5);
        }
    }

    //Method to Load the UI Panels
    public void LoadProfilePanel()
    {
        LoadPanel = true;
        ProfilePanel.SetActive(true);
        CluesPanel.SetActive(false);
        MapPanel.SetActive(false);

    }

    public void LoadCluesPanel()
    {
        LoadPanel = true;
        CluesPanel.SetActive(true);
        MapPanel.SetActive(false);
        ProfilePanel.SetActive(false);

    }

    public void LoadMapPanel()
    {
        LoadPanel = true;
        MapPanel.SetActive(true);
        CluesPanel.SetActive(false);
        ProfilePanel.SetActive(false);

    }

    public void UnloadPanel()
    {
        LoadPanel = false;
        ProfilePanel.SetActive(false);
        CluesPanel.SetActive(false);
        MapPanel.SetActive(false);
    }
}
