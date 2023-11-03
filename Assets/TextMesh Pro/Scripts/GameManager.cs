using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class GameManager : MonoBehaviour
{

    //receive Game Objects
    public GameObject ProfilePanel;

    //variables
    bool LoadProfile = false;


    public void LoadProfilePanel()
    {
        LoadProfile = true;
        
    }

    private void Update()
    {
        if (LoadProfile)
        {
            ProfilePanel.transform.position = Vector3.Lerp(ProfilePanel.transform.position, new Vector3(0.1f, 0, 0), Time.deltaTime * 5);
        }
    }
}
