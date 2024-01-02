using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EFUnlockResearchArea : EventFlag
{
    [SerializeField] string clueID;

    [Header("Event")]
    [SerializeField] GameObject researchAreaButton;
    [SerializeField] GameObject bert;
    [SerializeField] GameObject colby;

    public override string ClueID
    {
        get { return clueID; }
        set { clueID = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        researchAreaButton.SetActive(false);
        bert.SetActive(true);
        colby.SetActive(true);
    }

    public override void Event()
    {
        GameManager.walkButtons.Add(researchAreaButton.GetComponent<Button>());
        bert.SetActive(false);
        colby.SetActive(false);
        Debug.Log("unlocked research area event");
    }
}
