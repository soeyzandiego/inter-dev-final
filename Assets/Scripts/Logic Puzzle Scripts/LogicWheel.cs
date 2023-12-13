using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LogicWheel : MonoBehaviour
{
    public int rightEvidence; // the index of the correct string. Feel free to change this into a string and just use .Equals().

    public bool correctEvidence; // is the current index at the correct evidence.

    [SerializeField] TMP_Text displayedString;

    //public GameObject[] evidenceCards;
    //anything related to this is physical implementation of the evidence cards in the looping lists found in the logic puzzle. This is currently not being implemented, in favor of a simpler string replacement.

    //[SerializeField] GameObject evidenceCardPrefab;
    //anything related to this is physical implementation of the evidence cards in the looping lists found in the logic puzzle. This is currently not being implemented, in favor of a simpler string replacement.

    public string[] evidence = new string[4]; // the strings associated with this wheels logic. Should be 4 long.

    // Scroll Arrows
    [SerializeField] GameObject upArrowPrefab;
    [SerializeField] GameObject downArrowPrefab;
    [SerializeField] GameObject[] scrollButtons = new GameObject[2];

    [SerializeField] int currentIndex; // the index currently being displayed


    // Start is called before the first frame update
    void Start()
    {
        displayedString = gameObject.GetComponentInChildren<TMP_Text>(); // gets reference to text that we can change

        scrollButtons[0] = Instantiate(upArrowPrefab, transform.parent);
        scrollButtons[0].transform.position = transform.position + new Vector3(0, 1, 0);
        Button temp = scrollButtons[0].GetComponent<Button>();
        temp.onClick.AddListener(() => { ScrollUp(); });
        scrollButtons[1] = Instantiate(downArrowPrefab, transform.parent);
        scrollButtons[1].transform.position = transform.position + new Vector3(0, -1, 0);
        temp = scrollButtons[1].GetComponent<Button>();
        temp.onClick.AddListener(() => { ScrollDown(); });

        if (currentIndex == rightEvidence)
        {
            correctEvidence = true;
        }

        /*
        for (int i = 0; i < 4; i++)
        {
            evidenceCards[i] = Instantiate(evidenceCardPrefab);
        }
        */
    }

    // Increments current index and displays the text by editing the previous displayedString object.
    public void ScrollUp()
    {
        currentIndex++;
        if(currentIndex >= evidence.Length)
        {
            currentIndex = 0;
        }
        displayedString.text = evidence[currentIndex];
        if (currentIndex == rightEvidence)
        {
            correctEvidence = true;
            Debug.Log("correct desc");
        }
        else
        {
            correctEvidence = false;
            Debug.Log("incorrect desc");
        }
    }
    public void ScrollDown()
    {
        currentIndex--;
        if (currentIndex < 0)
        {
            currentIndex = 3;
        }
        displayedString.text = evidence[currentIndex];
        if(currentIndex == rightEvidence)
        {
            correctEvidence = true;
        }
        else
        {
            correctEvidence = false;
        }
    }
}
