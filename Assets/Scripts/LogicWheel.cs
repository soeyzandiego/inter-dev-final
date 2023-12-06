using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class LogicWheel : MonoBehaviour
{
    public bool rightEvidence = false;

    [SerializeField] TextMeshPro displayedString;
    //public GameObject[] evidenceCards;
    //anything related to this is physical implementation of the evidence cards in the looping lists found in the logic puzzle. This is currently not being implemented, in favor of a simpler string replacement.

    //[SerializeField] GameObject evidenceCardPrefab;
    //anything related to this is physical implementation of the evidence cards in the looping lists found in the logic puzzle. This is currently not being implemented, in favor of a simpler string replacement.

    public string[] evidence = new string[4]; // the strings associated with this wheels logic. Should be 4 long.

    [SerializeField] int correctEvidence; // the index of the correct string. Feel free to change this into a string and just use .Equals().

    [SerializeField] int currentIndex; // the index currently being displayed


    // Start is called before the first frame update
    void Start()
    {
        displayedString = gameObject.GetComponent<TextMeshPro>();
        /*
        for (int i = 0; i < 4; i++)
        {
            evidenceCards[i] = Instantiate(evidenceCardPrefab);
        }
        */
    }

    // Increments current index and displays the text by editing the previous displayedString object.
    public void scrollUp()
    {
        currentIndex++;
        if(currentIndex >= evidence.Length)
        {
            currentIndex = 0;
        }
        displayedString.text = evidence[currentIndex];
        if (currentIndex == correctEvidence)
        {
            rightEvidence = true;
        }
        else
        {
            rightEvidence = false;
        }
    }
    public void scrollDown()
    {
        currentIndex--;
        if (currentIndex < 0)
        {
            currentIndex = 3;
        }
        displayedString.text = evidence[currentIndex];
        if(currentIndex == correctEvidence)
        {
            rightEvidence = true;
        }
        else
        {
            rightEvidence = false;
        }
    }
}
